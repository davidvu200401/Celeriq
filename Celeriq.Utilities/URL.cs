using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Utilities
{
    /// <summary>
    /// A class that parses a URL structure
    /// </summary>
    public class URL : ICloneable
    {
        #region Class Members

        /// <summary />
        protected string _page = "";
        /// <summary />
        protected URLParameterCollection _parameters = new URLParameterCollection();

        #endregion

        #region Constructors

        /// <summary />
        public URL()
        {
        }

        /// <summary />
        public URL(string url)
            : this()
        {
            if (url.Contains("%")) url = System.Web.HttpUtility.UrlDecode(url);
            var arr = url.Split('?');
            _page = arr[0];

            if (arr.Length >= 2)
            {
                //NameValueCollection list = System.Web.HttpUtility.ParseQueryString(arr[1], System.Text.Encoding.ASCII);
                //foreach (string key in list.AllKeys)
                //{
                //  this.Parameters.Add(new URLParameter(key, list[key]));
                //}

                var groups = arr[1].Split('&');
                foreach (var g in groups)
                {
                    var values = g.Split('=');
                    if (values.Length == 2)
                        this.Parameters.Add(new URLParameter(values[0], values[1]));
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
        public virtual URLParameterCollection Parameters
        {
            get { return _parameters; }
        }

        #endregion

        #region Methods

        /// <summary />
        public string GetURL()
        {
            var retval = string.Empty;
            return retval;
        }

        /// <summary>
        /// The string representation of the URL
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var retval = this.Page;
            if (this.Parameters.Count != 0)
            {
                retval += "?";
                foreach (var item in this.Parameters)
                {
                    retval += item.Name + "=" + item.Value + "&";
                }
            }

            if (retval.EndsWith("&"))
                retval = retval.Substring(0, retval.Length - 1);

            return retval;
        }

        #endregion

        #region ICloneable Members

        /// <summary />
        public virtual object Clone()
        {
            var retval = new URL();
            retval.Page = this.Page;
            retval.Parameters.AddRange(this.Parameters);
            return retval;
        }

        #endregion
    }
}