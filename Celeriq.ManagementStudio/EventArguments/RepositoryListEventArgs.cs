using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Common;

namespace Celeriq.ManagementStudio.EventArguments
{
    internal class RepositoryListEventArgs : System.EventArgs
    {
        public RepositoryListEventArgs()
        {
        }

        public RepositoryListEventArgs(List<IRemotingObject> list)
            : this()
        {
            this.RepositoryList = new List<IRemotingObject>();
            this.RepositoryList.AddRange(list);
        }

        public List<IRemotingObject> RepositoryList { get; private set; }
    }
}