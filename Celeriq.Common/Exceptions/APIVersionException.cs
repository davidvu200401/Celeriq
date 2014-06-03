using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Common.Exceptions
{
    [Serializable]
    public class APIVersionException : System.Exception
    {
        public APIVersionException() : base() { }

        public APIVersionException(long hash, long queryHash)
            : base()
        {
            this.Hash = hash;
            this.QueryHash = queryHash;
        }

        public long Hash { get; private set; }
        public long QueryHash { get; private set; }

        public override string Message
        {
            get { return "The generated API is out of date. The repository model has been changed. Please re-generate this code file. Computed: " + this.Hash + ", Returned: " + this.QueryHash; }
        }
    }

}
