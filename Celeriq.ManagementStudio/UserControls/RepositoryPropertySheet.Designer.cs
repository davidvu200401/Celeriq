namespace Celeriq.ManagementStudio.UserControls
{
	partial class RepositoryPropertySheet
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
			this.lvwItem = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// lvwItem
			// 
			this.lvwItem.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvwItem.FullRowSelect = true;
			this.lvwItem.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwItem.HideSelection = false;
			this.lvwItem.Location = new System.Drawing.Point(0, 0);
			this.lvwItem.Name = "lvwItem";
			this.lvwItem.Size = new System.Drawing.Size(573, 184);
			this.lvwItem.TabIndex = 0;
			this.lvwItem.UseCompatibleStateImageBehavior = false;
			this.lvwItem.View = System.Windows.Forms.View.Details;
			// 
			// RepositoryPropertySheet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lvwItem);
			this.Name = "RepositoryPropertySheet";
			this.Size = new System.Drawing.Size(573, 184);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvwItem;

	}
}
