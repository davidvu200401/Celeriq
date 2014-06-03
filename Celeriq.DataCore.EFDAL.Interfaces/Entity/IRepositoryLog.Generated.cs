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
	/// This is the interface for the entity RepositoryLog
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial interface IRepositoryLog
	{
		#region Properties

		/// <summary>
		/// The property that maps back to the database 'RepositoryLog.Count' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Count")]
		int Count { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryLog.ElapsedTime' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("ElapsedTime")]
		int ElapsedTime { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryLog.IPAddress' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("IPAddress")]
		string IPAddress { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryLog.Query' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("Query")]
		string Query { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryLog.QueryId' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("QueryId")]
		System.Guid QueryId { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryLog.RepositoryId' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RepositoryId")]
		int RepositoryId { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryLog.RepositoryLogId' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("RepositoryLogId")]
		int RepositoryLogId { get; set; }

		/// <summary>
		/// The property that maps back to the database 'RepositoryLog.UsedCache' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("UsedCache")]
		bool UsedCache { get; set; }

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

		#endregion

		#region Navigation Properties

		/// <summary>
		/// The navigation definition for walking RepositoryLog->RepositoryDefinition
		/// </summary>
		Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryDefinition RepositoryDefinition { get; set; }

		#endregion

	}

}

#region Metadata Class

namespace Celeriq.DataCore.EFDAL.Interfaces.Entity.Metadata
{
	/// <summary>
	/// Metadata class for the 'RepositoryLog' entity
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial class RepositoryLogMetadata : Celeriq.DataCore.EFDAL.Interfaces.IMetadata
	{
		#region Properties

		/// <summary>
		/// Metadata information for the 'Count' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'Count' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Count", AutoGenerateField = true)]
		public object Count;

		/// <summary>
		/// Metadata information for the 'ElapsedTime' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'ElapsedTime' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "ElapsedTime", AutoGenerateField = true)]
		public object ElapsedTime;

		/// <summary>
		/// Metadata information for the 'IPAddress' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'IPAddress' is required.")]
		[System.ComponentModel.DataAnnotations.StringLength(50, ErrorMessage = "The property 'IPAddress' has a maximum length of 50")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "IPAddress", AutoGenerateField = true)]
		public object IPAddress;

		/// <summary>
		/// Metadata information for the 'Query' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.StringLength(2147483647, ErrorMessage = "The property 'Query' has a maximum length of 2147483647")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "Query", AutoGenerateField = true)]
		public object Query;

		/// <summary>
		/// Metadata information for the 'QueryId' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'QueryId' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "QueryId", AutoGenerateField = true)]
		public object QueryId;

		/// <summary>
		/// Metadata information for the 'RepositoryId' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryId' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryId", AutoGenerateField = true)]
		public object RepositoryId;

		/// <summary>
		/// Metadata information for the 'RepositoryLogId' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryLogId' is required.")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryLogId", AutoGenerateField = true)]
		public object RepositoryLogId;

		/// <summary>
		/// Metadata information for the 'UsedCache' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'UsedCache' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "UsedCache", AutoGenerateField = true)]
		public object UsedCache;

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

		#endregion

	}

}

#endregion

