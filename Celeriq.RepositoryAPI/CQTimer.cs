using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Celeriq.RepositoryAPI
{
    internal class CQTimer
    {
        private DateTime _startTime;
        private Stopwatch _timer = new Stopwatch();
        private int _elapsed = 0;

        public CQTimer()
        {
            _startTime = DateTime.Now;
            _timer.Reset();
            _timer.Start();
        }

        public DateTime StartTime
        {
            get { return _startTime; }
        }

        public int Elapsed
        {
            get { return _elapsed; }
        }

        public int Stop()
        {
            _timer.Stop();
            _elapsed = (int) _timer.ElapsedMilliseconds;
            return _elapsed;
        }

    }
}