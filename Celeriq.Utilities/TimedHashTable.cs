using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Celeriq.Utilities
{
    /// <summary>
    /// This is a thread-safe timed hashtable class
    /// </summary>
    /// <typeparam name="K">The object type of the key</typeparam>
    /// <typeparam name="T">The object type of the value</typeparam>
    public class TimedHashTable<K, T> : HashTable<K, T>
    {
        private int _expiration = 60;
        private HashTable<K, DateTime> _cache = new HashTable<K, DateTime>();

        /// <summary />
        public TimedHashTable()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expiration">The number of seconds to hold values</param>
        public TimedHashTable(int expiration)
            : this()
        {
            if (expiration <= 0)
                throw new Exception("The 'Expiration' value must be greater than zero!");

            _expiration = expiration;
        }

        /// <summary />
        public override void Add(K key, T value)
        {
            lock (_cache)
            {
                _cache[key] = DateTime.Now;
                base[key] = value;
            }
        }

        /// <summary />
        public override void Clear()
        {
            lock (_cache)
            {
                base.Clear();
                _cache.Clear();
            }
        }

        /// <summary />
        public override void Remove(K key)
        {
            lock (_cache)
            {
                base.Remove(key);
                _cache.Remove(key);
            }
        }

        /// <summary />
        public override bool ContainsKey(K key)
        {
            lock (_cache)
            {
                var keys = this.Keys; //This will validate collection
                return base.ContainsKey(key);
            }
        }

        /// <summary />
        public override bool ContainsValue(T value)
        {
            lock (_cache)
            {
                var keys = this.Keys; //This will validate collection
                return base.ContainsValue(value);
            }
        }

        /// <summary />
        public override ICollection<K> Keys
        {
            get
            {
                lock (_cache)
                {
                    var retval = base.Keys;
                    foreach (var k in retval)
                    {
                        var q = GetCache(k); //This will remove the item if it has expired
                    }
                    return base.Keys;
                }
            }
        }

        /// <summary />
        public override T this[K key]
        {
            get { return this.GetCache(key); }
            set { base[key] = value; }
        }

        private T GetCache(K key)
        {
            lock (_cache)
            {
                //If the value is not null then check if expired and if so remove it from the hastable
                if (_cache.ContainsKey(key))
                {
                    var t = _cache[key];
                    if (DateTime.Now.Subtract(t).TotalSeconds >= _expiration)
                    {
                        base.Remove(key);
                        _cache.Remove(key);
                    }
                }
                return base[key];
            }
        }

    }
}