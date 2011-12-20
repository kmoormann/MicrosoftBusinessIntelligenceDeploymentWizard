﻿using System;
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


        private static List<BusinessIntelligenceItem> CreateBIItemsListingFromDirectory(DirectoryInfo directoryInfo, string extension)
        {
            List<BusinessIntelligenceItem> items = new List<BusinessIntelligenceItem>();
            foreach (var directory in directoryInfo.GetDirectories())
                items.AddRange(CreateBIItemsListingFromDirectory(directory, extension).AsEnumerable());
            foreach (var file in directoryInfo.GetFiles())
                if (extension.Equals(file.Extension, StringComparison.InvariantCultureIgnoreCase) && !items.Any(x => x.FileName.EndsWith(file.Name)))
                {
                    switch(extension)
                    {
                        case ".dtsx":
                            items.Add(new SSISItem(file.FullName));
                            break;
                        case ".sql":
                            items.Add(new SQLItem(file.FullName));
                            break;
                        default:
                            break;
                    }
                    
                }
            return items;
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
            var info = new FileInfo(slnFile).Directory;
            InitializeSSISList(info);
            InitializeSQLLists(info);
        }

        private void InitializeSQLLists(DirectoryInfo info)
        {
            SQLCommitItems.Clear();
            foreach (var item in CreateBIItemsListingFromDirectory(info, ".sql"))
            {
                SQLCommitItems.Add((SQLItem)item);
            }
        }

        private void InitializeSSISList(DirectoryInfo root)
        {
            SSISItems.Clear();
            foreach (var item in CreateBIItemsListingFromDirectory(root, ".dtsx"))
            {
                SSISItems.Add((SSISItem)item);
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            //deploy to the destination
        }

        private void sqlDataGridCommit_CurrentChanged(object sender, EventArgs e)
        {

        }

   
    }
}
