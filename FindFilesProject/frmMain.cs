using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FindFilesProject.Classes;

namespace FindFilesProject
{
    public partial class frmMain : Form, Observer
    {
        Controller controller;

        public frmMain()
        {
            InitializeComponent();
            lblStatus.Text = "";
            controller = new Controller();
            controller.Subscribe(this);
        }

        
        private void btnInput_Click(object sender, EventArgs e)
        {
            if (fbdSelectPath.ShowDialog() == DialogResult.OK)
            {
                txtInputPath.Text = fbdSelectPath.SelectedPath;
                controller.InputPath = fbdSelectPath.SelectedPath;
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            if (fbdSelectPath.ShowDialog() == DialogResult.OK)
            {
                txtOutputPath.Text = fbdSelectPath.SelectedPath;
                controller.OutputPath = fbdSelectPath.SelectedPath;
            }
        }

        public void Update(File file)
        {
            string[] row = { file.Drive, file.FilePath, file.FileName, file.FileSize.ToString() + " MB" };
            ListViewItem listViewItem = new ListViewItem(row);
            if (lsvOutput.InvokeRequired)
                lsvOutput.Invoke(new Action(() => lsvOutput.Items.Add(listViewItem)));
            else
                lsvOutput.Items.Add(listViewItem);
        }

        public void UpdateStarted()
        {
            lblStatus.Text = "Searching for files...";
            this.Cursor = Cursors.WaitCursor;
        }

        public void UpdateFinished()
        {
            lblStatus.Text = "Finished searching for files";
            MessageBox.Show("Finished!");
            if (this.InvokeRequired)
                this.Invoke(new Action(() => this.Cursor = Cursors.Default));
            else
                this.Cursor = Cursors.Default;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInputPath.Text) && !string.IsNullOrEmpty(txtOutputPath.Text))
            {
                controller.InputPath = txtInputPath.Text;
                controller.OutputPath = txtOutputPath.Text;
                controller.Start();
            }
            else
            {
                MessageBox.Show("Gelieve een pad in te vullen bij input en output!");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            controller.Stop();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lsvOutput.Clear();
            txtInputPath.Text = "";
            txtOutputPath.Text = "";
        }
    }
}
