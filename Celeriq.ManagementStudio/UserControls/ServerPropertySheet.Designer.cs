namespace Celeriq.ManagementStudio.UserControls
{
	partial class ServerPropertySheet
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerPropertySheet));
            this.lvwItem = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlAction = new System.Windows.Forms.Panel();
            this.lblDetails = new System.Windows.Forms.Label();
            this.cboRPP = new System.Windows.Forms.ComboBox();
            this.txtPageNo = new System.Windows.Forms.TextBox();
            this.cmdPageNext = new System.Windows.Forms.Button();
            this.cmdPageBack = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwItem
            // 
            this.lvwItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwItem.FullRowSelect = true;
            this.lvwItem.HideSelection = false;
            this.lvwItem.Location = new System.Drawing.Point(0, 37);
            this.lvwItem.Name = "lvwItem";
            this.lvwItem.Size = new System.Drawing.Size(573, 147);
            this.lvwItem.SmallImageList = this.imageList1;
            this.lvwItem.StateImageList = this.imageList1;
            this.lvwItem.TabIndex = 0;
            this.lvwItem.UseCompatibleStateImageBehavior = false;
            this.lvwItem.View = System.Windows.Forms.View.Details;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "database.png");
            this.imageList1.Images.SetKeyName(1, "database_stopped.png");
            // 
            // pnlAction
            // 
            this.pnlAction.Controls.Add(this.lblDetails);
            this.pnlAction.Controls.Add(this.cboRPP);
            this.pnlAction.Controls.Add(this.txtPageNo);
            this.pnlAction.Controls.Add(this.cmdPageNext);
            this.pnlAction.Controls.Add(this.cmdPageBack);
            this.pnlAction.Controls.Add(this.txtFilter);
            this.pnlAction.Controls.Add(this.label1);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAction.Enabled = false;
            this.pnlAction.Location = new System.Drawing.Point(0, 0);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(573, 37);
            this.pnlAction.TabIndex = 1;
            // 
            // lblDetails
            // 
            this.lblDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetails.Location = new System.Drawing.Point(424, 10);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(146, 17);
            this.lblDetails.TabIndex = 6;
            // 
            // cboRPP
            // 
            this.cboRPP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboRPP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRPP.FormattingEnabled = true;
            this.cboRPP.Location = new System.Drawing.Point(258, 5);
            this.cboRPP.Name = "cboRPP";
            this.cboRPP.Size = new System.Drawing.Size(69, 21);
            this.cboRPP.TabIndex = 5;
            this.cboRPP.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtPageNo
            // 
            this.txtPageNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPageNo.Location = new System.Drawing.Point(354, 6);
            this.txtPageNo.Name = "txtPageNo";
            this.txtPageNo.ReadOnly = true;
            this.txtPageNo.Size = new System.Drawing.Size(40, 20);
            this.txtPageNo.TabIndex = 3;
            this.txtPageNo.Text = "1";
            this.txtPageNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPageNo.WordWrap = false;
            // 
            // cmdPageNext
            // 
            this.cmdPageNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPageNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPageNext.Location = new System.Drawing.Point(393, 6);
            this.cmdPageNext.Name = "cmdPageNext";
            this.cmdPageNext.Size = new System.Drawing.Size(23, 20);
            this.cmdPageNext.TabIndex = 4;
            this.cmdPageNext.Text = ">";
            this.cmdPageNext.UseVisualStyleBackColor = true;
            this.cmdPageNext.Click += new System.EventHandler(this.cmdPageNext_Click);
            // 
            // cmdPageBack
            // 
            this.cmdPageBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPageBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPageBack.Location = new System.Drawing.Point(333, 6);
            this.cmdPageBack.Name = "cmdPageBack";
            this.cmdPageBack.Size = new System.Drawing.Size(23, 20);
            this.cmdPageBack.TabIndex = 2;
            this.cmdPageBack.Text = "<";
            this.cmdPageBack.UseVisualStyleBackColor = true;
            this.cmdPageBack.Click += new System.EventHandler(this.cmdPageBack_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(60, 7);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(192, 20);
            this.txtFilter.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filter:";
            // 
            // ServerPropertySheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvwItem);
            this.Controls.Add(this.pnlAction);
            this.Name = "ServerPropertySheet";
            this.Size = new System.Drawing.Size(573, 184);
            this.pnlAction.ResumeLayout(false);
            this.pnlAction.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvwItem;
		private System.Windows.Forms.Panel pnlAction;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox txtPageNo;
        private System.Windows.Forms.Button cmdPageNext;
        private System.Windows.Forms.Button cmdPageBack;
        private System.Windows.Forms.ComboBox cboRPP;
        private System.Windows.Forms.Label lblDetails;

	}
}
