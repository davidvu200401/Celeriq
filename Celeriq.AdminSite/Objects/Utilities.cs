#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Celeriq.AdminSite.Objects
{
    internal static class Utilities
    {
        public static bool IsFileNameValid(string fileName)
        {
            try
            {
                var fi = new FileInfo(fileName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string ToSizeDisplay(long size)
        {
            const long KB = 1024;
            const long MB = 1024*KB;
            const long GB = 1024*MB;

            if (size > GB)
            {
                return (size/(GB*1.0)).ToString("###,###,###,##0.0") + " GB";
            }
            else if (size > MB)
            {
                return (size/(MB*1.0)).ToString("###,###,###,##0.0") + " MB";
            }
            else
            {
                return (size/(KB*1.0)).ToString("###,###,###,##0.0") + " KB";
            }
        }
    }
}