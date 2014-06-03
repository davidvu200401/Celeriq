using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Celeriq.Common
{
    public interface IRepositoryItem
    {
        long __RecordIndex { get; }
        DateTime __CreateDate { get; }
    }
}