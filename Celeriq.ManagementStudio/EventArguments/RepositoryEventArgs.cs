using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Common;

namespace Celeriq.ManagementStudio.EventArguments
{
    internal class RepositoryEventArgs : System.EventArgs
    {
        public RepositoryEventArgs()
        {
        }

        public RepositoryEventArgs(IRemotingObject repository)
            : this()
        {
            this.Repository = repository;
        }

        public IRemotingObject Repository { get; set; }
    }
}