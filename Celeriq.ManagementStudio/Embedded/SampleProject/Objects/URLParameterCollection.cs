using System.Collections.Generic;
using System;
using System.Linq;

namespace CeleriqTestWebsite.Objects
{
    public class URLParameterCollection : List<URLParameter>
    {
        public string this[string name]
        {
            get
            {
                return (from item in this
                        where string.Compare(item.Name, name, true) == 0
                        select item.Value).FirstOrDefault();
            }
            set
            {
                if (string.IsNullOrEmpty(name)) return;
                var item = this.FirstOrDefault(x => string.Compare(x.Name, name, true) == 0);
                if (item == null)
                {
                    item = new URLParameter(name, value);
                    this.Add(item);
                }
                item.Value = value;
            }
        }

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

        public void SetValue(string name, bool value)
        {
            SetValue(name, value.ToString().ToLower());
        }

        public void SetValue(string name, int value)
        {
            SetValue(name, value.ToString());
        }

        public void SetValue(string name, Guid value)
        {
            SetValue(name, value.ToString());
        }

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
                    this[name] = value;
                else
                    this.Add(new URLParameter(name, value));
            }

        }

        public string GetValue(string name)
        {
            return this.GetValue(name, string.Empty);
        }

        public string GetValue(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                return defaultValue;

            if (this.Contains(name))
                return this[name];
            else
                return defaultValue;
        }

        public int GetValue(string name, int defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                return defaultValue;

            if (this.Contains(name))
                return System.Convert.ToInt32(this[name]);
            else
                return defaultValue;
        }

        public Guid GetValue(string name, Guid defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                return defaultValue;

            if (this.Contains(name))
            {
                Guid v;
                if (Guid.TryParse(this[name], out v))
                    return v;
                return defaultValue;
            }
            else
                return defaultValue;
        }

        public bool GetValue(string name, bool defaultValue)
        {
            if (string.IsNullOrEmpty(name))
                return defaultValue;

            if (this.Contains(name))
            {
                bool v;
                if (bool.TryParse(this[name], out v))
                    return v;
                return defaultValue;
            }
            else
                return defaultValue;
        }
    }
}