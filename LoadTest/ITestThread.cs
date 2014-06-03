using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadTest
{
    public interface ITestThread
    {
        bool Cancel { get; set; }
        void Run();
    }
}
