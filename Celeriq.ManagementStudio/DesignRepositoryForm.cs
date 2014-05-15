using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celeriq.Common;
using Celeriq.ManagementStudio.Objects;
using Celeriq.Common;

namespace Celeriq.ManagementStudio
{
    internal partial class DesignRepositoryForm : Form
    {
        private IRemotingObject _cache = null;

        public DesignRepositoryForm()
        {
            InitializeComponent();

            const int ColWidth = 110;

            lvwColumn.Columns.Clear();
            lvwColumn.Columns.Add("Primary key", ColWidth);
            lvwColumn.Columns.Add("Name", ColWidth);
            lvwColumn.Columns.Add("Data type", ColWidth);
            lvwColumn.Columns.Add("Field type", ColWidth);
            lvwColumn.Columns.Add("Dimension type", ColWidth);
            lvwColumn.Columns.Add("Length", ColWidth);
            lvwColumn.Columns.Add("Text search", ColWidth);
            lvwColumn.Columns.Add("Parent", ColWidth);

            lvwColumn.SelectedIndexChanged += new EventHandler(lvwColumn_SelectedIndexChanged);
            lvwColumn.KeyDown += lvwColumn_KeyDown;
            this.UpdateUI();
        }

        private void lvwColumn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.RemoveSelected();
            }
        }

        public void Populate(IRemotingObject cache)
        {
            _cache = cache;
            txtName.Text = cache.Repository.Name;

            foreach (var field in cache.Repository.FieldList)
            {
                var newItem = new ListViewItem();
                UpdateField(newItem, field);
                lvwColumn.Items.Add(newItem);
            }

        }

        private void UpdateField(ListViewItem listItem, IFieldDefinition field)
        {
            listItem.SubItems.Clear();
            listItem.Text = field.IsPrimaryKey.ToString();
            listItem.SubItems.Add(field.Name);
            listItem.SubItems.Add(field.DataType.ToString());
            listItem.SubItems.Add(field.FieldType.ToString());
            var dimension = field as DimensionDefinition;
            if (dimension != null)
                listItem.SubItems.Add(dimension.DimensionType.ToString());
            else
                listItem.SubItems.Add(string.Empty);

            if (field.DataType == RepositorySchema.DataTypeConstants.String)
                listItem.SubItems.Add(field.Length.ToString());
            else
                listItem.SubItems.Add("---");

            if (field.AllowTextSearch)
                listItem.SubItems.Add(field.AllowTextSearch.ToString());
            else
                listItem.SubItems.Add(string.Empty);

            if (dimension != null)
                listItem.SubItems.Add(dimension.Parent);
            else
                listItem.SubItems.Add(string.Empty);

            listItem.Tag = field;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("The repository must have a name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_cache.Repository.FieldList.Any() && !_cache.Repository.DimensionList.Any())
            {
                MessageBox.Show("The repository must have at least one dimension or field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_cache.Repository.FieldList.Count(x => x.IsPrimaryKey) != 1)
            {
                MessageBox.Show("The repository must have exactly one field marked as primary key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_cache.Repository.FieldList.Count() != _cache.Repository.FieldList.Select(x => x.Name.ToLower()).Distinct().Count())
            {
                MessageBox.Show("All repository fields must have a unique name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_cache.Repository.DimensionList.Count() != _cache.Repository.DimensionList.Select(x => x.Name.ToLower()).Distinct().Count())
            {
                MessageBox.Show("All repository dimensions must have a unique name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Saving this repository will clear all data from it!", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                _cache.Repository.Name = txtName.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void EditColumn(ListViewItem item)
        {
            var field = item.Tag as FieldDefinition;
            if (field != null)
            {
                var F = new EditFieldForm(field, _cache.Repository);
                if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //TODO
                    this.UpdateField(item, field);
                }
            }
        }

        private void lvwColumn_DoubleClick(object sender, EventArgs e)
        {
            if (lvwColumn.SelectedItems.Count == 1)
            {
                EditColumn(lvwColumn.SelectedItems[0]);
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            var newItem = new ListViewItem();
            var field = new FieldDefinition() { Name = "[New Field]" };
            _cache.Repository.FieldList.Add(field);
            UpdateField(newItem, field);
            lvwColumn.Items.Add(newItem);
            newItem.Selected = true;
            this.UpdateUI();
            this.EditColumn(newItem);
        }

        private void cmdAddDimension_Click(object sender, EventArgs e)
        {
            var newItem = new ListViewItem();
            var field = new DimensionDefinition() { Name = "[New Dimension]" };
            _cache.Repository.FieldList.Add(field);
            UpdateField(newItem, field);
            lvwColumn.Items.Add(newItem);
            newItem.Selected = true;
            this.UpdateUI();
            this.EditColumn(newItem);
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            RemoveSelected();
        }

        private void RemoveSelected()
        {
            if (lvwColumn.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Do you wish to delete this field?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    var field = lvwColumn.SelectedItems[0].Tag as FieldDefinition;
                    _cache.Repository.FieldList.Remove(field);
                    lvwColumn.Items.Remove(lvwColumn.SelectedItems[0]);
                }
            }
            this.UpdateUI();
        }

        private void lvwColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateUI();
        }

        private void UpdateUI()
        {
            cmdRemove.Enabled = (lvwColumn.SelectedItems.Count > 0);
        }

    }
}