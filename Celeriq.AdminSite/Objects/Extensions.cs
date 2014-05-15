using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Celeriq.AdminSite.Objects
{
    public static class Extensions
    {
        public static string FormatNumber(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            int v;
            if (int.TryParse(s, out v))
                return v.ToString("###,###,##0");
            return s;
        }

        public static string FormatNumber(this int v)
        {
            return v.ToString("###,###,##0");
        }

        public static string FormatNumber(this long v)
        {
            return v.ToString("###,###,##0");
        }

    }
}