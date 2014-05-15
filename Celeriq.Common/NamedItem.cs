using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	[Serializable]
	public class NamedItem
	{
		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public string Value { get; set; }
	}

	[Serializable]
	public class NamedItemList : List<NamedItem>
	{
		public void Add(string key, string value)
		{
			this.Add(new NamedItem() { Key = key, Value = value });
		}

		public string this[string key]
		{
			get
			{
				var item = this.FirstOrDefault(x => x.Key == key);
				if (item == null) return null;
				else return item.Value;
			}
			set
			{
				var item = this.FirstOrDefault(x => x.Key == key);
				if (item != null) item.Value = value;
				else this.Add(key, value);
			}
		}
	}
}
