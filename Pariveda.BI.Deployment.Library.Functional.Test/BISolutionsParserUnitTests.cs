using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace Pariveda.BI.Deployment.Library.Functional.Test
{
    [TestClass]
    public class BISolutionsParserUnitTests
    {
        [TestMethod]
        public void GetAllSSISProjectXMLFromSolutionFile()
        {
            //Arrange
            var slnFile = new FileInfo(@"C:\ParivedaDW_Projects\ParivedaAWDW\ParivedaAWDW.sln");
            //Act
            var projectFiles = SolutionParser.GetProjectXmlDocs(slnFile.FullName, ".dtproj");

            //Assert
            foreach (var item in projectFiles)
            {

            }
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void GetRelativePath()
        {
            //Arrange
            var slnFile = new FileInfo(@"C:\ParivedaDW_Projects\ParivedaAWDW\ParivedaAWDW.sln");
            var baseDirectory = slnFile.Directory;
            var relativePathDown = @"\Configurations\Common Connections.dtsConfig";
            //var relativePathUp = @"..\ParivedaAWCUBE\ParivedaAWCUBE\SSASUserLoginName.sql";

            //Act
            var fileDown = SolutionParser.GetXmlDoc(baseDirectory,relativePathDown);
            
            //Assert
            
            Console.WriteLine(fileDown.InnerXml);
        }

        private string GetQuoted(string stringToParse)
        {
            string pattern = @"([""'])(?:(?=(\\?))\2.)*?\1";
            return (new Regex(pattern)).Match(stringToParse).Value.Replace("\"", String.Empty);
        }
        [TestMethod]
        public void parsedatabaseProject()
        {
            string folderName = "";
            var scriptsList = new List<KeyValuePair<string, FileInfo>>();
            foreach (var line in System.IO.File.ReadAllLines(@"C:\DeploymentsTemp\ProjectFiles\db.dbp"))
            {
                if (line.Trim().StartsWith("Begin Folder")) 
                    folderName = GetQuoted(line.Trim());
                if (line.Trim().StartsWith("Script"))
                    scriptsList.Add(new KeyValuePair<string,FileInfo>(folderName, new FileInfo(GetQuoted(line.Trim()))));

            }

            foreach (var item in scriptsList)
            {
                Console.WriteLine(item.Key + "\t" + item.Value);
            }
            
        }

        [TestMethod]
        public void parseSSISProject()
        {
            foreach (var item in SSISProjectFileParser.ParseSSISProjectFile(@"C:\DeploymentsTemp\ProjectFiles\ssis.dtproj"))
                Assert.IsFalse(item.Exists);

        }

    }
}
