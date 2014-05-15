using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CeleriqSetup.EventArguments;

namespace CeleriqSetup
{
    public partial class MainForm : Form
    {
        private bool _processingComplete = false;

        public MainForm()
        {
            InitializeComponent();

            chkWinAuth.Click += chkWinAuth_Click;
            Wizard1.BeforeSwitchPages += Wizard1_BeforeSwitchPages;
            Wizard1.AfterSwitchPages += Wizard1_AfterSwitchPages;
            Wizard1.FinishEnabled = false;
            timer1.Enabled = false;
            timer1.Tick += timer1_Tick;
        }

        private int _percentDone = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_percentDone < 100)
            {
                _percentDone += 1;
                progressBar1.Value = _percentDone;
            }
        }

        private void Wizard1_BeforeSwitchPages(object sender, nHydrate.Wizard.Wizard.BeforeSwitchPagesEventArgs e)
        {
            try
            {
                if (e.OldIndex == this.Wizard1.WizardPages.IndexOf(pageStart) && (e.OldIndex < e.NewIndex))
                {
                    if (!chkSetupDatabase.Checked)
                    {
                        Wizard1.WizardPages.Remove(pageDatabase);
                    }
                    else if(!Wizard1.WizardPages.Contains(pageDatabase))
                    {
                        Wizard1.WizardPages.Insert(this.Wizard1.WizardPages.IndexOf(pageStart) + 1, pageDatabase);
                    }
                }
                else if (e.OldIndex == this.Wizard1.WizardPages.IndexOf(pageDatabase) && (e.OldIndex < e.NewIndex))
                {
                    var isValid = true;
                    if (string.IsNullOrEmpty(txtDBName.Text)) isValid = false;
                    if (string.IsNullOrEmpty(txtDBServer.Text)) isValid = false;
                    if (!chkWinAuth.Checked)
                    {
                        if (string.IsNullOrEmpty(txtDBUser.Text)) isValid = false;
                        if (string.IsNullOrEmpty(txtDBPassword.Text)) isValid = false;
                    }
                    if (!isValid)
                    {
                        MessageBox.Show("The database configuration is not valid.", "Fix Issues", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                }
                else if (e.OldIndex == this.Wizard1.WizardPages.IndexOf(pageSummary) && (e.OldIndex < e.NewIndex))
                {
                    if (!_processingComplete)
                    {
                        //When leaving the summary page run all processes
                        Wizard1.NextEnabled = false;
                        Wizard1.BackEnabled = false;
                        Wizard1.CancelEnabled = false;
                        this.ProcessSetup();
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Wizard1_AfterSwitchPages(object sender, nHydrate.Wizard.Wizard.AfterSwitchPagesEventArgs e)
        {
            Wizard1.FinishEnabled = (e.NewIndex == Wizard1.WizardPages.Count - 1);

            if (e.NewIndex == this.Wizard1.WizardPages.IndexOf(pageFinish) && (e.OldIndex < e.NewIndex))
            {
                //Leave button disabled
                //The thread will complete and fix all of this
                Wizard1.NextEnabled = false;
                Wizard1.BackEnabled = false;
                Wizard1.CancelEnabled = false;
            }
        }

        private void chkWinAuth_Click(object sender, EventArgs e)
        {
            txtDBUser.Enabled = !chkWinAuth.Checked;
            txtDBPassword.Enabled = !chkWinAuth.Checked;
            if (chkWinAuth.Checked)
            {
                txtDBUser.Text = string.Empty;
                txtDBPassword.Text = string.Empty;
            }
        }

        private void ProcessSetup()
        {
            try
            {
                var setup = new Celeriq.DataCore.Install.InstallSetup();
                setup.AcceptVersionWarnings = true;
                setup.InstallStatus = (chkIsCreate.Checked ? Celeriq.DataCore.Install.InstallStatusConstants.Create : Celeriq.DataCore.Install.InstallStatusConstants.Upgrade);
                var builder = new SqlConnectionStringBuilder();
                builder.DataSource = txtDBServer.Text;
                builder.InitialCatalog = txtDBName.Text;
                if (chkWinAuth.Checked)
                {
                    builder.IntegratedSecurity = true;
                }
                else
                {
                    builder.IntegratedSecurity = false;
                    builder.UserID = txtDBUser.Text;
                    builder.Password = txtDBPassword.Text;
                }
                setup.ConnectionString = builder.ToString();

                if (chkIsCreate.Checked)
                {
                    setup.NewDatabaseName = txtDBName.Text;
                    builder.InitialCatalog = "master";
                    setup.MasterConnectionString = builder.ToString();
                }

                AddProgressMessage(null, new TextChangedEventArgs("Started database installation..."));

                var work = new WorkThreader(setup);
                work.UseAdminsite = chkSetupAdminSite.Checked;
                work.UseDatabase = chkSetupDatabase.Checked;
                work.UseService = chkSetupService.Checked;
                work.Complete += work_Complete;
                work.ProgressText += work_ProgressText;
                var t = new System.Threading.Thread(new System.Threading.ThreadStart(work.Run));
                t.Start();
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                _processingComplete = true;
                Wizard1.SelectedPage = pageFinish;
                Wizard1.FinishEnabled = true;
                Wizard1.NextEnabled = false;
                Wizard1.BackEnabled = false;
                Wizard1.CancelEnabled = false;
            }
        }

        private void ProcessComplete(object sender, EventArgs e)
        {
            Wizard1.FinishEnabled = true;
            Wizard1.NextEnabled = false;
            Wizard1.BackEnabled = false;
            Wizard1.CancelEnabled = false;
            timer1.Enabled = false;
            progressBar1.Value = 100;
        }

        private void work_Complete(object sender, EventArgs e)
        {
            this.BeginInvoke(new EventHandler(ProcessComplete), e);
        }

        delegate void SetTextCallback(string text);

        private void AddProgressMessage(object o, CeleriqSetup.EventArguments.TextChangedEventArgs e)
        {
            txtSummary.Text += e.Text + "\r\n";
        }

        private void work_ProgressText(object sender, EventArguments.TextChangedEventArgs e)
        {
            this.BeginInvoke(new EventHandler<CeleriqSetup.EventArguments.TextChangedEventArgs>(AddProgressMessage), e.Text);
        }

    }
}