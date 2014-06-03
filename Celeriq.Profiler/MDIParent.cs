using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Celeriq.Profiler
{
    public partial class MDIParent : Form
    {
        private int childFormNumber = 0;

        public MDIParent()
        {
            InitializeComponent();
            EnableMenus();

            var timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 500;
            timer.Start();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            if (timer != null)
                timer.Stop();

            this.OpenConnection();
        }

        #region Menu Handlers

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        #endregion

        private void EnableMenus()
        {
            newToolStripButton.Enabled = true;
            var child = this.ActiveMdiChild as MainForm;
            if (child == null)
            {
                playToolStripButton.Enabled = false;
                stopToolStripButton.Enabled = false;
                autoScrollToolStripButton.Enabled = false;
                findToolStripButton.Enabled = false;
                clearToolStripButton.Enabled = false;
            }
            else
            {
                playToolStripButton.Enabled = !child.IsRunning;
                stopToolStripButton.Enabled = child.IsRunning;
                autoScrollToolStripButton.Enabled = true;
                autoScrollToolStripButton.Checked = child.AutoScroll;
                findToolStripButton.Enabled = true;
                clearToolStripButton.Enabled = true;
            }
        }

        private void OpenConnection()
        {
            var F = new ServerConnectionForm();
            if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var childForm = new MainForm();
                childForm.MdiParent = this;
                childForm.Text = "Window " + childFormNumber++;
                childForm.Credentials = F.Credentials;
                childForm.Repository = F.Repository;
                childForm.InitSize();
                childForm.Show();

                childForm.Credentials = F.Credentials;
                childForm.PublicKey = F.PublicKey;
                childForm.ServerName = F.ServerName;
                childForm.Repository = F.Repository;
                childForm.StartProfiling();

                this.EnableMenus();
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            OpenConnection();
        }

        private void playToolStripButton_Click(object sender, EventArgs e)
        {
            var child = this.ActiveMdiChild as MainForm;
            if (child != null)
            {
                child.IsRunning = true;
                EnableMenus();
            }
        }

        private void StopToolStripButton_Click(object sender, EventArgs e)
        {
            var child = this.ActiveMdiChild as MainForm;
            if (child != null)
            {
                child.IsRunning = false;
                EnableMenus();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var F = new SplashForm();
            F.ShowDialog();
            System.Windows.Forms.Application.DoEvents();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenConnection();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void findToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void clearToolStripButton_Click(object sender, EventArgs e)
        {
            var child = this.ActiveMdiChild as MainForm;
            if (child != null)
            {
                child.ClearAll();
            }
        }

        private void autoScrollToolStripButton_Click(object sender, EventArgs e)
        {
            autoScrollToolStripButton.Checked = !autoScrollToolStripButton.Checked;
            var child = this.ActiveMdiChild as MainForm;
            if (child != null)
            {
                child.AutoScroll = autoScrollToolStripButton.Checked;
            }
        }

    }
}