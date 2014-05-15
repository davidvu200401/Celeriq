#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Celeriq.Utilities
{
    /// <summary />
    public static class ObjectHelper
    {
        /// <summary>
        /// Determines the amount of memory an object uses
        /// </summary>
        public static long SizeOf(object o, bool useDiskBuffer = false)
        {
            if (o == null) return 0;
            try
            {
                var timer = new Stopwatch();
                timer.Start();
                long retval = -1;
                if (useDiskBuffer)
                {
                    var fileName = Path.GetTempFileName();
                    using (var fs = File.Create(fileName))
                    {
                        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        formatter.Serialize(fs, o);
                        retval = fs.Length;
                    }
                    if (File.Exists(fileName)) File.Delete(fileName);
                }
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        formatter.Serialize(ms, o);
                        retval = ms.Length;
                    }
                }
                timer.Stop();
                Logger.LogInfo("SizeOf (" + useDiskBuffer + "): Size=" + retval + ", Elapsed=" + timer.ElapsedMilliseconds);
                return retval;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        
        //public static long SizeOf(object o)
        //{
        //    if (o == null) return 0;
        //    var fileName = Path.GetTempFileName();
        //    try
        //    {
        //        using (var fs = File.Create(fileName))
        //        {
        //            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //            formatter.Serialize(fs, o);
        //            return fs.Length;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }
        //    finally
        //    {
        //        if (File.Exists(fileName))
        //            File.Delete(fileName);
        //    }
        //}

    }
}