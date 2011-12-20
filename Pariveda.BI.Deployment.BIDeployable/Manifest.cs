using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Pariveda.BI.Deployment.Library.Extensions;
using Pariveda.BI.Deployment.Library.Enums;
using Pariveda.BI.Deployment.Library.Properties;


namespace Pariveda.BI.Deployment.Library
{

    public class Manifest
    {
        private readonly DirectoryInfo _deploymentFolderRoot;
        public DirectoryInfo DeploymentFolder { get; set; }

        public uint TicketNumber { get; set; }

        public IEnumerable<IBIDeployable> BIDeployables { get; set; }

        public Manifest(uint ticketNumber, DirectoryInfo deploymentFolderRoot)
        {
            TicketNumber = ticketNumber;
            _deploymentFolderRoot = deploymentFolderRoot;
        }

        public void Deploy()
        {
            CreateDeploymentFolder(_deploymentFolderRoot);
            InitializeSourceFolder();
            InitializeRollbackFolder();
            InitializeDeployScriptsFolder();
            DeployBIItems();

        }

        private void InitializeDeployScriptsFolder()
        {
            InitializeSourceOrRollbackFolder(DeploymentFolderType.DeployScripts);
            GetPowerShellScripts();
            GetBatchFiles();
        }

        private void GetBatchFiles()
        {
            var deployScriptsFolder = this[DeploymentFolderType.DeployScripts];
            var psFilesFolder = new DirectoryInfo(Settings.Default.PowerShellDeployScriptsPath);
            psFilesFolder.Parent.CopyFilesWithExtension("bat", this[DeploymentFolderType.DeployScripts].Parent, true);
        }



        private void GetPowerShellScripts()
        {
            var deployScriptsFolder = this[DeploymentFolderType.DeployScripts];
            var psFilesFolder = new DirectoryInfo(Settings.Default.PowerShellDeployScriptsPath);
            psFilesFolder.CopyFilesWithExtension("ps1", this[DeploymentFolderType.DeployScripts], true);
        }

        private DirectoryInfo this[DeploymentFolderType folderType]
        {
            get
            {
                return this.DeploymentFolder.GetDirectories()
                    .Where(d =>
                                d.Name.Equals(folderType.ToString()
                                    , StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
            }
        }

        private void DeployBIItems()
        {
            if (null != BIDeployables && 0 < BIDeployables.Count())
            {

                foreach (IBIDeployable item in BIDeployables)
                {
                    BusinessIntelligenceItem biItem = item as BusinessIntelligenceItem;
                    SourceRollbackFolderType sourceFolder = SourceRollbackFolderType.Unknown;
                    switch (biItem.BIItemType)
                    {
                        case BusinessIntelligenceItemType.FlatFile:
                            sourceFolder = SourceRollbackFolderType.File;
                            break;
                        case BusinessIntelligenceItemType.SQLFile:
                            sourceFolder = SourceRollbackFolderType.DB;
                            break;
                        case BusinessIntelligenceItemType.SSISPackage:
                            sourceFolder = SourceRollbackFolderType.MSDB;
                            break;
                        case BusinessIntelligenceItemType.SSRSRdl:
                            sourceFolder = SourceRollbackFolderType.SSRS;
                            break;
                        //case Pariveda.BI.Deployment.BusinessIntelligenceItemType.Unknown:
                        default:
                            throw new InvalidDataException("A business intelligence item type of unknown was attempted to be deployed.");
                    }

                    item.Deploy(new DirectoryInfo(string.Format("{0}\\{1}\\{2}",
                        DeploymentFolder.FullName, DeploymentFolderType.Source, sourceFolder)));
                }

            }
        }

        private void InitializeSourceFolder()
        {
            InitializeSourceOrRollbackFolder(DeploymentFolderType.Source);
        }

        private void InitializeRollbackFolder()
        {
            InitializeSourceOrRollbackFolder(DeploymentFolderType.Rollback);
        }

        private void InitializeSourceOrRollbackFolder(DeploymentFolderType folderType)
        {
            if (DeploymentFolderType.Unknown == folderType)
                throw new ArgumentException("folderType should not be unknown.");
            var folder = DeploymentFolder.CreateSubdirectory(folderType.ToString());
            switch (folderType)
            {

                case DeploymentFolderType.Rollback:
                case DeploymentFolderType.Source:
                    CreateSourceRollBackStructure(folder);
                    break;
                case DeploymentFolderType.DeployScripts:
                    break;

            }

        }

        private static void CreateSourceRollBackStructure(DirectoryInfo folder)
        {
            foreach (string name in Enum.GetNames(typeof(SourceRollbackFolderType)).Where(s => !"Unknown".Equals(s)))
            {
                var root = folder.CreateSubdirectory(name);
                var sourceRollbackFolder = (SourceRollbackFolderType)Enum.Parse(typeof(SourceRollbackFolderType), name);
                if (sourceRollbackFolder == SourceRollbackFolderType.DB)
                {
                    foreach (string subName in Enum.GetNames(typeof(ServerTypes)).Where(s => !"Unknown".Equals(s)))
                    {
                        root.CreateSubdirectory(subName);
                    }
                }
            }
        }

        private void CreateDeploymentFolder(DirectoryInfo root)
        {
            var deploymentFolder = new DirectoryInfo(GetDeploymentFolderName(root.FullName));
            if (deploymentFolder.Exists)
            {
                deploymentFolder.Delete(true);
            }
            deploymentFolder.Create();
            DeploymentFolder = deploymentFolder;
        }

        private string GetDeploymentFolderName(string rootPath)
        {
            return string.Format("{0}\\{1}", rootPath, GetDeploymentFolderName());
        }

        private string GetDeploymentFolderName()
        {
            return string.Format("Ticket{0}", TicketNumber);
        }
    }

}