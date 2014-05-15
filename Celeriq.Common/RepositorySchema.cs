#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;

namespace Celeriq.Common
{
    [Serializable()]
    [DataContract]
    [KnownType(typeof (DimensionDefinition))]
    [KnownType(typeof (FieldDefinition))]
    [KnownType(typeof (GeoCode))]
    [KnownType(typeof (GeoCodeFieldFilter))]
    public class RepositorySchema
    {
        [Serializable()]
        public enum DataTypeConstants
        {
            [EnumMember]
            String,

            [EnumMember]
            Int,

            [EnumMember]
            DateTime,

            [EnumMember]
            Float,

            [EnumMember]
            Bool,

            [EnumMember]
            GeoCode,

            [EnumMember]
            List,
        }

        [Serializable()]
        public enum DimensionTypeConstants
        {
            [EnumMember]
            Normal,

            [EnumMember]
            Range,

            [EnumMember]
            List,
        }

        [Serializable()]
        public enum FieldTypeConstants
        {
            [EnumMember]
            Field,

            [EnumMember]
            Dimension,
        }

        [Serializable()]
        public enum MultivalueComparisonContants
        {
            [EnumMember]
            Union,
            [EnumMember]
            Intersect
        }

        public RepositorySchema()
        {
            this.FieldList = new List<FieldDefinition>();
            this.ID = Guid.NewGuid();
            this.CreatedDate = DateTime.Now;
        }

