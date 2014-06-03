using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Celeriq.Utilities
{
    /// <summary>
    /// Provides a mechanism to determines the order in which items were add to this list
    /// </summary>
    public class SequencedHashTable<K, T> : HashTable<K, T>
    {
        private static List<K> _keyList = null;

        /// <summary />
        public SequencedHashTable()
        {
            _keyList = new List<K>();
        }

        /// <summary />
        public override void Add(K key, T value)
        {
            lock (_keyList)
            {
                //If key already exists then remove it so it can be re-added so it will move up in rank
                if (_keyList.Contains(key))
                    _keyList.Remove(key);

                _keyList.Add(key);
                base[key] = value;
            }
        }

        /// <summary />
        public IEnumerable<K> OrderedKeys
        {
            get
            {
                lock (_keyList)
                {
                    return _keyList.AsEnumerable().ToList();
                }
            }
        }

        /// <summary />
        public override void Clear()
        {
            lock (_keyList)
            {
                _keyList.Clear();
                base.Clear();
            }
        }

        /// <summary />
        public override void Remove(K key)
        {
            lock (_keyList)
            {
                if (_keyList.Contains(key))
                    _keyList.Remove(key);
                base.Remove(key);
            }
        }

    }
}