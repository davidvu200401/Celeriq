using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celeriq.Common
{
	public interface ICloneable<T>
	{
		T Clone();

		T Clone(T dest);
	}
}
