using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
	public interface IFieldDefinition
	{
		[DataMember]
		string Name { get; set; }
		[DataMember]
		RepositorySchema.FieldTypeConstants FieldType { get; set; }
		[DataMember]
		int Length { get; set; }
		[DataMember]
		RepositorySchema.DataTypeConstants DataType { get; set; }
		[DataMember]
		bool IsPrimaryKey { get; set; }
		[DataMember]
		bool AllowTextSearch { get; set; }

		bool SortingSupported { get; }
		bool FilteringSupported { get; }
	}
}
