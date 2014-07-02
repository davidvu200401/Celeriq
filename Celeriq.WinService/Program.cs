using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Celeriq.WinService
{
    internal static class Program
    {
        private static ManualResetEvent m_daemonUp = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            if (args.Any(x => x == "-console") || args.Any(x => x == "/console"))
            {
                var service = new PersistentService();
                service.Start();
                Console.WriteLine("Press <ENTER> to stop...");
                Console.ReadLine();
                service.Cleanup();
                service.Stop();
            }
            else
            {
                var ServicesToRun = new ServiceBase[]
                                                            {
                                                                new PersistentService()
                                                            };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}