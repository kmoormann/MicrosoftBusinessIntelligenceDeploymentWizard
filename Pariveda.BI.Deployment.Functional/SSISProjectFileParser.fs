

namespace Pariveda.BI.Deployment.Library
open System
open System.IO
open System.Xml


    module SSISProjectFileParser =

        let ParseSSISProjectFile projFilePath =
            let projFile = new FileInfo(projFilePath)
            let doc = new XmlDocument()
            do doc.Load(projFile.FullName)
            seq {for path in doc.SelectNodes("/Project/DTSPackages/DtsPackage/Name")
                    -> new FileInfo(projFile.Directory.FullName + "\\" + path.InnerText) }

