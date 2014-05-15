namespace Celeriq.ManagementStudio
{
	partial class EditFieldForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.chkAllowTextSearch = new System.Windows.Forms.CheckBox();
            this.chkPrimaryKey = new System.Windows.Forms.CheckBox();
            this.cboDataType = new System.Windows.Forms.ComboBox();
            this.udLength = new System.Windows.Forms.NumericUpDown();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDataType = new System.Windows.Forms.Label();
            this.lblFieldTypeHeader = new System.Windows.Forms.Label();
            this.lblLength = new System.Windows.Forms.Label();
            this.cboDimensionType = new System.Windows.Forms.ComboBox();
            this.lblDimensionType = new System.Windows.Forms.Label();
            this.lblFieldType = new System.Windows.Forms.Label();
            this.cboParent = new System.Windows.Forms.ComboBox();
            this.lblParent = new System.Windows.Forms.Label();
            this.lblLengthDefined = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udLength)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(256, 238);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(175, 238);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // chkAllowTextSearch
            // 
            this.chkAllowTextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllowTextSearch.AutoSize = true;
            this.chkAllowTextSearch.Location = new System.Drawing.Point(121, 179);
            this.chkAllowTextSearch.Name = "chkAllowTextSearch";
            this.chkAllowTextSearch.Size = new System.Drawing.Size(112, 17);
            this.chkAllowTextSearch.TabIndex = 6;
            this.chkAllowTextSearch.Text = "Allow Text Search";
            this.chkAllowTextSearch.UseVisualStyleBackColor = true;
            // 
            // chkPrimaryKey
            // 
            this.chkPrimaryKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPrimaryKey.AutoSize = true;
            this.chkPrimaryKey.Location = new System.Drawing.Point(121, 202);
            this.chkPrimaryKey.Name = "chkPrimaryKey";
            this.chkPrimaryKey.Size = new System.Drawing.Size(92, 17);
            this.chkPrimaryKey.TabIndex = 7;
            this.chkPrimaryKey.Text = "Is Primary Key";
            this.chkPrimaryKey.UseVisualStyleBackColor = true;
            // 
            // cboDataType
            // 
            this.cboDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataType.FormattingEnabled = true;
            this.cboDataType.Location = new System.Drawing.Point(121, 38);
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size(213, 21);
            this.cboDataType.TabIndex = 1;
            this.cboDataType.SelectedIndexChanged += new System.EventHandler(this.cboDataType_SelectedIndexChanged);
            // 
            // udLength
            // 
            this.udLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.udLength.Location = new System.Drawing.Point(121, 119);
            this.udLength.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.udLength.Name = "udLength";
            this.udLength.Size = new System.Drawing.Size(212, 20);
            this.udLength.TabIndex = 4;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(121, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(213, 20);
            this.txtName.TabIndex = 0;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "Name:";
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(12, 38);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(56, 13);
            this.lblDataType.TabIndex = 7;
            this.lblDataType.Text = "Data type:";
            // 
            // lblFieldTypeHeader
            // 
            this.lblFieldTypeHeader.AutoSize = true;
            this.lblFieldTypeHeader.Location = new System.Drawing.Point(12, 65);
            this.lblFieldTypeHeader.Name = "lblFieldTypeHeader";
            this.lblFieldTypeHeader.Size = new System.Drawing.Size(55, 13);
            this.lblFieldTypeHeader.TabIndex = 7;
            this.lblFieldTypeHeader.Text = "Field type:";
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(12, 119);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(43, 13);
            this.lblLength.TabIndex = 7;
            this.lblLength.Text = "Length:";
            // 
            // cboDimensionType
            // 
            this.cboDimensionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDimensionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDimensionType.FormattingEnabled = true;
            this.cboDimensionType.Location = new System.Drawing.Point(121, 92);
            this.cboDimensionType.Name = "cboDimensionType";
            this.cboDimensionType.Size = new System.Drawing.Size(213, 21);
            this.cboDimensionType.TabIndex = 3;
            // 
            // lblDimensionType
            // 
            this.lblDimensionType.AutoSize = true;
            this.lblDimensionType.Location = new System.Drawing.Point(13, 92);
            this.lblDimensionType.Name = "lblDimensionType";
            this.lblDimensionType.Size = new System.Drawing.Size(82, 13);
            this.lblDimensionType.TabIndex = 7;
            this.lblDimensionType.Text = "Dimension type:";
            // 
            // lblFieldType
            // 
            this.lblFieldType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFieldType.BackColor = System.Drawing.SystemColors.Control;
            this.lblFieldType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFieldType.Location = new System.Drawing.Point(121, 66);
            this.lblFieldType.Name = "lblFieldType";
            this.lblFieldType.Size = new System.Drawing.Size(213, 23);
            this.lblFieldType.TabIndex = 9;
            this.lblFieldType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboParent
            // 
            this.cboParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboParent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParent.FormattingEnabled = true;
            this.cboParent.Location = new System.Drawing.Point(121, 145);
            this.cboParent.Name = "cboParent";
            this.cboParent.Size = new System.Drawing.Size(213, 21);
            this.cboParent.TabIndex = 5;
            // 
            // lblParent
            // 
            this.lblParent.AutoSize = true;
            this.lblParent.Location = new System.Drawing.Point(13, 145);
            this.lblParent.Name = "lblParent";
            this.lblParent.Size = new System.Drawing.Size(41, 13);
            this.lblParent.TabIndex = 7;
            this.lblParent.Text = "Parent:";
            // 
            // lblLengthDefined
            // 
            this.lblLengthDefined.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLengthDefined.BackColor = System.Drawing.SystemColors.Control;
            this.lblLengthDefined.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLengthDefined.Location = new System.Drawing.Point(134, 135);
            this.lblLengthDefined.Name = "lblLengthDefined";
            this.lblLengthDefined.Size = new System.Drawing.Size(213, 23);
            this.lblLengthDefined.TabIndex = 10;
            this.lblLengthDefined.Text = "(Predefined)";
            this.lblLengthDefined.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLengthDefined.Visible = false;
            // 
            // EditFieldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(343, 273);
            this.Controls.Add(this.lblLengthDefined);
            this.Controls.Add(this.lblFieldType);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.lblParent);
            this.Controls.Add(this.lblDimensionType);
            this.Controls.Add(this.lblFieldTypeHeader);
            this.Controls.Add(this.lblDataType);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.udLength);
            this.Controls.Add(this.cboParent);
            this.Controls.Add(this.cboDimensionType);
            this.Controls.Add(this.cboDataType);
            this.Controls.Add(this.chkPrimaryKey);
            this.Controls.Add(this.chkAllowTextSearch);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditFieldForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit";
            ((System.ComponentModel.ISupportInitialize)(this.udLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.CheckBox chkAllowTextSearch;
		private System.Windows.Forms.CheckBox chkPrimaryKey;
		private System.Windows.Forms.ComboBox cboDataType;
		private System.Windows.Forms.NumericUpDown udLength;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblDataType;
		private System.Windows.Forms.Label lblFieldTypeHeader;
		private System.Windows.Forms.Label lblLength;
		private System.Windows.Forms.ComboBox cboDimensionType;
		private System.Windows.Forms.Label lblDimensionType;
		private System.Windows.Forms.Label lblFieldType;
		private System.Windows.Forms.ComboBox cboParent;
		private System.Windows.Forms.Label lblParent;
		private System.Windows.Forms.Label lblLengthDefined;
	}
}