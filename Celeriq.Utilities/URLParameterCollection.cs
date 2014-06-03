using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Utilities
{
    /// <summary />
    public class URLParameterCollection : List<URLParameter>
    {
        /// <summary />
        public URLParameter this[string name]
        {
            get
            {
                foreach (var item in this)
                {
                    if (string.Compare(item.Name, name, true) == 0)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        /// <summary />
        public bool Contains(string name)
        {
            foreach (var item in this)
            {
                if (string.Compare(item.Name, name, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary />
        public void Remove(string name)
        {
            var index = 0;
            foreach (var item in this)
            {
                if (string.Compare(item.Name, name, true) == 0)
                {
                    this.RemoveAt(index);
                    return;
                }
                index++;
            }
        }

        /// <summary />
        public void SetValue(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (string.IsNullOrEmpty(value))
            {
                //If there is no value set then remove the parameter if it exists
                if (this.Contains(name))
                    this.Remove(name);
            }
            else
            {
                if (this.Contains(name))
                    this[name].Value = value;
                else
                    this.Add(new URLParameter(name, value));
            }

        }

        /// <summary />
        public void SetValue(string name, bool value)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (this.Contains(name))
                this[name].Value = value.ToString();
            else
                this.Add(new URLParameter(name, value.ToString()));
        }

        /// <summary />
        public void SetValue(string name, int value)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (this.Contains(name))
                this[name].Value = value.ToString();
            else
                this.Add(new URLParameter(name, value.ToString()));

        }

        /// <summary />
        public string GetValue(string name)
        {
            return this.GetValue(name, string.Empty);
        }

        /// <summary />
        public string GetValue(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                return defaultValue;

            if (this.Contains(name))
                return this[name].Value;
            else
                return defaultValue;
        }

        /// <summary />
        public bool GetValue(string name, bool defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                return defaultValue;

            if (this.Contains(name))
            {
                bool b;
                bool.TryParse(this[name].Value, out b);
                return b;
            }
            else
                return defaultValue;
        }

        /// <summary />
        public int GetValue(string name, int defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                return defaultValue;

            if (this.Contains(name))
            {
                int b;
                int.TryParse(this[name].Value, out b);
                return b;
            }
            else
                return defaultValue;
        }

    }
}