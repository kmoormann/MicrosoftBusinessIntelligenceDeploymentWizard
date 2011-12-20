// Learn more about F# at http://fsharp.net


#light

namespace Pariveda.BI.Deployment.Library
open System
open System.IO
open System.Xml

    module SolutionParser =

        let ParseSolutionFile a extension = 
            let parsed = System.IO.File.ReadLines(a)
                         |> Seq.filter (fun l -> l.StartsWith("Project"))
                         |> Seq.map (fun l -> l.Split(',')|>Array.toList)
                         |> Seq.map (fun l -> l.Item(1))
                         |> Seq.map (fun l -> l.Replace("\"",""))
                         |> Seq.filter (fun l -> l.EndsWith(extension))
                         |> Seq.map (fun l -> l.Trim())
            parsed

        let GetXmlDoc (basis: System.IO.DirectoryInfo, relativePath: string) =
          let path = basis.FullName + (if relativePath.StartsWith("\\") then relativePath else "\\" + relativePath)
          let doc = new XmlDocument()
          do doc.Load(path)
          doc

        let GetProjectXmlDocs a extension =
            let baseDirectory = (new FileInfo(a)).Directory
            seq {for x in (ParseSolutionFile a extension) do
                     let path = String.Concat(baseDirectory.FullName,(if x.StartsWith("\\") then x else "\\" + x))
                     let doc = new XmlDocument()
                     do doc.Load(path)
                     yield doc
                     //yield path
                }
                             
            
           