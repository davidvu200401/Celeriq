namespace CeleriqSetup
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Wizard1 = new nHydrate.Wizard.Wizard();
            this.pageSummary = new nHydrate.Wizard.WizardPage();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.pageFinish = new nHydrate.Wizard.WizardPage();
            this.pageDatabase = new nHydrate.Wizard.WizardPage();
            this.chkIsCreate = new System.Windows.Forms.CheckBox();
            this.chkWinAuth = new System.Windows.Forms.CheckBox();
            this.txtDBPassword = new System.Windows.Forms.TextBox();
            this.txtDBUser = new System.Windows.Forms.TextBox();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.txtDBServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pageStart = new nHydrate.Wizard.WizardPage();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chkSetupDatabase = new System.Windows.Forms.CheckBox();
            this.chkSetupService = new System.Windows.Forms.CheckBox();
            this.chkSetupAdminSite = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Wizard1.SuspendLayout();
            this.pageSummary.SuspendLayout();
            this.pageDatabase.SuspendLayout();
            this.pageStart.SuspendLayout();
            this.SuspendLayout();
            // 
            // Wizard1
            // 
            this.Wizard1.ButtonFlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.Wizard1.Controls.Add(this.pageStart);
            this.Wizard1.Controls.Add(this.pageFinish);
            this.Wizard1.Controls.Add(this.pageSummary);
            this.Wizard1.Controls.Add(this.pageDatabase);
            this.Wizard1.HeaderImage = ((System.Drawing.Image)(resources.GetObject("Wizard1.HeaderImage")));
            this.Wizard1.Location = new System.Drawing.Point(0, 0);
            this.Wizard1.Name = "Wizard1";
            this.Wizard1.Size = new System.Drawing.Size(567, 301);
            this.Wizard1.TabIndex = 0;
            this.Wizard1.WizardPages.AddRange(new nHydrate.Wizard.WizardPage[] {
            this.pageStart,
            this.pageDatabase,
            this.pageSummary,
            this.pageFinish});
            // 
            // pageSummary
            // 
            this.pageSummary.Controls.Add(this.label6);
            this.pageSummary.Controls.Add(this.progressBar1);
            this.pageSummary.Controls.Add(this.txtSummary);
            this.pageSummary.Description = "Summary verification";
            this.pageSummary.Location = new System.Drawing.Point(0, 0);
            this.pageSummary.Name = "pageSummary";
            this.pageSummary.Size = new System.Drawing.Size(567, 253);
            this.pageSummary.TabIndex = 10;
            this.pageSummary.Title = "Summary";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(12, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(543, 45);
            this.label6.TabIndex = 2;
            this.label6.Text = "Press the \'Next\' button to start the system setup.";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 225);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(543, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // txtSummary
            // 
            this.txtSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSummary.Location = new System.Drawing.Point(12, 118);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.Size = new System.Drawing.Size(543, 101);
            this.txtSummary.TabIndex = 0;
            // 
            // pageFinish
            // 
            this.pageFinish.Description = "The setup is complete";
            this.pageFinish.Location = new System.Drawing.Point(0, 0);
            this.pageFinish.Name = "pageFinish";
            this.pageFinish.Size = new System.Drawing.Size(567, 253);
            this.pageFinish.TabIndex = 11;
            this.pageFinish.Title = "Finish";
            // 
            // pageDatabase
            // 
            this.pageDatabase.Controls.Add(this.chkIsCreate);
            this.pageDatabase.Controls.Add(this.chkWinAuth);
            this.pageDatabase.Controls.Add(this.txtDBPassword);
            this.pageDatabase.Controls.Add(this.txtDBUser);
            this.pageDatabase.Controls.Add(this.txtDBName);
            this.pageDatabase.Controls.Add(this.txtDBServer);
            this.pageDatabase.Controls.Add(this.label4);
            this.pageDatabase.Controls.Add(this.label3);
            this.pageDatabase.Controls.Add(this.label2);
            this.pageDatabase.Controls.Add(this.label1);
            this.pageDatabase.Description = "Setup the database backing store";
            this.pageDatabase.Location = new System.Drawing.Point(0, 0);
            this.pageDatabase.Name = "pageDatabase";
            this.pageDatabase.Size = new System.Drawing.Size(567, 253);
            this.pageDatabase.TabIndex = 7;
            this.pageDatabase.Title = "Database Setup";
            // 
            // chkIsCreate
            // 
            this.chkIsCreate.AutoSize = true;
            this.chkIsCreate.Checked = true;
            this.chkIsCreate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsCreate.Location = new System.Drawing.Point(116, 126);
            this.chkIsCreate.Name = "chkIsCreate";
            this.chkIsCreate.Size = new System.Drawing.Size(104, 17);
            this.chkIsCreate.TabIndex = 5;
            this.chkIsCreate.Text = "Create database";
            this.chkIsCreate.UseVisualStyleBackColor = true;
            // 
            // chkWinAuth
            // 
            this.chkWinAuth.AutoSize = true;
            this.chkWinAuth.Location = new System.Drawing.Point(391, 126);
            this.chkWinAuth.Name = "chkWinAuth";
            this.chkWinAuth.Size = new System.Drawing.Size(163, 17);
            this.chkWinAuth.TabIndex = 6;
            this.chkWinAuth.Text = "Use Windows Authentication";
            this.chkWinAuth.UseVisualStyleBackColor = true;
            // 
            // txtDBPassword
            // 
            this.txtDBPassword.Location = new System.Drawing.Point(403, 100);
            this.txtDBPassword.Name = "txtDBPassword";
            this.txtDBPassword.PasswordChar = '*';
            this.txtDBPassword.Size = new System.Drawing.Size(151, 20);
            this.txtDBPassword.TabIndex = 4;
            // 
            // txtDBUser
            // 
            this.txtDBUser.Location = new System.Drawing.Point(116, 100);
            this.txtDBUser.Name = "txtDBUser";
            this.txtDBUser.Size = new System.Drawing.Size(156, 20);
            this.txtDBUser.TabIndex = 3;
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(403, 74);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(151, 20);
            this.txtDBName.TabIndex = 2;
            // 
            // txtDBServer
            // 
            this.txtDBServer.Location = new System.Drawing.Point(116, 74);
            this.txtDBServer.Name = "txtDBServer";
            this.txtDBServer.Size = new System.Drawing.Size(156, 20);
            this.txtDBServer.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "User ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(300, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Database:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // pageStart
            // 
            this.pageStart.Controls.Add(this.label7);
            this.pageStart.Controls.Add(this.chkSetupAdminSite);
            this.pageStart.Controls.Add(this.chkSetupService);
            this.pageStart.Controls.Add(this.chkSetupDatabase);
            this.pageStart.Controls.Add(this.label5);
            this.pageStart.Description = "Getting Started";
            this.pageStart.Location = new System.Drawing.Point(0, 0);
            this.pageStart.Name = "pageStart";
            this.pageStart.Size = new System.Drawing.Size(567, 253);
            this.pageStart.TabIndex = 9;
            this.pageStart.Title = "Celeriq Setup";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(12, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(543, 92);
            this.label5.TabIndex = 0;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // chkSetupDatabase
            // 
            this.chkSetupDatabase.AutoSize = true;
            this.chkSetupDatabase.Checked = true;
            this.chkSetupDatabase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSetupDatabase.Location = new System.Drawing.Point(43, 191);
            this.chkSetupDatabase.Name = "chkSetupDatabase";
            this.chkSetupDatabase.Size = new System.Drawing.Size(72, 17);
            this.chkSetupDatabase.TabIndex = 1;
            this.chkSetupDatabase.Text = "Database";
            this.chkSetupDatabase.UseVisualStyleBackColor = true;
            // 
            // chkSetupService
            // 
            this.chkSetupService.AutoSize = true;
            this.chkSetupService.Checked = true;
            this.chkSetupService.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSetupService.Location = new System.Drawing.Point(157, 191);
            this.chkSetupService.Name = "chkSetupService";
            this.chkSetupService.Size = new System.Drawing.Size(107, 17);
            this.chkSetupService.TabIndex = 2;
            this.chkSetupService.Text = "Windows service";
            this.chkSetupService.UseVisualStyleBackColor = true;
            // 
            // chkSetupAdminSite
            // 
            this.chkSetupAdminSite.AutoSize = true;
            this.chkSetupAdminSite.Enabled = false;
            this.chkSetupAdminSite.Location = new System.Drawing.Point(311, 191);
            this.chkSetupAdminSite.Name = "chkSetupAdminSite";
            this.chkSetupAdminSite.Size = new System.Drawing.Size(110, 17);
            this.chkSetupAdminSite.TabIndex = 3;
            this.chkSetupAdminSite.Text = "Administration site";
            this.chkSetupAdminSite.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 164);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Setup Components";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 301);
            this.Controls.Add(this.Wizard1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Celeriq Setup";
            this.Wizard1.ResumeLayout(false);
            this.Wizard1.PerformLayout();
            this.pageSummary.ResumeLayout(false);
            this.pageSummary.PerformLayout();
            this.pageDatabase.ResumeLayout(false);
            this.pageDatabase.PerformLayout();
            this.pageStart.ResumeLayout(false);
            this.pageStart.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private nHydrate.Wizard.Wizard Wizard1;
        private nHydrate.Wizard.WizardPage pageDatabase;
        private System.Windows.Forms.TextBox txtDBPassword;
        private System.Windows.Forms.TextBox txtDBUser;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.TextBox txtDBServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkWinAuth;
        private nHydrate.Wizard.WizardPage pageStart;
        private System.Windows.Forms.Label label5;
        private nHydrate.Wizard.WizardPage pageSummary;
        private nHydrate.Wizard.WizardPage pageFinish;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.CheckBox chkIsCreate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chkSetupAdminSite;
        private System.Windows.Forms.CheckBox chkSetupService;
        private System.Windows.Forms.CheckBox chkSetupDatabase;
        private System.Windows.Forms.Label label7;
    }
}

