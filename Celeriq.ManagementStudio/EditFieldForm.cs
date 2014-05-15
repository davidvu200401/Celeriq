using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.Common;

namespace Celeriq.ManagementStudio
{
    public partial class EditFieldForm : Form
    {
        private RepositorySchema _repository = null;
        private FieldDefinition _field = null;

        public EditFieldForm()
        {
            InitializeComponent();

            txtName.GotFocus += TextboxSelectOnEnter;
            txtName.Enter += TextboxSelectOnEnter;

            lblLengthDefined.Location = udLength.Location;
            lblLengthDefined.Size = udLength.Size;

            cboDataType.Items.Clear();

            //Data types
            var dataTypeList = Enum.GetNames(typeof(RepositorySchema.DataTypeConstants));
            foreach (var item in dataTypeList)
            {
                cboDataType.Items.Add(item.ToString());
            }

            //Dimension types
            var dimensionTypeList = Enum.GetNames(typeof(RepositorySchema.DimensionTypeConstants));
            foreach (var item in dimensionTypeList)
            {
                cboDimensionType.Items.Add(item.ToString());
            }

            cboDimensionType.SelectedIndexChanged += cboDimensionType_SelectedIndexChanged;
            //this.RefreshControls();
        }

        private void cboDimensionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshControls();
        }

        private void RefreshControls()
        {
            if ((string)cboDimensionType.SelectedItem == "List")
            {
                cboDataType.Enabled = false;
                cboDataType.SelectedItem = "List";
            }
            else
            {
                cboDataType.Enabled = true;
            }
        }

        private void TextboxSelectOnEnter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        public EditFieldForm(FieldDefinition field, RepositorySchema repository)
            : this()
        {
            _field = field;
            _repository = repository;

            txtName.Text = field.Name;
            chkAllowTextSearch.Checked = field.AllowTextSearch;
            cboDataType.SelectedItem = Enum.Parse(typeof(RepositorySchema.DataTypeConstants), field.DataType.ToString()).ToString();
            lblFieldType.Text = field.FieldType.ToString();

            var dimension = field as DimensionDefinition;
            if (dimension != null)
            {
                //Load all dimensions in the parent box
                cboParent.Items.Add("(No Parent)");
                cboParent.SelectedIndex = 0;
                cboParent.Items.AddRange(repository.FieldList.Where(x => x.FieldType == RepositorySchema.FieldTypeConstants.Dimension && x != field).Select(x => x.Name).ToArray());

                cboParent.SelectedItem = dimension.Parent;
                cboDimensionType.SelectedItem = Enum.Parse(typeof(RepositorySchema.DimensionTypeConstants), dimension.DimensionType.ToString()).ToString();
            }
            else
            {
                cboParent.Visible = false;
                lblParent.Visible = false;
            }
            this.EnableFields();

            chkPrimaryKey.Checked = field.IsPrimaryKey;
            udLength.Value = field.Length;
        }

        public FieldDefinition GetNewDefinition()
        {
            return null;
        }

        private void EnableFields()
        {
            var t = (RepositorySchema.FieldTypeConstants)Enum.Parse(typeof(RepositorySchema.FieldTypeConstants), lblFieldType.Text);
            if (t == RepositorySchema.FieldTypeConstants.Dimension)
            {
                lblDimensionType.Enabled = true;
                cboDimensionType.Enabled = true;
            }
            else
            {
                cboDimensionType.SelectedIndex = 0;
                lblDimensionType.Enabled = false;
                cboDimensionType.Enabled = false;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            _field.Name = txtName.Text;
            _field.AllowTextSearch = chkAllowTextSearch.Checked;
            _field.DataType = (RepositorySchema.DataTypeConstants)Enum.Parse(typeof(RepositorySchema.DataTypeConstants), cboDataType.SelectedItem.ToString());
            //_field.FieldType = (RepositoryDefinition.FieldTypeConstants)Enum.Parse(typeof(RepositoryDefinition.FieldTypeConstants), cboFieldType.SelectedItem.ToString());

            var dimension = _field as DimensionDefinition;
            if (dimension != null)
            {
                dimension.DimensionType = (RepositorySchema.DimensionTypeConstants)Enum.Parse(typeof(RepositorySchema.DimensionTypeConstants), cboDimensionType.SelectedItem.ToString());
                if (cboParent.SelectedIndex == 0)
                    dimension.Parent = null;
                else
                    dimension.Parent = (string)cboParent.SelectedItem;
            }
            this.EnableFields();

            _field.IsPrimaryKey = chkPrimaryKey.Checked;
            _field.Length = (int)udLength.Value;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cboFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableFields();
        }

        private void cboDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepositorySchema.DataTypeConstants a;
            if (Enum.TryParse<RepositorySchema.DataTypeConstants>(cboDataType.SelectedItem.ToString(), true, out a))
            {
                chkPrimaryKey.Enabled = (a == RepositorySchema.DataTypeConstants.Int);
                if (!chkPrimaryKey.Enabled) chkPrimaryKey.Checked = false;

                chkAllowTextSearch.Enabled = (a == RepositorySchema.DataTypeConstants.String);
                if (!chkAllowTextSearch.Enabled) chkAllowTextSearch.Checked = false;

                switch (a)
                {
                    case RepositorySchema.DataTypeConstants.Bool:
                        udLength.Visible = false;
                        lblLengthDefined.Visible = true;
                        break;
                    case RepositorySchema.DataTypeConstants.DateTime:
                        udLength.Visible = false;
                        lblLengthDefined.Visible = true;
                        break;
                    case RepositorySchema.DataTypeConstants.Float:
                        udLength.Visible = false;
                        lblLengthDefined.Visible = true;
                        break;
                    case RepositorySchema.DataTypeConstants.GeoCode:
                        udLength.Visible = false;
                        lblLengthDefined.Visible = true;
                        break;
                    case RepositorySchema.DataTypeConstants.Int:
                        udLength.Visible = false;
                        lblLengthDefined.Visible = true;
                        break;
                    case RepositorySchema.DataTypeConstants.List:
                        udLength.Visible = false;
                        lblLengthDefined.Visible = true;
                        break;
                    case RepositorySchema.DataTypeConstants.String:
                        udLength.Visible = true;
                        lblLengthDefined.Visible = false;
                        break;
                }
            }
        }

    }
}