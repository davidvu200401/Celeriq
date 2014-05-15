using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Utilities;

namespace LoadTest
{
    public class Junk
    {
        private CeleriqLock _locker = null;

        public Junk(CeleriqLock locker)
        {
            _locker = locker;
        }

        public void Run()
        {
            using (var q = new AcquireWriterLock(_locker, "q"))
            {
                System.Threading.Thread.Sleep(60000);
            }
        }
    }
}