        public void Load(string fileName)
        {
            try
            {
                this.ID = Guid.NewGuid();
                var document = new XmlDocument();
                document.Load(fileName);
                this.LoadXml(document.OuterXml);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void LoadXml(string xml)
        {
            try
            {
                var document = new XmlDocument();
                document.LoadXml(xml);
                this.Name = XmlHelper.GetNodeValue(document.DocumentElement, "name", string.Empty);
                try
                {
                    this.CreatedDate = DateTime.ParseExact(XmlHelper.GetNodeValue(document.DocumentElement, "createddate", "2010-01-01"), "yyyy-MM-dd HH:mm:ss", null);
                }
                catch (Exception ex)
                {
                    this.CreatedDate = DateTime.ParseExact(XmlHelper.GetNodeValue(document.DocumentElement, "createddate", "2010-01-01"), "yyyy-MM-dd", null);
                }
                this.ID = new Guid(XmlHelper.GetNodeValue(document.DocumentElement, "id", Guid.NewGuid().ToString()));

                if (document.DocumentElement == null) return;
                var nodeList = document.DocumentElement.SelectNodes("fields/field");
                this.FieldList.Clear();
                foreach (XmlNode node in nodeList)
                {
                    #region Load values from XML

                    var name = XmlHelper.GetAttribute(node, "name", string.Empty);

                    DataTypeConstants dataType;
                    var dtXml = XmlHelper.GetAttribute(node, "datatype", DataTypeConstants.String.ToString());
                    if (!Enum.TryParse<DataTypeConstants>(dtXml, out dataType))
                        throw new Exception("Unknown data type: '" + dtXml + "'!");

                    FieldTypeConstants fieldType;
                    var ftXML = XmlHelper.GetAttribute(node, "fieldtype", FieldTypeConstants.Field.ToString());
                    if (!Enum.TryParse<FieldTypeConstants>(ftXML, out fieldType))
                        throw new Exception("Unknown field type: " + ftXML + "!");

                    var length = XmlHelper.GetAttribute(node, "length", 100);
                    bool allowTextSearch;
                    bool.TryParse(XmlHelper.GetAttribute(node, "allowtextsearch", string.Empty), out allowTextSearch);
                    bool isprimarykey;
                    bool.TryParse(XmlHelper.GetAttribute(node, "isprimarykey", string.Empty), out isprimarykey);

                    #endregion

                    FieldDefinition newField = null;
                    if (fieldType == FieldTypeConstants.Dimension)
                    {
                        newField = new DimensionDefinition();
                    }
                    else
                    {
                        newField = new FieldDefinition();
                    }

                    //Setup base field properties
                    if (this.FieldList == null) this.FieldList = new List<FieldDefinition>();
                    this.FieldList.Add(newField);
                    newField.IsPrimaryKey = isprimarykey;
                    newField.AllowTextSearch = allowTextSearch;
                    newField.DataType = dataType;
                    newField.FieldType = fieldType;
                    newField.Length = length;
                    newField.Name = name;

                    //Setup dimension specific properties
                    if (fieldType == FieldTypeConstants.Dimension)
                    {
                        var dimension = newField as DimensionDefinition;
                        if (dimension != null)
                        {
                            int didx;
                            if (int.TryParse(XmlHelper.GetAttribute(node, "didx", "0"), out didx))
                                dimension.DIdx = didx;

                            dimension.Parent = XmlHelper.GetAttribute(node, "parent", string.Empty);

                            var dtypename = XmlHelper.GetAttribute(node, "dimensiontype", string.Empty);
                            DimensionTypeConstants dtype;
                            if (Enum.TryParse<DimensionTypeConstants>(dtypename, out dtype))
                                dimension.DimensionType = dtype;

                            //Process this only for Int data types
                            if (dimension.DataType == DataTypeConstants.Int && XmlHelper.AttributeExists(node, "numericbreak"))
                            {
                                var nb = XmlHelper.GetAttribute(node, "numericbreak", -1);
                                if (nb > 0) dimension.NumericBreak = nb;
                                else throw new Exception("The numeric break value must be greater than zero!");
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ToDisk(string fileName)
        {
            try
            {
                var document = new XmlDocument();
                document.LoadXml(this.ToXml());
                document.Save(fileName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ToXml()
        {
            try
            {
                var document = new XmlDocument();
                document.LoadXml("<repository></repository>");
                XmlHelper.AddElement(document.DocumentElement, "name", this.Name);
                XmlHelper.AddElement(document.DocumentElement, "createddate", this.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                XmlHelper.AddElement(document.DocumentElement, "id", this.ID.ToString());

                var fieldListNode = XmlHelper.AddElement(document.DocumentElement, "fields");

                foreach (var field in this.FieldList)
                {
                    var dimensionDef = field as DimensionDefinition;

                    var fieldNode = XmlHelper.AddElement(fieldListNode, "field");
                    XmlHelper.AddAttribute(fieldNode, "name", field.Name);
                    XmlHelper.AddAttribute(fieldNode, "datatype", field.DataType.ToString());
                    XmlHelper.AddAttribute(fieldNode, "fieldtype", field.FieldType.ToString());

                    if (field.DataType == DataTypeConstants.String)
                    {
                        XmlHelper.AddAttribute(fieldNode, "length", field.Length.ToString());
                        if (field.AllowTextSearch)
                            XmlHelper.AddAttribute(fieldNode, "allowtextsearch", field.AllowTextSearch.ToString());
                    }

                    if (dimensionDef != null && dimensionDef.DataType == DataTypeConstants.Int && dimensionDef.NumericBreak != null)
                    {
                        XmlHelper.AddAttribute(fieldNode, "numericbreak", dimensionDef.NumericBreak.ToString());
                    }

                    if (dimensionDef != null && !string.IsNullOrEmpty(dimensionDef.Parent))
                    {
                        XmlHelper.AddAttribute(fieldNode, "parent", dimensionDef.Parent);
                    }

                    if (dimensionDef != null)
                    {
                        //Check data types
                        if (dimensionDef.DimensionType == DimensionTypeConstants.List && dimensionDef.DataType != DataTypeConstants.List)
                           throw new Exception("A list dimension must have a datatype of list as well.");

                        XmlHelper.AddAttribute(fieldNode, "dimensiontype", dimensionDef.DimensionType.ToString());
                    }

                    if (field.IsPrimaryKey)
                        XmlHelper.AddAttribute(fieldNode, "isprimarykey", field.IsPrimaryKey.ToString());
                }
                return XmlHelper.FormatXMLString(document.OuterXml);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public string CachePath { get; set; }

        [DataMember]
        public List<FieldDefinition> FieldList { get; set; }

        public IEnumerable<DimensionDefinition> DimensionList
        {
            get { return this.FieldList.Where(x => x.FieldType == FieldTypeConstants.Dimension).Cast<DimensionDefinition>().OrderBy(x => x.Name); }
        }

        public FieldDefinition PrimaryKey
        {
            get { return this.FieldList.FirstOrDefault(x => x.IsPrimaryKey); }
        }

        public string GetNamespaceID()
        {
            return "Q" + this.ID.ToString().Replace("-", string.Empty);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public long VersionHash
        {
            get
            {
                var document = new XmlDocument();
                document.LoadXml(this.ToXml());
                XmlHelper.RemoveElement(document, "//name");
                XmlHelper.RemoveElement(document, "//id");
                XmlHelper.RemoveElement(document, "//createddate");
                return Utilities.EncryptionDomain.Hash(document.OuterXml);
            }
        }

    }
}