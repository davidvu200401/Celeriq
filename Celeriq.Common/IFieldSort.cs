using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Common
{
	public enum SortDirectionConstants
	{
		Asc,
		Desc,
	}

	public interface IFieldSort
	{
		Celeriq.Common.SortDirectionConstants SortDirection { get; set; }
		string Name { get; set; }
	}
}
