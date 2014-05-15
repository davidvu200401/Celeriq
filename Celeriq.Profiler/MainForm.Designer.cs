namespace Celeriq.Profiler
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolConnection = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolLine = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolRows = new System.Windows.Forms.ToolStripStatusLabel();
			this.lvwItem = new System.Windows.Forms.ListView();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStatus,
            this.toolConnection,
            this.toolLine,
            this.toolRows});
			this.statusStrip1.Location = new System.Drawing.Point(0, 456);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(926, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStatus
			// 
			this.toolStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.toolStatus.Name = "toolStatus";
			this.toolStatus.Size = new System.Drawing.Size(651, 17);
			this.toolStatus.Spring = true;
			this.toolStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolConnection
			// 
			this.toolConnection.AutoSize = false;
			this.toolConnection.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolConnection.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.toolConnection.Name = "toolConnection";
			this.toolConnection.Size = new System.Drawing.Size(100, 17);
			this.toolConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolLine
			// 
			this.toolLine.AutoSize = false;
			this.toolLine.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolLine.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.toolLine.Name = "toolLine";
			this.toolLine.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this.toolLine.Size = new System.Drawing.Size(80, 17);
			this.toolLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolRows
			// 
			this.toolRows.AutoSize = false;
			this.toolRows.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolRows.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.toolRows.Name = "toolRows";
			this.toolRows.Size = new System.Drawing.Size(80, 17);
			this.toolRows.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lvwItem
			// 
			this.lvwItem.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwItem.FullRowSelect = true;
			this.lvwItem.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwItem.HideSelection = false;
			this.lvwItem.Location = new System.Drawing.Point(0, 0);
			this.lvwItem.MultiSelect = false;
			this.lvwItem.Name = "lvwItem";
			this.lvwItem.Size = new System.Drawing.Size(926, 456);
			this.lvwItem.TabIndex = 1;
			this.lvwItem.UseCompatibleStateImageBehavior = false;
			this.lvwItem.View = System.Windows.Forms.View.Details;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(926, 478);
			this.Controls.Add(this.lvwItem);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Celeriq Profiler";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ListView lvwItem;
		private System.Windows.Forms.ToolStripStatusLabel toolStatus;
		private System.Windows.Forms.ToolStripStatusLabel toolLine;
		private System.Windows.Forms.ToolStripStatusLabel toolRows;
		private System.Windows.Forms.ToolStripStatusLabel toolConnection;

	}
}

