using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoadTest
{
    public class DataSetup
    {
        public string[] Dim1Array = { "AAA", "BBB", "CCC", "DDD", "EEE", "FFF" };
        public string[] Dim2Array = { "ZZZ", "XXX", "YYY", "WWW" };
        public List<string> Dim3Array = new List<string>();
        public List<string> Dim4Array = new List<string>();
        public List<Guid> PredefinedLoad { get; private set; }

        private Random _rnd = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public DataSetup()
        {
            this.PredefinedLoad = new List<Guid>();

            #region Setup
            for (var ii = 0; ii < 10; ii++)
            {
                Dim3Array.Add(RandomString(5));
                Dim4Array.Add(RandomString(5));
            }
            #endregion
        }

        public string RandomString(int size)
        {
            if (size < 1) size = 1;
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rnd.Next(_chars.Length)];
            }
            return new string(buffer);
        }

    }
}
