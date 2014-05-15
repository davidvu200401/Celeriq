using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace Celeriq.Utilities
{
    /// <summary />
    public static class Extensions
    {
        internal static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// Concatenate Lambdas using the AND operation
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        /// <summary>
        /// Concatenate Lambdas using the OR operation
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        /// <summary>
        /// Can compare 2 strings with case insensitivity
        /// </summary>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            if (string.IsNullOrEmpty(source)) return false;
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static byte[] Zip(this string value)
        {
            var byteArray = System.Text.Encoding.UTF8.GetBytes(value);
            return byteArray.ZipBytes();
        }

        public static byte[] ZipBytes(this byte[] byteArray)
        {
            try
            {
                //Prepare for compress
                using (var ms = new System.IO.MemoryStream())
                {
                    using (var sw = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress))
                    {
                        //Compress
                        sw.Write(byteArray, 0, byteArray.Length);
                        sw.Close();

                        //Transform byte[] zip data to string
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string Unzip(this byte[] byteArray)
        {
            //If null stream return null string
            if (byteArray == null)
                return null;

            //If NOT compressed then return string, no de-compression
            if (byteArray.Length > 3 && (byteArray[0] == 31 && byteArray[1] == 139 && byteArray[2] == 8))
            {
                //Compressed
            }
            else
            {
                var xml = System.Text.Encoding.Unicode.GetString(byteArray);

                // Check for byte order mark
                if (xml.StartsWith("<") || xml[0] == 0xfeff)
                {
                    xml = System.Text.RegularExpressions.Regex.Replace(xml, @"[^\u0000-\u007F]", string.Empty);
                    return xml;
                }
                else
                {
                    return System.Text.Encoding.UTF8.GetString(byteArray);
                }
            }
            return System.Text.Encoding.UTF8.GetString(byteArray.UnzipBytes());
        }

        public static byte[] UnzipBytes(this byte[] byteArray)
        {
            //If null stream return null string
            if (byteArray == null)
                return null;

            try
            {
                using (var memoryStream = new MemoryStream(byteArray))
                {
                    using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        using (var writerStream = new MemoryStream())
                        {
                            byteArray = new byte[1024];
                            int readBytes;
                            while ((readBytes = gZipStream.Read(byteArray, 0, byteArray.Length)) != 0)
                            {
                                writerStream.Write(byteArray, 0, readBytes);
                            }
                            gZipStream.Close();
                            memoryStream.Close();
                            return writerStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary />
        public static byte[] ObjectToBin(this object obj)
        {
            if (obj == null) throw new Exception("Object cannot be null");
            try
            {
                //Open stream and move to end for writing
                using (var stream = new MemoryStream())
                {
                    stream.Seek(0, SeekOrigin.End);
                    var formatter = new BinaryFormatter();
                    formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                    formatter.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;
                    formatter.Binder = new versionConfigToNamespaceAssemblyObjectBinder();
                    formatter.Serialize(stream, obj);
                    stream.Close();
                    return stream.ToArray().ZipBytes();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary />
        public static T BinToObject<T>(this byte[] data)
        {
            try
            {
                var formatter = new BinaryFormatter();
                formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                formatter.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;
                formatter.Binder = new versionConfigToNamespaceAssemblyObjectBinder();
                using (var stream = new MemoryStream(data.UnzipBytes()))
                {
                    return (T)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region versionConfigToNamespaceAssemblyObjectBinder
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
        #endregion

        public static void Using<T>(this T client, Action<T> work)
            where T : System.ServiceModel.ICommunicationObject
        {
            try
            {
                work(client);
                client.Close();
            }
            catch (System.ServiceModel.EndpointNotFoundException e)
            {
                throw;
            }
            catch (System.ServiceModel.CommunicationException e)
            {
                client.Abort();
            }
            catch (TimeoutException e)
            {
                client.Abort();
            }
            catch (Exception e)
            {
                client.Abort();
                throw;
            }
        }

    }

}