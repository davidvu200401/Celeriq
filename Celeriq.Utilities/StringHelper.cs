using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Utilities
{
    /// <summary />
    public class StringHelper
    {
        internal static string ConvertString(byte[] array)
        {
            var sb = new StringBuilder();
            foreach (var b in array)
                sb.Append((char) b);
            return sb.ToString();
        }

        internal static byte[] ConvertArray(string s)
        {
            if (s == null) return null;
            else
            {
            }
            var retval = new byte[s.Length];
            for (var ii = 0; ii < s.Length; ii++)
                retval[ii] = (byte) s[ii];
            return retval;
        }
    }
}