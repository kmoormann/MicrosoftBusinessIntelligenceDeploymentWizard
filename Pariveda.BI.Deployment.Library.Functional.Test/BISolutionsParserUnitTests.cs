using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Pariveda.BI.Deployment.Library.Extensions;

namespace Pariveda.BI.Deployment.Library.Functional.Test
{
    [TestClass]
    public class BISolutionsParserUnitTests
    {
        [TestMethod]
        public void GetAllSSISProjectXMLFromSolutionFile()
        {
            //Arrange
            var biFiles = (new FileInfo(@"C:\ParivedaDW_Projects\ParivedaAWDW\ParivedaAWDW.sln")).ParseBISolutionFile();
            var biExtensions = new List<string>{".dtsx", ".sql", ".rdl"};
            //Act
            int count = biFiles.Count();
            int biFileCount = biFiles.Where(f => biExtensions.Contains(f.Extension)).Count();

            //Assert
            Assert.AreEqual(count, biFileCount);// AreSame(count, biFileCount);
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
            //foreach (var item in SSISProjectFileParser.ParseSSISProjectFile(@"C:\DeploymentsTemp\ProjectFiles\ssis.dtproj"))
                //Assert.IsFalse(item.Exists);

        }

    }
}
