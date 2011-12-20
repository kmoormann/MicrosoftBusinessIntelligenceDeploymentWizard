using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Pariveda.BI.Deployment;
using System.IO;
using NUnit.Framework;
using ApprovalTests.Reporters;
using Pariveda.BI.Deployment.Library.Enums;

namespace Pariveda.BI.Deployment.Library.AcceptanceTests
{
    [Binding]
    public class US02_DeploySSIS : US00_BaselineSteps
    {

        [Given(@"I have a ticket number (.*)")]
        public void GivenIHaveATicketNumber(string ticketNumber)
        {
            _context.TicketNumber = UInt32.Parse(ticketNumber);
            Assert.Greater(_context.TicketNumber, 0);
        }

        [Given(@"I have selected the deployment path of (.*)")]
        public void GivenIHaveSelectedTheDeploymentPathOfCDeploymentsTemp(string destinationPath)
        {
            _context.DeploymentFolder = new DirectoryInfo(destinationPath);
            Assert.IsTrue(_context.DeploymentFolder.Exists);
        }

        [When(@"I press on the deploy button")]
        public void PressTheButton()
        {
            _context.Manifest.Deploy();
        }

        [Given(@"I have the following Business Intelligence Deployables")]
        public void GivenIHaveTheFollowingBIDeployables(Table table)
        {
            var items = new List<IBIDeployable>();
            foreach (var row in table.Rows)
            {
                switch (row["Type"])
                {
                    case "SSIS":
                        items.Add(new SSISItem(row["Path"], Boolean.Parse(row["Deploy"])));
                        break;
                    case "SQL":
                        items.Add(new SQLItem(row["Path"], Boolean.Parse(row["Deploy"])));
                        break;
                    default:
                        break;
                }
            }
            _context.Manifest = new Manifest(_context.TicketNumber, _context.DeploymentFolder);
            _context.Manifest.BIDeployables = items.AsEnumerable<IBIDeployable>();
        }

        [Then(@"the (.*) should be deployed to the (.*)\\(.*) folder")]
        public void ThenTheBIDeployablesShouldBeDeployedToTheSourceFolder(string ignoreString, string deployment, string biType)
        {
            BusinessIntelligenceItemType type = BusinessIntelligenceItemType.Unknown;
            switch (biType)
            {
                case "MSDB":
                    type = BusinessIntelligenceItemType.SSISPackage;
                    break;
                case "DB":
                case "SQL":
                    type = BusinessIntelligenceItemType.SQLFile;
                    break;
                default: 
                    break;
            }

            var sourceFolder = new DirectoryInfo(_context.Manifest.DeploymentFolder.FullName + "\\" + deployment + "\\" + biType);
            var files = sourceFolder.GetFiles().AsQueryable();
            var deployedItems = new List<BusinessIntelligenceItem>();
            foreach (IBIDeployable deployable in _context.Manifest.BIDeployables)
            {
                if ((null != (deployable as BusinessIntelligenceItem) && ((BusinessIntelligenceItem)deployable).ShouldDeploy))
                    deployedItems.Add((deployable as BusinessIntelligenceItem));
            }
            var filesFound = files.AsQueryable().Join(
                                     deployedItems //collection to join to
                                      , file => file.Name //left hand criteria
                                     , deployedItem => deployedItem.FileName //left hand criteria
                                    , (file, deployedItem) => new { file.Name, deployedItem.FileName } //result set
                                    );
            Assert.AreEqual(deployedItems.Where(x => x.BIItemType == type).Count()
                , filesFound.Count());
        }

        [Given(@"I have entered the ticket number (.*)")]
        public void GivenIHaveEnteredTheTicketNumber(string ticketNumber)
        {
            _context.TicketNumber = UInt32.Parse(ticketNumber);
            Assert.Greater(_context.TicketNumber, 0);
        }

        [Given(@"the project name is ""BI Project""")]
        public void GivenTheProjectName()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I selected a destination of (.*)")]
        public void GivenIHaveSelectedADestination(string destinationPath)
        {
            _context.DeploymentFolder = new DirectoryInfo(destinationPath);
            Assert.IsTrue(_context.DeploymentFolder.Exists);
        }


        [Then(@"a folder should be created")]
        public void ThenAFolderShouldBeCreated()
        {
            var folder = new DirectoryInfo(_context.Manifest.DeploymentFolder.FullName);
            Assert.IsTrue(_context.Manifest.DeploymentFolder.Exists, string.Format("The folder \r\n\t{0}\r\ndoes not exist", _context.Manifest.DeploymentFolder.FullName));
        }

