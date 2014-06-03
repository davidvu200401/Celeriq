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
	/// This is the interface for the entity DimensionStore
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial interface IDimensionStore
	{
		#region Properties

		/// <summary>
		/// The property that maps back to the database 'DimensionStore.DIdx' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("DIdx")]
		long DIdx { get; set; }

		/// <summary>
		/// The property that maps back to the database 'DimensionStore.DimensionStoreId' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("DimensionStoreId")]
		long DimensionStoreId { get; set; }

		/// <summary>
		/// The property that maps back to the database 'DimensionStore.Name' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Name")]
		string Name { get; set; }

		/// <summary>
		/// The property that maps back to the database 'DimensionStore.RepositoryId' field
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
		/// The back navigation definition for walking DimensionStore->DimensionData
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Celeriq.DataCore.EFDAL.Interfaces.Entity", "FK__DimensionData_DimensionStore", "DimensionDataList")]
		ICollection<Celeriq.DataCore.EFDAL.Interfaces.Entity.IDimensionData> DimensionDataList { get; }

		/// <summary>
		/// The navigation definition for walking DimensionStore->RepositoryDefinition
		/// </summary>
		Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryDefinition RepositoryDefinition { get; set; }

		#endregion

	}

}

#region Metadata Class

namespace Celeriq.DataCore.EFDAL.Interfaces.Entity.Metadata
{
	/// <summary>
	/// Metadata class for the 'DimensionStore' entity
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial class DimensionStoreMetadata : Celeriq.DataCore.EFDAL.Interfaces.IMetadata
	{
		#region Properties

		/// <summary>
		/// Metadata information for the 'DIdx' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'DIdx' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "DIdx", AutoGenerateField = true)]
		public object DIdx;

		/// <summary>
		/// Metadata information for the 'DimensionStoreId' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'DimensionStoreId' is required.")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "DimensionStoreId", AutoGenerateField = true)]
		public object DimensionStoreId;

		/// <summary>
		/// Metadata information for the 'Name' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'Name' is required.")]
		[System.ComponentModel.DataAnnotations.StringLength(50, ErrorMessage = "The property 'Name' has a maximum length of 50")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Name", AutoGenerateField = true)]
		public object Name;

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

