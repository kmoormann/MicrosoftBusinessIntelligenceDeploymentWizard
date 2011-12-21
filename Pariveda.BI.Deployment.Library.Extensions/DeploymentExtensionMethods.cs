using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Pariveda.BI.Deployment.Library.Extensions
{
    public static class DeploymentExtensionMethods
    {
        public static DirectoryInfo CopyFilesWithExtension(this DirectoryInfo source, string extension, DirectoryInfo destination, Boolean overwrite)
        {
            if (!extension.StartsWith(".")) extension = string.Format(".{0}", extension);
            foreach (FileInfo fInfo in source.
                                        GetFiles().
                                        Where(f => f.Extension.EndsWith(extension)))
            {
                fInfo.CopyTo(string.Format("{0}\\{1}", destination.FullName, fInfo.Name), overwrite);
            }
            return source;
        }

        public static IEnumerable<FileInfo> ParseBISolutionFile(this FileInfo source)
        {
            var biFiles = new List<FileInfo>();
            biFiles.AddRange(source.GetSSISPackages());
            biFiles.AddRange(source.GetSqlScripts());
            return biFiles.AsEnumerable();
        }

        public static string GetQuoted(this string stringToParse)
        {
            string pattern = @"([""'])(?:(?=(\\?))\2.)*?\1";
            return (new Regex(pattern)).Match(stringToParse).Value.Replace("\"", String.Empty);
        }

        public static IEnumerable<FileInfo> GetSqlScripts(this FileInfo source)
        {
            var ssisFiles = new List<FileInfo>();
            foreach (var item in SolutionParser.GetProjectXmlDocPaths(source.FullName, ".dtproj"))
            {
                ssisFiles.AddRange(SSISProjectFileParser.GetSQLScripts(item));
            }
            return ssisFiles.AsEnumerable();
        }
        //public static IEnumerable<FileInfo> GetSQLScripts(this FileInfo source)
        //{
   

        //[TestMethod]
        //public void parsedatabaseProject()
        //{
        //    string folderName = "";
        //    var scriptsList = new List<KeyValuePair<string, FileInfo>>();
        //    foreach (var line in System.IO.File.ReadAllLines(@"C:\DeploymentsTemp\ProjectFiles\db.dbp"))
        //    {
        //        if (line.Trim().StartsWith("Begin Folder")) 
        //            folderName = GetQuoted(line.Trim());
        //        if (line.Trim().StartsWith("Script"))
        //            scriptsList.Add(new KeyValuePair<string,FileInfo>(folderName, new FileInfo(GetQuoted(line.Trim()))));

        //    }

        //    foreach (var item in scriptsList)
        //    {
        //        Console.WriteLine(item.Key + "\t" + item.Value);
        //    }
            
        //}
            
        //}
        
        
        public static IEnumerable<FileInfo> GetSSISPackages(this FileInfo source)
        {
            var ssisFiles = new List<FileInfo>();
            foreach (var item in SolutionParser.GetProjectXmlDocPaths(source.FullName, ".dtproj"))
            {
                ssisFiles.AddRange(SSISProjectFileParser.GetSSISPackages(item));
            }
            return ssisFiles.AsEnumerable();
        }
    }
}
