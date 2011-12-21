// This file is a script that can be executed with the F# Interactive.  
// It can be used to explore and test the library project.
// Note that script files will not be part of the project build.

#load "BISolutionParser.fs"
#load "SSISProjectFileParser.fs"
open Pariveda.BI.Deployment.Library

let testSln = @"C:\ParivedaDW_Projects\ParivedaAWDW\ParivedaAWDW\ParivedaAWDW.dtproj";

let sqlTest = SSISProjectFileParser.GetSQLScripts(testSln)

//let ParseProjectFile fileName =
//    let file = new FileInfo(fileName)
//    match (file.Extension) with
//        |".dbp"-> "database"
//        |".dtproj" -> "integration"
//        |".dwproj" -> "analysis"
//        |".rptproj" -> "reports"
//        |_ -> "not supported"
//
//let GetQuoted a =
//    let pattern = @"([""'])(?:(?=(\\?))\2.)*?\1"
//    let quoted = (new Regex(pattern)).Match(a).Value
//    quoted.Replace("\"","")
//
//let ParseDatabaseProjectFile file =
//    let lines = System.IO.File.ReadAllLines(file)|>Seq.map (fun x -> x.Trim())
//                |>List.p
//    let mutable scripts = List.empty<string>
//    let mutable folderName = "none"
//    for line in lines do
//            folderName <- if line.StartsWith("Begin Folder") then GetQuoted line else folderName
//            scripts <- if line.StartsWith("Script") then List.append scripts (GetQuoted line) else scripts
//    folderName
//    
    