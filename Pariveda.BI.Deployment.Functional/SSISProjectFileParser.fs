

namespace Pariveda.BI.Deployment.Library
open System
open System.IO
open System.Xml


    module SSISProjectFileParser =

        let private GetFilePaths (projFilePath, xpath) =
            let projFile = new FileInfo(projFilePath)
            let doc = new XmlDocument()
            do doc.Load(projFile.FullName)
            seq {for path in doc.SelectNodes(xpath)
                    -> projFile.Directory.FullName + "\\" + path.InnerText }

        let private CorrectExtension (info: FileInfo, ext:string) = 
            ext.Equals(info.Extension)

        let GetSSISPackages projFilePath =
            let xpath = "/Project/DTSPackages/DtsPackage/Name"
            let ssisPackages = GetFilePaths (projFilePath, xpath)
                                |>Seq.filter (fun l -> l.EndsWith(".dtsx"))
                                |>Seq.map (fun l -> new FileInfo(l))
            ssisPackages


        let GetSQLScripts projFilePath =
            let xpath = "/Project/Miscellaneous/ProjectItem/Name"
            let sqlScripts = GetFilePaths (projFilePath, xpath)
                                |>Seq.filter (fun l -> l.EndsWith(".sql"))
                                |>Seq.map (fun l -> new FileInfo(l))
            sqlScripts 