using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using Celeriq.Common;

namespace Celeriq.Utilities.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestHashTable1();
            //TestHash();
            //TestSecurity();

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static void TestSecurity()
        {
            var ww = SecurityHelper.GetMachineId();
        }


        private static void TestHash()
        {
            var q = SecurityHelper.GetHashRaw("hello there. I am here sd sd asd as das d asd as d");
            var w = System.Text.Encoding.Default.GetString(q);
            var e = BitConverter.ToString(q).Replace("-", "");
            var qq = SecurityHelper.GetHash("hello there. I am here sd sd asd as das d asd as d", true);
        }

        private static void TestHashTable1()
        {
            var resultsCache = new SequencedHashTable<int, DataQueryResults>();

            var rnd = new Random(0);

            const int MAXITEMS = 3;
            const int LOOPS = 2000;
            for (var ii = 0; ii < LOOPS; ii++)
            {
                //var k1 = rnd.Next(1, 1000);
                var k1 = (ii % 4);
                var item = new DataQueryResults();

                if (resultsCache.Count >= MAXITEMS)
                {
                    var k = resultsCache.OrderedKeys.FirstOrDefault();
                    if (k != null) resultsCache.Remove(k);
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Key not found. 0x2998");
                    }
                }

                resultsCache.Add(k1, item);

                var k2 = resultsCache.OrderedKeys.FirstOrDefault();
                if (k2 == null)
                {
                    System.Diagnostics.Debug.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Cached items: " + resultsCache.Count + " / Key: " + k1 + " / Key Count: " + resultsCache.OrderedKeys.Count());
                }

            }

        }
    }
}