using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Celeriq.Utilities;

namespace Celeriq.Server.Core
{
    internal class AutoLoader
    {
        private SystemCore _system = null;
        public AutoLoader(SystemCore system)
        {
            _system = system;
        }

        public void Run()
        {
            try
            {
                var timer = new Stopwatch();
                timer.Start();

                //Get a list to work from
                var idList = _system.Manager.List.Select(x => x.Repository.ID).ToList();

                var count = 0;
                foreach (var id in idList)
                {
                    //Check to make sure it exists (may have been removed)
                    if (_system.Manager.List.Any(x => x.Repository.ID == id))
                    {
                        _system.Manager.LoadData(id, _system.RootUser);
                        count++;
                    }
                }

                timer.Stop();
                Logger.LogInfo("AutoLoader Complete: Count=" + count + ", Elapsed=" + timer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}
