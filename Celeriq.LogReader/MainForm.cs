using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Celeriq.LogReader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            txtFilter.TextChanged += txtFilter_TextChanged;

            var width = lvwFile.Width - 40;

            lvwSummary.Columns.Clear();
            lvwSummary.Columns.Add("Name", width / 2);
            lvwSummary.Columns.Add("Value", width / 2);

            lvwFile.Columns.Clear();
            lvwFile.Columns.Add("Log file", width);
            lvwFile.ItemSelectionChanged += lvwFile_ItemSelectionChanged;

            lvwLog.Columns.Clear();
            lvwLog.Columns.Add("Log time", width / 5);
            lvwLog.Columns.Add("Source", width / 5);
            lvwLog.Columns.Add("Item count", width / 5);
            lvwLog.Columns.Add("Time (ms)", width / 5);
            lvwLog.Columns.Add("Cache hit", width / 5);
            lvwLog.Columns.Add("Url", width / 5);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (lvwFile.SelectedItems.Count == 0) return;
            ProcessSingleFile(lvwFile.SelectedItems[0].Tag as string);
        }

        private void lvwFile_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.ProcessSingleFile(e.Item.Tag as string);
        }

        private void cmdFile_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
                this.LoadLogs(txtFolder.Text);
            }
        }

        private void LoadLogs(string folderName)
        {
            var files = System.IO.Directory.GetFiles(folderName, "*.txt").OrderBy(x => x);
            lvwFile.Items.Clear();
            lvwLog.Items.Clear();
            lvwSummary.Items.Clear();

            foreach (var s in files)
            {
                var fi = new FileInfo(s);
                DateTime dt;
                if (DateTime.TryParseExact(fi.Name.Replace(fi.Extension, string.Empty), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    var newItem = lvwFile.Items.Add(fi.Name.Replace(".txt", string.Empty));
                    newItem.Tag = fi.FullName;
                }
            }
        }

        private void ProcessSingleFile(string fileName)
        {
            var urlList = new Dictionary<string, int>();
            long totalTime = 0;
            long cacheHits = 0;
            long totalHits = 0;
            double totalResults = 0;

            lvwLog.Items.Clear();
            using (var sr = File.OpenText(fileName))
            {
                var text = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    if (text.Contains("| Query:"))
                    {
                        var elapsed = GetElapsed(text);
                    }
                    else if (text.Contains("| FlushCache ("))
                    {

                    }
                    else if (text.Contains("| UpdateIndexList:"))
                    {

                    }
                    text = sr.ReadLine();
                }
            }

            #region Summary

            lvwSummary.Items.Clear();

            #endregion
        }

        private int GetElapsed(string t)
        {
            const string TargetText="Elapsed=";
            if (string.IsNullOrEmpty(t)) return 0;
            var index = t.IndexOf(TargetText);
            if (index == -1) return 0;
            t = t.Substring(index + TargetText.Length);
            return 0;
        }

        private void AddSummaryItem(string name, string value)
        {
            var newItem = new ListViewItem() { Text = name };
            newItem.SubItems.Add(value);
            lvwSummary.Items.Add(newItem);
        }

        private void AddLogEntry(string logTime, string ip, long resultCount, int time, bool isCache, string url)
        {
            var newItem = lvwLog.Items.Add(logTime);
            newItem.SubItems.Add(ip);
            newItem.SubItems.Add(resultCount.ToString("###,###,###,##0"));
            newItem.SubItems.Add(time.ToString("###,###,###,##0"));
            newItem.SubItems.Add(isCache.ToString().ToLower());
            newItem.SubItems.Add(url);
        }
    }
}