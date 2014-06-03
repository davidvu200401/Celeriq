using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Common
{
	public enum ComparisonConstants
	{
		LessThan,
		GreaterThan,
		LessThanOrEq,
		GreaterThanOrEq,
		Equals,
		Like,
		Between,
		NotEqual,
	}

	public interface IFieldFilter
	{
		string Name { get; set; }
		Celeriq.Common.ComparisonConstants Comparer { get; set; }
		object Value { get; set; }
		object Value2 { get; set; }
	}
}
