using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Celeriq.Agent
{
    static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Any(x => x == "-console"))
            {
                var service = new AgentService();
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
                                                                new AgentService()
                                                            };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}