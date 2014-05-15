//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 0168
#pragma warning disable 0108
using System;
using System.Linq;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Celeriq.DataCore.EFDAL.Interfaces.Entity;

namespace Celeriq.DataCore.EFDAL.Interfaces
{
	#region StaticDataConstants Enumeration for 'RepositoryActionType' entity
	/// <summary>
	/// Enumeration to define static data items and their ids 'RepositoryActionType' table.
	/// </summary>
	public enum RepositoryActionTypeConstants
	{
		/// <summary>
		/// Enumeration for the 'Query' item
		/// </summary>
		[Description("")]
		Query = 1,
		/// <summary>
		/// Enumeration for the 'LoadData' item
		/// </summary>
		[Description("")]
		LoadData = 2,
		/// <summary>
		/// Enumeration for the 'SaveData' item
		/// </summary>
		[Description("")]
		SaveData = 3,
		/// <summary>
		/// Enumeration for the 'UnloadData' item
		/// </summary>
		[Description("")]
		UnloadData = 4,
		/// <summary>
		/// Enumeration for the 'Reset' item
		/// </summary>
		[Description("")]
		Reset = 5,
		/// <summary>
		/// Enumeration for the 'ExportSchema' item
		/// </summary>
		[Description("")]
		ExportSchema = 6,
		/// <summary>
		/// Enumeration for the 'Backup' item
		/// </summary>
		[Description("")]
		Backup = 7,
		/// <summary>
		/// Enumeration for the 'Restore' item
		/// </summary>
		[Description("")]
		Restore = 8,
		/// <summary>
		/// Enumeration for the 'Compress' item
		/// </summary>
		[Description("")]
		Compress = 9,
		/// <summary>
		/// Enumeration for the 'Shutdown' item
		/// </summary>
		[Description("")]
		Shutdown = 10,
		/// <summary>
		/// Enumeration for the 'DeleteData' item
		/// </summary>
		[Description("")]
		DeleteData = 11,
	}
	#endregion

	#region Entity Context

	/// <summary>
	/// There are no comments for DataCoreEntities in the schema.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	public partial interface IDataCoreEntities : System.IDisposable
	{
		/// <summary>
		/// There are no comments for ConfigurationSetting in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IConfigurationSetting> ConfigurationSetting { get ; }

		/// <summary>
		/// There are no comments for DimensionData in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IDimensionData> DimensionData { get ; }

		/// <summary>
		/// There are no comments for DimensionStore in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IDimensionStore> DimensionStore { get ; }

		/// <summary>
		/// There are no comments for RepositoryActionType in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType> RepositoryActionType { get ; }

		/// <summary>
		/// There are no comments for RepositoryData in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryData> RepositoryData { get ; }

		/// <summary>
		/// There are no comments for RepositoryDefinition in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryDefinition> RepositoryDefinition { get ; }

		/// <summary>
		/// There are no comments for RepositoryLog in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryLog> RepositoryLog { get ; }

		/// <summary>
		/// There are no comments for RepositoryStat in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryStat> RepositoryStat { get ; }

		/// <summary>
		/// There are no comments for ServerStat in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IServerStat> ServerStat { get ; }

		/// <summary>
		/// There are no comments for UserAccount in the schema.
		/// </summary>
		IObjectSet<Celeriq.DataCore.EFDAL.Interfaces.Entity.IUserAccount> UserAccount { get ; }

		#region AddItem Method

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IConfigurationSetting entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IDimensionData entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IDimensionStore entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryData entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryDefinition entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryLog entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryStat entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IServerStat entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void AddItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IUserAccount entity);

		#endregion

		#region DeleteItem Methods

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IConfigurationSetting entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IDimensionData entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IDimensionStore entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryData entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryDefinition entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryLog entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryStat entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IServerStat entity);

		/// <summary>
		/// Adds an object to the object context.
		/// </summary>
		void DeleteItem(Celeriq.DataCore.EFDAL.Interfaces.Entity.IUserAccount entity);

		#endregion

	}

	#endregion

	/// <summary />
	public partial interface IMetadata
	{
	}

	/// <summary />
	public partial interface IDTO
	{
	}

}

#pragma warning restore 0168
#pragma warning restore 0108
