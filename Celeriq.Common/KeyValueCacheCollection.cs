using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[Serializable]
	[DataContract()]
	public partial class KeyValueCacheCollection : List<KeyValueCache>
	{
		public void Add(string key, string value)
		{
			this.Add(new KeyValueCache()
			         	{
			         		Key = key,
			         		Value = value
			         	}
				);
		}

		public string this[string key]
		{
			get { var retval = this.FirstOrDefault(x => x.Key == key);
			if (retval == null) throw new Exception("The key was not found!");
				return retval.Value;
			}
		}

		public bool ContainsKey(string key)
		{
			return (this.Count(x => x.Key == key) > 0);
		}
	}

	[Serializable]
	[DataContract()]
	public partial class KeyValueCache
	{
		[DataMember]
		public virtual string Key { get; set; }
		[DataMember]
		public virtual string Value { get; set; }
	}
}
