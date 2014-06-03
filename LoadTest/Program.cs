using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Celeriq.Common;
using Celeriq.Utilities;

namespace LoadTest
{
    class Program
    {
        private static UserCredentials _credentials = null;
        private static DataSetup _data = new DataSetup();

        static void Main(string[] args)
        {

            //var locker = new CeleriqLock(System.Threading.LockRecursionPolicy.SupportsRecursion);
            //var o = new Junk(locker);
            //var tt = new Thread(new ThreadStart(o.Run));
            //tt.Start();
            //System.Threading.Thread.Sleep(1000);

            //var w = new AcquireWriterLock(locker, "w");
            //return;



            var server = string.Empty;
            var threadCreate = 0;
            var threadDelete = 0;
            var threadQuery = 0;
            var threadSingle = 0;

            //Server
            {
                var v = args.FirstOrDefault(x => x.StartsWith("/server"));
                if (!string.IsNullOrEmpty(v))
                {
                    var arr = v.Split(new char[] { ':' });
                    if (arr.Length == 2) server = arr[1];
                }
            }

            //Create
            {
                var v = args.FirstOrDefault(x => x.StartsWith("/create"));
                if (!string.IsNullOrEmpty(v))
                {
                    var arr = v.Split(new char[] { ':' });
                    if (arr.Length == 2) threadCreate = Convert.ToInt32(arr[1]);
                }
            }

            //Delete
            {
                var v = args.FirstOrDefault(x => x.StartsWith("/delete"));
                if (!string.IsNullOrEmpty(v))
                {
                    var arr = v.Split(new char[] { ':' });
                    if (arr.Length == 2) threadDelete = Convert.ToInt32(arr[1]);
                }
            }

            //Query
            {
                var v = args.FirstOrDefault(x => x.StartsWith("/query"));
                if (!string.IsNullOrEmpty(v))
                {
                    var arr = v.Split(new char[] { ':' });
                    if (arr.Length == 2) threadQuery = Convert.ToInt32(arr[1]);
                }
            }

            //Single
            {
                var v = args.FirstOrDefault(x => x.StartsWith("/single"));
                if (!string.IsNullOrEmpty(v))
                {
                    var arr = v.Split(new char[] { ':' });
                    if (arr.Length == 2) threadSingle = Convert.ToInt32(arr[1]);
                }
            }

            //Load File
            {
                const string loadFilePrompt = "/loadfile:";
                var v = args.FirstOrDefault(x => x.StartsWith(loadFilePrompt));
                if (!string.IsNullOrEmpty(v))
                {
                    v = v.Substring(loadFilePrompt.Length, v.Length - loadFilePrompt.Length);
                    if (File.Exists(v))
                    {
                        var g = File.ReadAllLines(v);
                        foreach (var s in g)
                        {
                            _data.PredefinedLoad.Add(new Guid(s));
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(server))
            {
                Console.WriteLine("Invalid server.");
                return;
            }

            if (threadCreate + threadDelete + threadQuery + threadSingle == 0)
            {
                Console.WriteLine("There is nothing to do.");
                return;
            }

            Console.WriteLine("Create: " + threadCreate + " / Delete: " + threadDelete + " / Query: " + threadQuery + " / Single: " + threadSingle);

            var threadList = new List<ITestThread>();
            System.Threading.Thread.Sleep(2000);

            var credentials = GetCredentials(server);

            {
                var t = new CreateThread(server, _credentials, _data, threadCreate) { RepositoryTemplate = GetRepositoryTemplate() };
                threadList.Add(t);
                if (threadCreate > 0)
                    ThreadPool.QueueUserWorkItem(delegate { t.Run(); }, null);
            }

            {
                var t = new DeleteThread(server, _credentials, _data, threadDelete);
                threadList.Add(t);
                if (threadDelete > 0)
                    ThreadPool.QueueUserWorkItem(delegate { t.Run(); }, null);
            }

            {
                var t = new QueryThread(server, _credentials, _data, threadQuery);
                threadList.Add(t);
                if (threadQuery > 0)
                    ThreadPool.QueueUserWorkItem(delegate { t.Run(); }, null);
            }

            {
                var t = new SingleChangeThread(server, _credentials, _data, threadSingle);
                threadList.Add(t);
                if (threadSingle > 0)
                    ThreadPool.QueueUserWorkItem(delegate { t.Run(); }, null);
            }

            Console.WriteLine("Press <ENTER> to end...");
            Console.ReadLine();

            threadList.ForEach(x => x.Cancel = true);
        }

        private static UserCredentials GetCredentials(string machine)
        {
            if (_credentials == null)
            {
                _credentials = new UserCredentials();
                using (var factory = SystemCoreInteractDomain.GetFactory(machine))
                {
                    var server = factory.CreateChannel();
                    _credentials.UserName = "root";
                    _credentials.Password = "password";
                    _credentials.Password = Celeriq.Utilities.SecurityHelper.Encrypt(server.GetPublicKey(), _credentials.Password);
                }
            }
            return _credentials;
        }

        private static string GetRepositoryTemplate()
        {
            var a = Assembly.GetExecutingAssembly();
            var st = a.GetManifestResourceStream("LoadTest.LoadTest.celeriq");
            var sr = new StreamReader(st);
            return sr.ReadToEnd();
        }

    }
}