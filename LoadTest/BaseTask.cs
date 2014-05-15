using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.Common;

namespace LoadTest
{
    public abstract class BaseTask: ITestThread
    {
        protected UserCredentials _credentials = null;
        protected DataSetup _data = null;
        protected Random _rnd = new Random();
        protected int _threadCount = 0;
        protected int _maxPageCount = 0;

        public BaseTask(string server, UserCredentials credentials, DataSetup data, int threadCount)
        {
            _threadCount = threadCount;
            _credentials = credentials;
            _data = data;
            this.Server = server;
        }

        public abstract void Run();

        public bool Cancel { get; set; }
        public string Server { get; private set; }
    }
}
