using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pariveda.BI.Deployment;
using System.IO;
using Pariveda.BI.Deployment.Library;
using Pariveda.BI.Deployment.Library.Extensions;

namespace Pariveda.BI.DeploymentWizard
{

    public partial class BIDeploymentForm : Form
    {
        BindingList<SSISItem> SSISItems;
        BindingList<SQLItem> SQLCommitItems;
        BindingList<SQLItem> SQLRollbackItems;

        public BIDeploymentForm()
        {
            InitializeComponent();
            SSISItems = new BindingList<SSISItem>();
            SQLCommitItems = new BindingList<SQLItem>();
            SQLRollbackItems = new BindingList<SQLItem>();
        }

        protected override void OnLoad(EventArgs e)
        {
            PopulateBIItemGrids();
            base.OnLoad(e);
        }

        private void PopulateBIItemGrids()
        {
            dataGridView1.DataSource = SSISItems;
            sqlDataGridCommit.DataSource = SQLCommitItems;
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var selected = openFileDialog1.FileName;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && !selected.Equals(openFileDialog1.FileName))
            {
                selected = openFileDialog1.FileName;
                label1.Text = selected;
                InitializeBILists(selected);
                PopulateBIItemGrids();
            }
        }

        private void InitializeBILists(string slnFile)
        {
            var biFiles = new FileInfo(slnFile).ParseBISolutionFile();
            InitializeSSISList(biFiles);
            InitializeSQLLists(slnFile);
        }

        private void InitializeSQLLists(string slnFile)
        {
            SQLCommitItems.Clear();
            foreach (var item in (new FileInfo(slnFile)).GetSqlScripts())
                SQLCommitItems.Add(item);
        }

        private void InitializeSSISList(IEnumerable<FileInfo> biFiles)
        {
            SSISItems.Clear();
            foreach (var item in biFiles)
            {
                SSISItems.Add(new SSISItem(item.FullName,true));
            }
        }   
    }
}
