namespace Celeriq.ManagementStudio
{
	partial class MainForm2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm2));
            this.MainStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainUserList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMainServerProp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMainExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.MainToolbar = new System.Windows.Forms.ToolStrip();
            this.cmdNew = new System.Windows.Forms.ToolStripButton();
            this.cmdLoadSchema = new System.Windows.Forms.ToolStripButton();
            this.cmdCreateFromTemplate = new System.Windows.Forms.ToolStripButton();
            this.cmdCode = new System.Windows.Forms.ToolStripButton();
            this.cmdRefresh = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.serverPropertySheet1 = new Celeriq.ManagementStudio.UserControls.ServerPropertySheet();
            this.MainStatus.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.MainToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainStatus
            // 
            this.MainStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblCount,
            this.lblServer});
            this.MainStatus.Location = new System.Drawing.Point(0, 434);
            this.MainStatus.Name = "MainStatus";
            this.MainStatus.Size = new System.Drawing.Size(717, 22);
            this.MainStatus.TabIndex = 2;
            this.MainStatus.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 17);
            this.lblStatus.Text = "Ready!";
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(330, 17);
            this.lblCount.Spring = true;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblServer
            // 
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(330, 17);
            this.lblServer.Spring = true;
            this.lblServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(717, 24);
            this.MainMenu.TabIndex = 3;
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileConnect,
            this.menuFileDisconnect,
            this.menuSep1,
            this.menuMainUserList,
            this.menuMainServerProp,
            this.toolStripMenuItem2,
            this.menuMainExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuFileConnect
            // 
            this.menuFileConnect.Name = "menuFileConnect";
            this.menuFileConnect.Size = new System.Drawing.Size(178, 22);
            this.menuFileConnect.Text = "&Connect Explorer";
            this.menuFileConnect.Click += new System.EventHandler(this.menuFileConnect_Click);
            // 
            // menuFileDisconnect
            // 
            this.menuFileDisconnect.Name = "menuFileDisconnect";
            this.menuFileDisconnect.Size = new System.Drawing.Size(178, 22);
            this.menuFileDisconnect.Text = "&Disconnect Explorer";
            this.menuFileDisconnect.Click += new System.EventHandler(this.menuFileDisconnect_Click);
            // 
            // menuSep1
            // 
            this.menuSep1.Name = "menuSep1";
            this.menuSep1.Size = new System.Drawing.Size(175, 6);
            // 
            // menuMainUserList
            // 
            this.menuMainUserList.Name = "menuMainUserList";
            this.menuMainUserList.Size = new System.Drawing.Size(178, 22);
            this.menuMainUserList.Text = "&User List";
            this.menuMainUserList.Click += new System.EventHandler(this.menuMainUserList_Click);
            // 
            // menuMainServerProp
            // 
            this.menuMainServerProp.Name = "menuMainServerProp";
            this.menuMainServerProp.Size = new System.Drawing.Size(178, 22);
            this.menuMainServerProp.Text = "&Server Properties";
            this.menuMainServerProp.Click += new System.EventHandler(this.menuMainServerProp_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(175, 6);
            // 
            // menuMainExit
            // 
            this.menuMainExit.Name = "menuMainExit";
            this.menuMainExit.Size = new System.Drawing.Size(178, 22);
            this.menuMainExit.Text = "E&xit";
            this.menuMainExit.Click += new System.EventHandler(this.menuMainExit_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.menuHelpAbout.Text = "&About";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // MainToolbar
            // 
            this.MainToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdNew,
            this.cmdLoadSchema,
            this.cmdCreateFromTemplate,
            this.cmdCode,
            this.cmdRefresh});
            this.MainToolbar.Location = new System.Drawing.Point(0, 24);
            this.MainToolbar.Name = "MainToolbar";
            this.MainToolbar.Size = new System.Drawing.Size(717, 25);
            this.MainToolbar.TabIndex = 4;
            // 
            // cmdNew
            // 
            this.cmdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdNew.Image = ((System.Drawing.Image)(resources.GetObject("cmdNew.Image")));
            this.cmdNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(23, 22);
            this.cmdNew.Text = "New";
            this.cmdNew.ToolTipText = "New Application";
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // cmdLoadSchema
            // 
            this.cmdLoadSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdLoadSchema.Image = ((System.Drawing.Image)(resources.GetObject("cmdLoadSchema.Image")));
            this.cmdLoadSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdLoadSchema.Name = "cmdLoadSchema";
            this.cmdLoadSchema.Size = new System.Drawing.Size(23, 22);
            this.cmdLoadSchema.Text = "Load Schema";
            this.cmdLoadSchema.Click += new System.EventHandler(this.cmdLoadSchema_Click);
            // 
            // cmdCreateFromTemplate
            // 
            this.cmdCreateFromTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdCreateFromTemplate.Image = ((System.Drawing.Image)(resources.GetObject("cmdCreateFromTemplate.Image")));
            this.cmdCreateFromTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdCreateFromTemplate.Name = "cmdCreateFromTemplate";
            this.cmdCreateFromTemplate.Size = new System.Drawing.Size(23, 22);
            this.cmdCreateFromTemplate.Text = "Create from Template";
            this.cmdCreateFromTemplate.Click += new System.EventHandler(this.cmdCreateFromTemplate_Click);
            // 
            // cmdCode
            // 
            this.cmdCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdCode.Image = ((System.Drawing.Image)(resources.GetObject("cmdCode.Image")));
            this.cmdCode.ImageTransparentColor = System.Drawing.Color.White;
            this.cmdCode.Name = "cmdCode";
            this.cmdCode.Size = new System.Drawing.Size(23, 22);
            this.cmdCode.Text = "Client Code";
            this.cmdCode.Click += new System.EventHandler(this.cmdCode_Click);
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdRefresh.Image = ((System.Drawing.Image)(resources.GetObject("cmdRefresh.Image")));
            this.cmdRefresh.ImageTransparentColor = System.Drawing.Color.White;
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(23, 22);
            this.cmdRefresh.Text = "Refresh";
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "database.png");
            this.imageList1.Images.SetKeyName(1, "database_stopped.png");
            this.imageList1.Images.SetKeyName(2, "field.png");
            this.imageList1.Images.SetKeyName(3, "index.png");
            this.imageList1.Images.SetKeyName(4, "OpenFolder.bmp");
            this.imageList1.Images.SetKeyName(5, "server.ico");
            this.imageList1.Images.SetKeyName(6, "user.png");
            // 
            // serverPropertySheet1
            // 
            this.serverPropertySheet1.Credentials = null;
            this.serverPropertySheet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverPropertySheet1.IsConnected = false;
            this.serverPropertySheet1.Location = new System.Drawing.Point(0, 49);
            this.serverPropertySheet1.Name = "serverPropertySheet1";
            this.serverPropertySheet1.PageInfo = ((Celeriq.Common.PagingInfo)(resources.GetObject("serverPropertySheet1.PageInfo")));
            this.serverPropertySheet1.ServerName = null;
            this.serverPropertySheet1.Size = new System.Drawing.Size(717, 385);
            this.serverPropertySheet1.TabIndex = 5;
            // 
            // MainForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 456);
            this.Controls.Add(this.serverPropertySheet1);
            this.Controls.Add(this.MainToolbar);
            this.Controls.Add(this.MainStatus);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainForm2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Celeriq Management Studio";
            this.MainStatus.ResumeLayout(false);
            this.MainStatus.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.MainToolbar.ResumeLayout(false);
            this.MainToolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip MainStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblServer;
		private System.Windows.Forms.MenuStrip MainMenu;
		private System.Windows.Forms.ToolStripMenuItem menuFile;
		private System.Windows.Forms.ToolStripMenuItem menuFileConnect;
		private System.Windows.Forms.ToolStripMenuItem menuFileDisconnect;
		private System.Windows.Forms.ToolStripMenuItem menuHelp;
		private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.ToolStrip MainToolbar;
        private System.Windows.Forms.ToolStripButton cmdCode;
		private System.Windows.Forms.ToolStripButton cmdRefresh;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStripButton cmdNew;
		private System.Windows.Forms.ToolStripButton cmdCreateFromTemplate;
		private System.Windows.Forms.ToolStripSeparator menuSep1;
        private System.Windows.Forms.ToolStripButton cmdLoadSchema;
		private System.Windows.Forms.ToolStripMenuItem menuMainServerProp;
		private System.Windows.Forms.ToolStripMenuItem menuMainUserList;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem menuMainExit;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private UserControls.ServerPropertySheet serverPropertySheet1;
	}
}

