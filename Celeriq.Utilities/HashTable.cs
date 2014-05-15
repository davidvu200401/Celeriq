using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Celeriq.Utilities
{
    /// <summary>
    /// This is a thread-safe hashtable class
    /// </summary>
    /// <typeparam name="K">The object type of the key</typeparam>
    /// <typeparam name="T">The object type of the value</typeparam>
    public class HashTable<K, T>
    {
        /// <summary />
        protected Hashtable _h = new Hashtable(1000);

        /// <summary />
        public HashTable()
        {
        }

        /// <summary />
        public virtual void Add(K key, T value)
        {
            lock (_h)
            {
                _h[key] = value;
            }
        }

        /// <summary />
        public virtual void Clear()
        {
            lock (_h)
            {
                _h.Clear();
            }
        }

        /// <summary />
        public virtual T this[K key]
        {
            get
            {
                lock (_h)
                {
                    return (T) _h[key];
                }
            }
            set
            {
                lock (_h)
                {
                    _h[key] = value;
                }
            }
        }

        /// <summary />
        public virtual bool ContainsKey(K key)
        {
            lock (_h)
            {
                return _h.ContainsKey(key);
            }
        }

        /// <summary />
        public int Count
        {
            get
            {
                lock (_h)
                {
                    return _h.Keys.Count;
                }
            }
        }

        /// <summary />
        public virtual bool ContainsValue(T value)
        {
            lock (_h)
            {
                return _h.ContainsValue(value);
            }
        }

        /// <summary />
        public virtual ICollection<K> Keys
        {
            get
            {
                lock (_h)
                {
                    return new List<K>(_h.Keys.Cast<K>());
                }
            }
        }

        /// <summary />
        public virtual void Remove(K key)
        {
            lock (_h)
            {
                _h.Remove(key);
            }
        }

        private ICollection<T> Values
        {
            get
            {
                lock (_h)
                {
                    return new List<T>(_h.Values.Cast<T>());
                }
            }
        }

    }
}