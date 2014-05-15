using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Common.Exceptions
{
    [Serializable]
    public class SchemaVersionException : System.Exception
    {
        public SchemaVersionException() : base() { }

    }

}
