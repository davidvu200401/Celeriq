using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Celeriq.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Gets a refinement value from dimension refinement item, if it exists
        /// </summary>
        /// <param name="dItem">The dimension on which this refinement resides</param>
        /// <param name="dvidx">The unique identifier of the refinement</param>
        /// <returns></returns>
        public static string GetRefinementValue(this DimensionItem dItem, long dvidx)
        {
            if (dItem != null)
            {
                var rItem = dItem.RefinementList.FirstOrDefault(x => x.DVIdx == dvidx);
                if (rItem != null) return rItem.FieldValue;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the dimenion item on which the specified refinement exists
        /// </summary>
        /// <param name="dimensionList">A list of dimensions to check</param>
        /// <param name="dvidx">The unique identifier of the refinement</param>
        /// <returns></returns>
        public static DimensionItem GetDimensionByDVIdx(this IEnumerable<DimensionItem> dimensionList, long dvidx)
        {
            foreach (var dItem in dimensionList)
            {
                var rItem = dItem.RefinementList.FirstOrDefault(x => x.DVIdx == dvidx);
                if (rItem != null) return dItem;
            }
            return null;
        }

        /// <summary>
        /// Gets the dimenion item on which the specified refinement value exists
        /// </summary>
        /// <param name="dimensionList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DimensionItem GetDimensionByRefinementValue(this IEnumerable<DimensionItem> dimensionList, string value)
        {
            if (dimensionList == null) return null;
            foreach (var dItem in dimensionList)
            {
                var rItem = dItem.RefinementList.FirstOrDefault(x => x.FieldValue == value);
                if (rItem != null) return dItem;
            }
            return null;
        }

        public static RefinementItem GetRefinementByValue(this DimensionItem dimension, string value)
        {
            if (dimension == null) return null;
            if (value == null) return null;
            var rItem = dimension.RefinementList.FirstOrDefault(x => x.FieldValue == value);
            if (rItem != null) return rItem;
            return null;
        }

        public static RefinementItem GetRefinementByMinValue(this DimensionItem dimension, long minValue)
        {
            if (dimension == null) return null;
            var rItem = dimension.RefinementList.FirstOrDefault(x => x.MinValue == minValue);
            if (rItem != null) return rItem;
            return null;
        }

        /// <summary>
        /// Given a refinement value find the first associated refinement item and returns its unique DVIdx
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long? GetDVIdxByValue(this DimensionItem dimension, string value)
        {
            if (dimension == null) return null;
            if (value == null) return null;
            var rItem = dimension.RefinementList.FirstOrDefault(x => x.FieldValue == value);
            if (rItem != null) return rItem.DVIdx;
            return null;
        }

        /// <summary>
        /// Returns a refinement item from a list of dimenions based on the unqiue dimension value index
        /// </summary>
        /// <param name="dimensionList"></param>
        /// <param name="dvidx"></param>
        /// <returns></returns>
        public static RefinementItem GetRefinementByDVIdx(this IEnumerable<DimensionItem> dimensionList, long dvidx)
        {
            foreach (var dItem in dimensionList)
            {
                var rItem = dItem.RefinementList.FirstOrDefault(x => x.DVIdx == dvidx);
                if (rItem != null) return rItem;
            }
            return null;
        }

        /// <summary>
        /// Trims the value parameter off both ends of a string
        /// </summary>
        public static string Trim(this string s, string value)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (string.IsNullOrEmpty(value)) return s;

            while (s.StartsWith(value))
            {
                s = s.Substring(value.Length, s.Length - value.Length);
            }

            while (s.EndsWith(value))
            {
                s = s.Substring(0, s.Length - value.Length);
            }

            return s;
        }

        public static string ToXml(object obj)
        {
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.IndentChars = "\t";
                var xmlWriter = XmlWriter.Create(writer, settings);

                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add(string.Empty, "http://www.w3.org/2001/XMLSchema");

                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(xmlWriter, obj, ns);
                return writer.ToString();
            }
        }

        public static object FromXml(string s, System.Type type)
        {
            var reader = new StringReader(s);
            var serializer = new XmlSerializer(type);
            var xmlReader = new XmlTextReader(reader);
            return serializer.Deserialize(xmlReader);
        }

        public static string ToCeleriqDateString(this DateTime? d)
        {
            if (d == null) return string.Empty;
            return d.Value.ToString(DimensionItem.DateTimeFormat);
        }

    }
}