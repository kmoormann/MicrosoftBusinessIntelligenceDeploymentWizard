using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Pariveda.BI.Deployment.Library.Functional.Test
{
    [TestClass]
    public class BISolutionsParserUnitTests
    {
        [TestMethod]
        public void GetAllProjectsFromSolutionFile()
        {
            //Arrange
            var slnFile = new FileInfo(@"C:\ParivedaDW_Projects\ParivedaAWDW\ParivedaAWDW.sln");
            //Act
            var projectFiles = SolutionParser.GetProjectXmlDocs(slnFile.FullName, ".dtproj");
                //.ParseSolutionFile(slnFile.FullName,".dtproj");
            //Assert
            foreach (var item in projectFiles)
            {
                Console.WriteLine(item.InnerXml);//ToString());
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

    }
}
