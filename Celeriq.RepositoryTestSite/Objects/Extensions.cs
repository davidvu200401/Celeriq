using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Celeriq.RepositoryTestSite.Objects
{
    internal static class Extensions
    {
        /// <summary>
        /// Add commas to a number
        /// </summary>
        public static string FormatNumber(this int number)
        {
            return number.ToString("###,###,###,##0");
        }

    }
}