        [Then(@"it should be named with the ticket number")]
        public void ThenItShouldBeNamedWithTheTicketNumber()
        {
            Assert.IsTrue(_context.Manifest.DeploymentFolder.FullName.Contains(_context.TicketNumber.ToString())
                , string.Format("The deployment folder {0}\r\ndoes not contain the name of the ticket{1}", _context.Manifest.DeploymentFolder.Name, _context.TicketNumber));
        }

        [Then(@"there should be a (.*) folder with a (.*) subfolder")]
        public void ThenThereShouldBeFolderWithAParticularSubFolder(string folderName, string subFolderName)
        {
            Exception ex = null;
            DirectoryInfo sourceDirectory = null, subFolder = null;
            try
            {
                var currentFolder = _context.Manifest.DeploymentFolder;
                if (folderName.Contains('\\'))
                {
                    var folderParts = folderName.Split('\\');
                    foreach (string f in folderParts)
                    {
                        TestForAFolder(currentFolder, f);
                        currentFolder = currentFolder.GetDirectories().Where(d => d.Name.Equals(f)).First();
                    }
                }
                else
                {
                    TestForAFolder(currentFolder, folderName);
                    currentFolder = currentFolder.GetDirectories().Where(d => d.Name.Equals(folderName)).First();
                }
                TestForAFolder(currentFolder, subFolderName);
            }
            catch(Exception e)
            {
                ex = e;
                Assert.IsNull(ex,
                    string.Format("Exception during processing of {0} with subfolder of {1}\r\nException message:\r\n\t{2}"
                                    , folderName, subFolderName, ex.Message));
                
            }
            
            
        }

        private void TestForAFolder(DirectoryInfo baseDirectory, string subFolderName)
        {
            if (String.IsNullOrEmpty(subFolderName))
            {
                AssertAFolderExists(baseDirectory);
            }

            var subFolder = baseDirectory.GetDirectories().Where(d => subFolderName.Equals(d.Name)).First();
            AssertAFolderExists(subFolder);
        }

        private void AssertAFolderExists(DirectoryInfo directory)
        {
            Assert.IsTrue(directory.Exists,
                        string.Format("Directory: {0} \t does not exist at {1}",
                                        directory.Name, directory.FullName));
        }

        [Then(@"there should be a (.*) folder")]
        public void ThenThereShouldBeFolder(string folderName)
        {
            var sourceDirectory = _context.Manifest.DeploymentFolder.GetDirectories().Where(d => folderName.Equals(d.Name)).First();
            Assert.IsTrue(sourceDirectory.Exists);
        }


        [Then(@"the (.*) file should be in the (.*) folder")]
        public void ThenTheDeploy_BatFileShouldBeInTheDeployScriptsFolder(string fileName, string folderName)
        {
            DirectoryInfo sourceDirectory;
            if ("Deployment".Equals(folderName, StringComparison.InvariantCultureIgnoreCase))
                sourceDirectory = _context.Manifest.DeploymentFolder;
            else
                sourceDirectory = _context.Manifest.DeploymentFolder.GetDirectories()
                .Where(d => d.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            Assert.IsNotNull(sourceDirectory,
                string.Format("directory:\n\r\t {0}\r\nnot found at\r\n\t{1}",
                folderName,_context.Manifest.DeploymentFolder.FullName));
            var sourceFile = sourceDirectory.GetFiles()
                .Where(f => f.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            Assert.IsNotNull(sourceFile,
                 string.Format("file:\n\r\t {0}\r\nnot found at\r\n\t{1}\\{2}",
                 fileName, _context.Manifest.DeploymentFolder.FullName,folderName));
            Assert.IsTrue(sourceFile.Exists);   
        }

        [Then(@"the deploymentVariables.ps1 file should be created in the DeploymentScripts folder")]
        public void ThenCreateTheDeploymentVariablesPs1File()
        {
            FileInfo deploymentVariablesPS1 = 
                    _context.Manifest.DeploymentFolder.GetDirectories()
                    .Where(d => "DeployScripts".Equals(d.Name))
                    .First().GetFiles().
                    Where(f => "DeploymentVariables.ps1".Equals(f.Name))
                    .First();

            Assert.IsTrue(deploymentVariablesPS1.Exists);
            _context.DeploymentVariablesPS = deploymentVariablesPS1;
        }

        [Then(@"it should have the correct (.*) (.*)")]
        [UseReporter(typeof(DiffReporter))]
        public void ThenItShouldHaveTheCorrectFileSetup(string name, string varOrHash)
        {
            ApprovalTests.Approvals.ApproveFile(_context.DeploymentVariablesPS.FullName);
        }

    }

}
