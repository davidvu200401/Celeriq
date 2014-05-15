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
using System.Data.Objects;
using System.Text;
using Celeriq.DataCore.EFDAL;
using nHydrate.EFCore.DataAccess;
using nHydrate.EFCore.EventArgs;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Data.Linq;

namespace Celeriq.DataCore.EFDAL.Entity
{
	/// <summary>
	/// The collection to hold 'RepositoryActionType' entities
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("nHydrateModelGenerator", "5.2.0")]
	[EdmEntityTypeAttribute(NamespaceName = "Celeriq.DataCore.EFDAL.Entity", Name = "RepositoryActionType")]
	[Serializable]
	[DataContractAttribute(IsReference = true)]
	[nHydrate.EFCore.Attributes.FieldNameConstantsAttribute(typeof(Celeriq.DataCore.EFDAL.Entity.RepositoryActionType.FieldNameConstants))]
	[System.ComponentModel.DataAnnotations.MetadataType(typeof(Celeriq.DataCore.EFDAL.Interfaces.Entity.Metadata.RepositoryActionTypeMetadata))]
	[System.ComponentModel.ImmutableObject(true)]
	[nHydrate.EFCore.Attributes.EntityMetadata("RepositoryActionType", false, false, false, false, "", true, true, true, "dbo")]
	public partial class RepositoryActionType : nHydrate.EFCore.DataAccess.NHEntityObject, nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, Celeriq.DataCore.EFDAL.IEntityWithContext, System.ComponentModel.IDataErrorInfo, Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType, System.ICloneable, System.IEquatable<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType>
	{
		#region FieldNameConstants Enumeration

		/// <summary>
		/// Enumeration to define each property that maps to a database field for the 'RepositoryActionType' table.
		/// </summary>
		public enum FieldNameConstants
		{
			/// <summary>
			/// Field mapping for the 'Name' property
			/// </summary>
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'Name' property")]
			Name,
			/// <summary>
			/// Field mapping for the 'RepositoryActionTypeId' property
			/// </summary>
			[nHydrate.EFCore.Attributes.PrimaryKeyAttribute()]
			[System.ComponentModel.ReadOnlyAttribute(true)]
			[System.ComponentModel.Description("Field mapping for the 'RepositoryActionTypeId' property")]
			RepositoryActionTypeId,
		}
		#endregion

		#region Constructors

		/// <summary>
		/// Method called when an instance of this class is created
		/// </summary>
		partial void OnCreated();

		/// <summary>
		/// Initializes a new instance of the Celeriq.DataCore.EFDAL.Entity.RepositoryActionType class
		/// </summary>
		protected internal RepositoryActionType()
		{
			this.OnCreated();
		}

		#endregion

		#region Properties

		/// <summary>
		/// The property that maps back to the database 'RepositoryActionType.Name' field.
		/// </summary>
		/// <remarks>Field: [RepositoryActionType].[Name], Field Length: 50, Not Nullable</remarks>
		[System.ComponentModel.Browsable(true)]
		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
		[DataMemberAttribute()]
		[System.ComponentModel.DisplayName("Name")]
		[System.ComponentModel.ReadOnly(true)]
		[System.Diagnostics.DebuggerNonUserCode]
		public virtual string Name
		{
			get { return _name; }
			protected set
			{
				if ((value != null) && (value.Length > GetMaxLength(FieldNameConstants.Name))) throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, "RepositoryActionType.Name", GetMaxLength(FieldNameConstants.Name)));
				if (value == _name) return;
				_name = value;
			}
		}

		/// <summary>
		/// This property is a wrapper for the typed enumeration for the 'RepositoryActionTypeId' field.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode]
		public virtual Celeriq.DataCore.EFDAL.Interfaces.RepositoryActionTypeConstants RepositoryActionTypeValue
		{
			get { return (Celeriq.DataCore.EFDAL.Interfaces.RepositoryActionTypeConstants)this.RepositoryActionTypeId; }
			set { this.RepositoryActionTypeId = (int)value; }
		}

		/// <summary>
		/// The property that maps back to the database 'RepositoryActionType.RepositoryActionTypeId' field.
		/// This property has an additional enumeration wrapper property RepositoryActionTypeValue. Use it as a strongly-typed property.
		/// </summary>
		/// <remarks>Field: [RepositoryActionType].[RepositoryActionTypeId], Not Nullable, Primary Key, Unique, Indexed</remarks>
		[System.ComponentModel.Browsable(true)]
		[EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
		[DataMemberAttribute()]
		[System.ComponentModel.DisplayName("RepositoryActionTypeId")]
		[System.ComponentModel.DataAnnotations.Key()]
		[System.ComponentModel.ReadOnly(true)]
		[System.Diagnostics.DebuggerNonUserCode]
		public virtual int RepositoryActionTypeId
		{
			get { return _repositoryActionTypeId; }
			protected set
			{
				//Error check the wrapped enumeration
				switch(value)
				{
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
						break;
					default: throw new Exception(string.Format(GlobalValues.ERROR_INVALID_ENUM, value.ToString(), "RepositoryActionType.RepositoryActionTypeId"));
				}

				if (value == _repositoryActionTypeId) return;
				_repositoryActionTypeId = value;
			}
		}

		#endregion

		#region Property Holders

		/// <summary />
		protected string _name;
		/// <summary />
		protected int _repositoryActionTypeId;

		#endregion

		#region GetMaxLength

		/// <summary>
		/// Gets the maximum size of the field value.
		/// </summary>
		public static int GetMaxLength(FieldNameConstants field)
		{
			switch (field)
			{
				case FieldNameConstants.Name:
					return 50;
				case FieldNameConstants.RepositoryActionTypeId:
					return 0;
			}
			return 0;
		}

		int nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetMaxLength(Enum field)
		{
			return GetMaxLength((FieldNameConstants)field);
		}

		#endregion

		#region GetFieldNameConstants

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldNameConstants()
		{
			return typeof(FieldNameConstants);
		}

		#endregion

		#region GetFieldType

		/// <summary>
		/// Gets the system type of a field on this object
		/// </summary>
		public static System.Type GetFieldType(FieldNameConstants field)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Celeriq.DataCore.EFDAL.Entity.RepositoryActionType.FieldNameConstants'.");

			switch ((FieldNameConstants)field)
			{
				case FieldNameConstants.Name: return typeof(string);
				case FieldNameConstants.RepositoryActionTypeId: return typeof(int);
			}
			return null;
		}

		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type 'Celeriq.DataCore.EFDAL.Entity.RepositoryActionType.FieldNameConstants'.");

			return GetFieldType((Celeriq.DataCore.EFDAL.Entity.RepositoryActionType.FieldNameConstants)field);
		}

		#endregion

		#region Get/Set Value

		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(System.Enum field)
		{
			return ((nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject)this).GetValue(field, null);
		}

		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(System.Enum field, object defaultValue)
		{
			if (field.GetType() != typeof(FieldNameConstants))
				throw new Exception("The '" + field.GetType().ReflectedType.ToString() + ".FieldNameConstants' value is not valid. The field parameter must be of type '" + this.GetType().ToString() + ".FieldNameConstants'.");
			return this.GetValue((FieldNameConstants)field, defaultValue);
		}

		#endregion

		#region PrimaryKey

		/// <summary>
		/// Hold the primary key for this object
		/// </summary>
		protected nHydrate.EFCore.DataAccess.IPrimaryKey _primaryKey = null;
		nHydrate.EFCore.DataAccess.IPrimaryKey nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.PrimaryKey
		{
			get { return null; }
		}

		#endregion

		#region IsParented

		/// <summary>
		/// Determines if this object is part of a collection or is detached
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual bool IsParented
		{
			get { return (this.EntityState != System.Data.EntityState.Detached); }
		}

		#endregion

		#region Clone

		/// <summary>
		/// Creates a shallow copy of this object
		/// </summary>
		public virtual object Clone()
		{
			return Celeriq.DataCore.EFDAL.Entity.RepositoryActionType.Clone(this);
		}

		/// <summary>
		/// Creates a shallow copy of this object with defined, default values and new PK
		/// </summary>
		public virtual object CloneAsNew()
		{
			var item = Celeriq.DataCore.EFDAL.Entity.RepositoryActionType.Clone(this);
			item._repositoryActionTypeId = 0;
			return item;
		}

		/// <summary>
		/// Creates a shallow copy of this object
		/// </summary>
		public static RepositoryActionType Clone(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType item)
		{
			var newItem = new RepositoryActionType();
			newItem.Name = item.Name;
			newItem.RepositoryActionTypeId = item.RepositoryActionTypeId;
			return newItem;
		}

		#endregion

		#region IsEquivalent

		/// <summary>
		/// Determines if all of the fields for the specified object exactly matches the current object.
		/// </summary>
		/// <param name="item">The object to compare</param>
		public override bool IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject item)
		{
			return ((System.IEquatable<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType>)this).Equals(item as Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType);
		}

		#endregion

		#region GetValue

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public object GetValue(FieldNameConstants field)
		{
			return GetValue(field, null);
		}

		/// <summary>
		/// Gets the value of one of this object's properties.
		/// </summary>
		public object GetValue(FieldNameConstants field, object defaultValue)
		{
			if (field == FieldNameConstants.Name)
				return this.Name;
			if (field == FieldNameConstants.RepositoryActionTypeId)
				return this.RepositoryActionTypeId;
			throw new Exception("Field '" + field.ToString() + "' not found!");
		}

		#endregion

		#region Navigation Properties

		/// <summary>
		/// The back navigation definition for walking [RepositoryActionType]->[RepositoryStat]
		/// Relationship Links: 
		/// [RepositoryActionType.RepositoryActionTypeId = RepositoryStat.RepositoryActionTypeId] (Required)
		/// </summary>
		[XmlIgnoreAttribute()]
		[SoapIgnoreAttribute()]
		[EdmRelationshipNavigationPropertyAttribute("Celeriq.DataCore.EFDAL.Entity", "FK__RepositoryStat_RepositoryActionType", "RepositoryStatList")]
		public virtual EntityCollection<Celeriq.DataCore.EFDAL.Entity.RepositoryStat> RepositoryStatList
		{
			get
			{
				//Eager load
				var retval = ((System.Data.Objects.DataClasses.IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<Celeriq.DataCore.EFDAL.Entity.RepositoryStat>("Celeriq.DataCore.EFDAL.Entity.FK__RepositoryStat_RepositoryActionType", "RepositoryStatList");
				if (!retval.IsLoaded  && this.EntityState != System.Data.EntityState.Added && this.EntityState != System.Data.EntityState.Detached)
				{
					retval.Load();
				}
				return retval;
			}
		}

		#region IRepositoryActionType Interface

		System.Collections.Generic.ICollection<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryStat> Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType.RepositoryStatList
		{
			get { return (System.Collections.Generic.ICollection<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryStat>)(System.Collections.Generic.ICollection<RepositoryStat>)this.RepositoryStatList; }
		}

		#endregion

		#endregion

		#region Static SQL Methods

		internal static string GetFieldAliasFromFieldNameSqlMapping(string alias)
		{
			alias = alias.Replace("[", string.Empty).Replace("]", string.Empty);
			switch (alias.ToLower())
			{
				case "name": return "name";
				case "repositoryactiontypeid": return "repositoryactiontypeid";
				default: throw new Exception("The select clause is not valid.");
			}
		}

		internal static string GetTableFromFieldAliasSqlMapping(string alias)
		{
			switch (alias.ToLower())
			{
				case "name": return "RepositoryActionType";
				case "repositoryactiontypeid": return "RepositoryActionType";
				default: throw new Exception("The select clause is not valid.");
			}
		}

		internal static string GetTableFromFieldNameSqlMapping(string field)
		{
			switch (field.ToLower())
			{
				case "name": return "RepositoryActionType";
				case "repositoryactiontypeid": return "RepositoryActionType";
				default: throw new Exception("The select clause is not valid.");
			}
		}

		internal static string GetRemappedLinqSql(string sql, string parentAlias, LinqSQLFromClauseCollection childTables)
		{
			sql = System.Text.RegularExpressions.Regex.Replace(sql, "\\[" + parentAlias + "\\]\\.\\[name\\]", "[" + childTables.GetBaseAliasTable(parentAlias, "RepositoryActionType") + "].[name]", RegexOptions.IgnoreCase);
			sql = System.Text.RegularExpressions.Regex.Replace(sql, "\\[" + parentAlias + "\\]\\.\\[repositoryactiontypeid\\]", "[" + childTables.GetBaseAliasTable(parentAlias, "RepositoryActionType") + "].[repositoryactiontypeid]", RegexOptions.IgnoreCase);
			return sql;
		}

		#endregion

		#region GetDatabaseFieldName

		/// <summary>
		/// Returns the actual database name of the specified field.
		/// </summary>
		internal static string GetDatabaseFieldName(RepositoryActionType.FieldNameConstants field)
		{
			return GetDatabaseFieldName(field.ToString());
		}

		/// <summary>
		/// Returns the actual database name of the specified field.
		/// </summary>
		internal static string GetDatabaseFieldName(string field)
		{
			switch (field)
			{
				case "Name": return "Name";
				case "RepositoryActionTypeId": return "RepositoryActionTypeId";
			}
			return string.Empty;
		}

		#endregion

		#region Context

		DataCoreEntities IEntityWithContext.Context
		{
			get { return _internalContext; }
			set { _internalContext = value; }
		}
		private DataCoreEntities _internalContext = null;

		#endregion

		#region Static Methods

		/// <summary>
		/// Creates and returns a metadata object for an entity type
		/// </summary>
		/// <returns>A metadata object for the entity types in this assembly</returns>
		public static Celeriq.DataCore.EFDAL.Interfaces.Entity.Metadata.RepositoryActionTypeMetadata GetMetadata()
		{
			var a = typeof(RepositoryActionType).GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MetadataTypeAttribute), true).FirstOrDefault();
			if (a == null) return null;
			var t = ((System.ComponentModel.DataAnnotations.MetadataTypeAttribute)a).MetadataClassType;
			if (t == null) return null;
			return Activator.CreateInstance(t) as Celeriq.DataCore.EFDAL.Interfaces.IMetadata as Celeriq.DataCore.EFDAL.Interfaces.Entity.Metadata.RepositoryActionTypeMetadata;
		}

		#endregion

		#region Equals
		/// <summary>
		/// Compares two objects of 'RepositoryActionType' type and determines if all properties match
		/// </summary>
		/// <returns>True if all properties match, false otherwise</returns>
		bool System.IEquatable<Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType>.Equals(Celeriq.DataCore.EFDAL.Interfaces.Entity.IRepositoryActionType other)
		{
			return this.Equals(other);
		}

		/// <summary>
		/// Compares two objects of 'RepositoryActionType' type and determines if all properties match
		/// </summary>
		/// <returns>True if all properties match, false otherwise</returns>
		public override bool Equals(object obj)
		{
			var other = obj as Celeriq.DataCore.EFDAL.Entity.RepositoryActionType;
			if (other == null) return false;
			return (
				other.Name == this.Name &&
				other.RepositoryActionTypeId == this.RepositoryActionTypeId
				);
		}

		/// <summary>
		/// Serves as a hash function for this type.
		/// </summary>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		#region IDataErrorInfo
		/// <summary />
		string System.ComponentModel.IDataErrorInfo.Error
		{
			get { return this.GetObjectDataErrorInfo(); }
		}

		/// <summary />
		/// <param name="columnName"></param>
		/// <returns></returns>
		string System.ComponentModel.IDataErrorInfo.this[string columnName]
		{
			get
			{
				if (string.IsNullOrEmpty(columnName))
					return string.Empty;

				var retval = GetObjectPropertyDataErrorInfo(columnName);
				if (string.IsNullOrEmpty(retval))
				{
					switch (columnName.ToLower())
					{
						case "name":
							if (string.IsNullOrEmpty(this.Name) || string.IsNullOrEmpty(this.Name.Trim()))
								retval = "Name is required!";
							break;
						default:
							break;
					}

				}
				return retval;
			}
		}
		#endregion

	}

	partial class RepositoryActionType
	{
	}

}
