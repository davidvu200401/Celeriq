#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Celeriq.Utilities
{
    /// <summary>
    /// This class provides a way to cache and retrieve serializable items to disk
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileCacheHelper<T> : System.IDisposable
    {
        private object locker = new object();
        private long _lastPosition = 0;
        private Rijndael _rijndaelAlg = Rijndael.Create();
        private ICryptoTransform _encryptor = null;

        /// <summary />
        public FileCacheHelper()
        {
            try
            {
                this.CacheFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary />
        public FileCacheHelper(string cacheFile)
            : this()
        {
            try
            {
                if (File.Exists(this.CacheFileName))
                    File.Delete(this.CacheFileName);

                if (!File.Exists(cacheFile))
                    File.CreateText(cacheFile).Close();

                this.CacheFileName = cacheFile;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private ICryptoTransform Encryptor
        {
            get
            {
                if (_encryptor == null)
                {
                    var machineId = SecurityHelper.GetMachineId();
                    if (string.IsNullOrEmpty(machineId))
                        machineId = "BB9671F439254223A6F3A71D9E67F95C";

                    //Using the machine key, make the proto key 32 bytes
                    var protoKey = machineId;
                    while (protoKey.Length < 32)
                    {
                        protoKey += machineId;
                    }
                    protoKey = protoKey.Substring(0, 32);

                    var key = new byte[32];
                    var iv = new byte[16];

                    //convert protokey to byte array
                    for (var ii = 0; ii < protoKey.Length; ii++)
                        key[ii] = (byte) protoKey[ii];

                    const string protoiv = "F439254223A6F3A7";

                    //convert protoiv to byte array
                    for (var ii = 0; ii < protoiv.Length; ii++)
                        iv[ii] = (byte) protoiv[ii];

                    _encryptor = _rijndaelAlg.CreateEncryptor(key, iv);
                }
                return _encryptor;
            }
        }

        /// <summary>
        /// The disk file where items are cached
        /// </summary>
        public string CacheFileName { get; private set; }

        /// <summary />
        public void WriteItem(T item)
        {
            var list = new List<T>();
            list.Add(item);
            WriteItem(list.ToArray());
        }

        /// <summary>
        /// Writes the item to the cache file
        /// </summary>
        public void WriteItem(T[] list)
        {
            lock (locker)
            {
                const int MAXTRY = 3;
                var retry = 0;
                while (retry < MAXTRY)
                {
                    try
                    {
                        //Open stream and move to end for writing
                        using (var stream = File.Open(this.CacheFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            stream.Seek(0, SeekOrigin.End);
                            var formatter = new BinaryFormatter();
                            formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                            formatter.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;
                            formatter.Binder = new versionConfigToNamespaceAssemblyObjectBinder();
                            foreach (var item in list)
                            {
                                formatter.Serialize(stream, item);
                            }
                            stream.Close(); //Force file flush
                            this.EOF = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        var folder = System.IO.Path.GetDirectoryName(this.CacheFileName);
                        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                        retry++;
                        if (retry >= MAXTRY) throw;
                    }
                }
            }
        }

        /// <summary>
        /// Writes the item to the cache file
        /// </summary>
        public void WriteEncryptedItem(T[] list)
        {
            //lock (locker)
            //{
            //	const int MAXTRY = 3;
            //	var retry = 0;
            //	while (retry < MAXTRY)
            //	{
            //		try
            //		{
            //			//Open stream and move to end for writing
            //			using (var stream = File.Open(this.CacheFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //			{
            //				using (var csEncrypt = new CryptoStream(stream, this.Encryptor, CryptoStreamMode.Write))
            //				{
            //						stream.Seek(0, SeekOrigin.End);
            //						var formatter = new BinaryFormatter();
            //						formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            //						formatter.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;
            //						formatter.Binder = new versionConfigToNamespaceAssemblyObjectBinder();
            //						foreach (var item in list)
            //						{
            //							formatter.Serialize(stream, item);
            //						}
            //						stream.Close(); //Force file flush
            //						this.EOF = true;
            //						return;
            //					}
            //				}
            //			}
            //		catch (Exception ex)
            //		{
            //			retry++;
            //			if (retry >= MAXTRY) throw;
            //		}
            //	}
            //}
        }

        /// <summary />
        public bool EOF { get; private set; }

        /// <summary>
        /// Gets the next object in the cache file
        /// </summary>
        public T ReadItem()
        {
            try
            {
                if (!File.Exists(this.CacheFileName))
                    throw new Exception("The cache file does not exist!");

                lock (locker)
                {
                    using (var stream = File.Open(this.CacheFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        stream.Seek(_lastPosition, SeekOrigin.Begin);
                        if (stream.Position == stream.Length)
                        {
                            this.EOF = true;
                            return default(T);
                        }
                        else
                        {
                            var formatter = new BinaryFormatter();
                            formatter.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;
                            formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                            formatter.Binder = new versionConfigToNamespaceAssemblyObjectBinder();
                            var retval = (T) formatter.Deserialize(stream);
                            _lastPosition = stream.Position;
                            stream.Close();
                            return retval;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Resets the file position to the begining
        /// </summary>
        public void ResetFile()
        {
            lock (locker)
            {
                this.EOF = false;
                _lastPosition = 0;
            }
        }

        /// <summary />
        public List<T> LoadAll(string fileName)
        {
            //Read 40MB chunks and move forward. When 4MB left, read another chunk
            const int DISKBLOCK = 4096; //4k
            var CHUNK = DISKBLOCK * 2500; //10 MB
            const int THRESHOLD = 1024 * 1024 * 1; //1 MB

            long lastPosition = 0;
            long absolutePosition = 0;
            try
            {
                var retval = new List<T>();
                var oneLoad = false; //if the file is small we will load the file into memory at one time
                if (File.Exists(fileName))
                {
                    using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        if (fs.Length > 0)
                        {
                            if (fs.Length < CHUNK)
                            {
                                CHUNK = (int)fs.Length;
                                oneLoad = true;
                            }

                            var array = new byte[CHUNK];
                            var bytesRead = fs.Read(array, 0, CHUNK);
                            using (var stream = new MemoryStream(array))
                            {
                                while (absolutePosition < fs.Length)
                                {
                                    stream.Seek(lastPosition, SeekOrigin.Begin);
                                    if (stream.Position != stream.Length)
                                    {
                                        var formatter = new BinaryFormatter();
                                        var item = (T)formatter.Deserialize(stream);
                                        absolutePosition += (stream.Position - lastPosition);
                                        lastPosition = stream.Position;
                                        retval.Add(item);
                                    }

                                    if (!oneLoad && (bytesRead == CHUNK) && ((CHUNK - stream.Position) < THRESHOLD))
                                    {
                                        fs.Seek(absolutePosition, SeekOrigin.Begin);
                                        var read = fs.Read(array, 0, CHUNK);
                                        if (read != 0)
                                        {
                                            //Something read
                                            stream.Position = 0;
                                            lastPosition = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return retval;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "lastPosition=" + lastPosition + " , absolutePosition=" + absolutePosition);
                throw;
            }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            //try
            //{
            //  if (File.Exists(this.CacheFileName))
            //  {
            //    File.Delete(this.CacheFileName);
            //  }
            //}
            //catch (Exception ex)
            //{
            //  //Do Nothing
            //}
        }

        #endregion

        private sealed class versionConfigToNamespaceAssemblyObjectBinder : System.Runtime.Serialization.SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Type typeToDeserialize = null;
                try
                {
                    var ToAssemblyName = assemblyName.Split(',')[0];
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var ass in assemblies)
                    {
                        if (ass.FullName.Split(',')[0] == ToAssemblyName)
                        {
                            typeToDeserialize = ass.GetType(typeName);
                            break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw;
                }
                return typeToDeserialize;
            }
        }
    }
}