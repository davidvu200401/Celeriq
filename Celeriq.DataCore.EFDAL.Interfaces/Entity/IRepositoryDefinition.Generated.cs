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
	/// This is the interface for the entity RepositoryDefinition
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial interface IRepositoryDefinition
	{
		#region Properties

		/// <summary>
		/// The property that maps back to the database 'RepositoryDefinition.DefinitionData' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("DefinitionData")]
		System.Byte[] DefinitionData { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryDefinition.ItemCount' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("ItemCount")]
		int ItemCount { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryDefinition.MemorySize' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("MemorySize")]
		long MemorySize { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryDefinition.Name' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Name")]
		string Name { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryDefinition.RepositoryId' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("RepositoryId")]
		int RepositoryId { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryDefinition.UniqueKey' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("UniqueKey")]
		System.Guid UniqueKey { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryDefinition.VersionHash' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("VersionHash")]
		long VersionHash { get; set; }

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
		/// The back navigation definition for walking RepositoryDefinition->RepositoryData
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Celeriq.DataCore.EFDAL.Interfaces.Entity", "FK__RepositoryData_RepositoryDefinition", "RepositoryDataList")]
		ICollection<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryData> RepositoryDataList { get; }

		/// <summary>
		/// The back navigation definition for walking RepositoryDefinition->DimensionStore
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Celeriq.DataCore.EFDAL.Interfaces.Entity", "FK__DimensionStore_RepositoryDefinition", "DimensionStoreList")]
		ICollection<Celeriq.DataCore.EFDAL.Interfaces.Entity.IDimensionStore> DimensionStoreList { get; }

		/// <summary>
		/// The back navigation definition for walking RepositoryDefinition->RepositoryLog
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Celeriq.DataCore.EFDAL.Interfaces.Entity", "FK__RepositoryLog_RepositoryDefinition", "RepositoryLogList")]
		ICollection<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryLog> RepositoryLogList { get; }

		/// <summary>
		/// The back navigation definition for walking RepositoryDefinition->RepositoryStat
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[DataMemberAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Celeriq.DataCore.EFDAL.Interfaces.Entity", "FK__RepositoryStat_RepositoryDefinition", "RepositoryStatList")]
		ICollection<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryStat> RepositoryStatList { get; }

		#endregion

	}

}

#region Metadata Class

namespace Celeriq.DataCore.EFDAL.Interfaces.Entity.Metadata
{
	/// <summary>
	/// Metadata class for the 'RepositoryDefinition' entity
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial class RepositoryDefinitionMetadata : Celeriq.DataCore.EFDAL.Interfaces.IMetadata
	{
		#region Properties

		/// <summary>
		/// Metadata information for the 'DefinitionData' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'DefinitionData' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "DefinitionData", AutoGenerateField = true)]
		public object DefinitionData;

		/// <summary>
		/// Metadata information for the 'ItemCount' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'ItemCount' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "ItemCount", AutoGenerateField = true)]
		public object ItemCount;

		/// <summary>
		/// Metadata information for the 'MemorySize' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'MemorySize' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "MemorySize", AutoGenerateField = true)]
		public object MemorySize;

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
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryId", AutoGenerateField = true)]
		public object RepositoryId;

		/// <summary>
		/// Metadata information for the 'UniqueKey' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'UniqueKey' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "UniqueKey", AutoGenerateField = true)]
		public object UniqueKey;

		/// <summary>
		/// Metadata information for the 'VersionHash' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'VersionHash' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "VersionHash", AutoGenerateField = true)]
		public object VersionHash;

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

