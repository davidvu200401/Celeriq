#pragma warning disable 0168
using System;
using System.Collections.Generic;

namespace Celeriq.RepositoryTestSite.Objects
{
    /// <summary>
    /// A class that parses a URL structure
    /// </summary>
    public class URL : ICloneable
    {
        #region Class Members

        protected string _page = "";
        protected URLParameterCollection _parameters = new URLParameterCollection();
        protected Dictionary<string, string> _defaults = new Dictionary<string, string>();

        #endregion

        #region Constructors

        public URL()
        {
        }

        public URL(string url)
            : this()
        {
            if (string.IsNullOrEmpty(url))
                return;
            var arr = url.Split('?');
            _page = arr[0];

            if (arr.Length >= 2)
            {
                var list = System.Web.HttpUtility.ParseQueryString(arr[1], System.Text.Encoding.ASCII);
                foreach (var key in list.AllKeys)
                {
                    if (!string.IsNullOrEmpty(key))
                        this.Parameters.Add(new URLParameter(key, list[key]));
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The page name without any parameters
        /// </summary>
        public virtual string Page
        {
            get { return _page; }
            set { _page = value; }
        }

        /// <summary>
        /// A collection of query parameters
        /// </summary>
        public URLParameterCollection Parameters
        {
            get { return _parameters; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a default value that will be used when building the URL string
        /// Values that are the same as defaults will not be appended to the URL string
        /// </summary>
        public void AddDefault(string key, string value)
        {
            if (_defaults.ContainsKey(key.ToLower()))
                _defaults[key.ToLower()] = value;
            else
                _defaults.Add(key.ToLower(), value);
        }

        /// <summary>
        /// The string representation of the URL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var retval = this.Page + string.Empty;
            var qs = this.QueryString;
            if (!string.IsNullOrEmpty(qs))
                retval += "?" + qs;
            return retval;
        }

        public string QueryString
        {
            get
            {
                try
                {
                    var retval = string.Empty;
                    if (this.Parameters.Count != 0)
                    {
                        foreach (var item in this.Parameters)
                        {
                            if (!(_defaults.ContainsKey(item.Name.ToLower()) && _defaults[item.Name.ToLower()] == item.Value))
                                retval += Microsoft.Security.Application.Encoder.UrlEncode(item.Name) + "=" + Microsoft.Security.Application.Encoder.UrlEncode(item.Value) + "&";
                        }
                    }

                    if (retval.EndsWith("&"))
                        retval = retval.Substring(0, retval.Length - 1);

                    return retval;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            var retval = new URL();
            retval.Page = this.Page;
            retval.Parameters.AddRange(this.Parameters);
            return retval;
        }

        #endregion

        /// <summary>
        /// Adds a new parameter if not already existing, or updates existing
        /// </summary>
        /// <param name="name">Name of query parameter</param>
        /// <param name="value">New value to assign to parameter</param>
        public void SetParameter(string name, string value)
        {
            var param = Parameters.Find(x => x.Name == name);
            if (param == null)
            {
                Parameters.Add(new URLParameter(name, value));
                return;
            }
            param.Value = value;
        }
    }
}