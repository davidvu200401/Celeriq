using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Celeriq.Utilities
{
    public static class FileHelper
    {
        public static int GetDiskAccessSpeed()
        {
            var elapsed = -1;
            var filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                var timer = new Stopwatch();
                timer.Start();
                using (var stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                }
                timer.Stop();
                elapsed = (int)timer.ElapsedMilliseconds;
                File.Delete(filename);
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
            return elapsed;
        }

    }
}
