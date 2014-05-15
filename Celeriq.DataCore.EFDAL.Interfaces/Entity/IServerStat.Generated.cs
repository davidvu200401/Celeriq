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
	/// This is the interface for the entity ServerStat
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial interface IServerStat
	{
		#region Properties

		/// <summary>
		/// The property that maps back to the database 'ServerStat.AddedDate' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("AddedDate")]
		DateTime AddedDate { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.MemoryUsageAvailable' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("MemoryUsageAvailable")]
		long MemoryUsageAvailable { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.MemoryUsageProcess' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("MemoryUsageProcess")]
		long MemoryUsageProcess { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.MemoryUsageTotal' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("MemoryUsageTotal")]
		long MemoryUsageTotal { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.ProcessorUsage' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("ProcessorUsage")]
		int ProcessorUsage { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.RepositoryCreateDelta' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RepositoryCreateDelta")]
		long RepositoryCreateDelta { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.RepositoryDeleteDelta' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RepositoryDeleteDelta")]
		long RepositoryDeleteDelta { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.RepositoryInMem' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RepositoryInMem")]
		int RepositoryInMem { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.RepositoryLoadDelta' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RepositoryLoadDelta")]
		int RepositoryLoadDelta { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.RepositoryTotal' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RepositoryTotal")]
		int RepositoryTotal { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.RepositoryUnloadDelta' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DisplayName("RepositoryUnloadDelta")]
		int RepositoryUnloadDelta { get; set; }

		/// <summary>
		/// The property that maps back to the database 'ServerStat.ServerStatId' field
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DisplayName("ServerStatId")]
		int ServerStatId { get; set; }

		#endregion

		#region Navigation Properties

		#endregion

	}

}

#region Metadata Class

namespace Celeriq.DataCore.EFDAL.Interfaces.Entity.Metadata
{
	/// <summary>
	/// Metadata class for the 'ServerStat' entity
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial class ServerStatMetadata : Celeriq.DataCore.EFDAL.Interfaces.IMetadata
	{
		#region Properties

		/// <summary>
		/// Metadata information for the 'AddedDate' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'AddedDate' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "AddedDate", AutoGenerateField = true)]
		public object AddedDate;

		/// <summary>
		/// Metadata information for the 'MemoryUsageAvailable' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'MemoryUsageAvailable' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "MemoryUsageAvailable", AutoGenerateField = true)]
		public object MemoryUsageAvailable;

		/// <summary>
		/// Metadata information for the 'MemoryUsageProcess' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'MemoryUsageProcess' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "MemoryUsageProcess", AutoGenerateField = true)]
		public object MemoryUsageProcess;

		/// <summary>
		/// Metadata information for the 'MemoryUsageTotal' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'MemoryUsageTotal' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "MemoryUsageTotal", AutoGenerateField = true)]
		public object MemoryUsageTotal;

		/// <summary>
		/// Metadata information for the 'ProcessorUsage' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'ProcessorUsage' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "ProcessorUsage", AutoGenerateField = true)]
		public object ProcessorUsage;

		/// <summary>
		/// Metadata information for the 'RepositoryCreateDelta' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryCreateDelta' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryCreateDelta", AutoGenerateField = true)]
		public object RepositoryCreateDelta;

		/// <summary>
		/// Metadata information for the 'RepositoryDeleteDelta' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryDeleteDelta' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryDeleteDelta", AutoGenerateField = true)]
		public object RepositoryDeleteDelta;

		/// <summary>
		/// Metadata information for the 'RepositoryInMem' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryInMem' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryInMem", AutoGenerateField = true)]
		public object RepositoryInMem;

		/// <summary>
		/// Metadata information for the 'RepositoryLoadDelta' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryLoadDelta' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryLoadDelta", AutoGenerateField = true)]
		public object RepositoryLoadDelta;

		/// <summary>
		/// Metadata information for the 'RepositoryTotal' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryTotal' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryTotal", AutoGenerateField = true)]
		public object RepositoryTotal;

		/// <summary>
		/// Metadata information for the 'RepositoryUnloadDelta' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'RepositoryUnloadDelta' is required.")]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "RepositoryUnloadDelta", AutoGenerateField = true)]
		public object RepositoryUnloadDelta;

		/// <summary>
		/// Metadata information for the 'ServerStatId' parameter
		/// </summary>
		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = "'ServerStatId' is required.")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.DataAnnotations.Editable(false)]
		[System.ComponentModel.ReadOnly(true)]
		[System.ComponentModel.DataAnnotations.DisplayAttribute(Description = "", Name = "ServerStatId", AutoGenerateField = true)]
		public object ServerStatId;

		#endregion

	}

}

#endregion
