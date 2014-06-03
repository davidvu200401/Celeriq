//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.Objects.DataClasses;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using Celeriq.DataCore.EFDAL.Interfaces;

namespace Celeriq.DataCore.EFDAL.Interfaces.Entity
{
	/// <summary>
	/// This is the interface for the entity RepositoryData
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial interface IRepositoryData
	{
		#region Properties

		/// <summary>
		/// The property that maps back to the database 'RepositoryData.Data' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Data")]
		System.Byte[] Data { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryData.Keyword' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Keyword")]
		string Keyword { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryData.RepositoryDataId' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("RepositoryDataId")]
		long RepositoryDataId { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryData.RepositoryId' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RepositoryId")]
		int RepositoryId { get; set; }

		/// <summary>
		/// The audit field for the 'Created By' column.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		string CreatedBy { get; }

		/// <summary>
		/// The audit field for the 'Created Date' column.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		DateTime? CreatedDate { get; }

		/// <summary>
		/// The audit field for the 'Modified By' column.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		string ModifiedBy { get; }

		/// <summary>
		/// The audit field for the 'Modified Date' column.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		DateTime? ModifiedDate { get; }

		#endregion

		#region Navigation Properties

		/// <summary>
		/// The navigation definition for walking RepositoryData->RepositoryDefinition
		/// </summary>
		Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryDefinition RepositoryDefinition { get; set; }

		#endregion

	}

}

#region Metadata Class

namespace Celeriq.DataCore.EFDAL.Interfaces.Entity.Metadata
{
	/// <summary>
	/// Metadata class for the 'RepositoryData' entity
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial class RepositoryDataMetadata : Celeriq.DataCore.EFDAL.Interfaces.IMetadata
	{
		#region Properties

		/// <summary>
		/// Metadata information for the 'Data' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'Data' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Data", AutoGenerateField = true)]
		public object Data;

		/// <summary>
		/// Metadata information for the 'Keyword' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(2147483647, ErrorMessage = "The property 'Keyword' has a maximum length of 2147483647")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Keyword", AutoGenerateField = true)]
		public object Keyword;

		/// <summary>
		/// Metadata information for the 'RepositoryDataId' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryDataId' is required.")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryDataId", AutoGenerateField = true)]
		public object RepositoryDataId;

		/// <summary>
		/// Metadata information for the 'RepositoryId' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryId' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryId", AutoGenerateField = true)]
		public object RepositoryId;

		/// <summary>
		/// Metadata information for the 'CreatedBy' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "The property 'CreatedBy' has a maximum length of 100")]
		[System.ComponentModel.ReadOnly(true)]
		public object CreatedBy;

		/// <summary>
		/// Metadata information for the 'CreatedDate' parameter
		/// </summary>
		[System.ComponentModel.ReadOnly(true)]
		public object CreatedDate;

		/// <summary>
		/// Metadata information for the 'ModifiedBy' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = "The property 'ModifiedBy' has a maximum length of 100")]
		[System.ComponentModel.ReadOnly(true)]
		public object ModifiedBy;

		/// <summary>
		/// Metadata information for the 'ModifiedDate' parameter
		/// </summary>
		[System.ComponentModel.ReadOnly(true)]
		public object ModifiedDate;

		/// <summary>
		/// Metadata information for the 'Timestamp' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Timestamp()]
		[System.ComponentModel.ReadOnly(true)]
		public object Timestamp;

		#endregion

	}

}

#endregion

