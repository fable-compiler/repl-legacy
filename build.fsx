// include Fake libs
#r "./packages/build/FAKE/tools/FakeLib.dll"
#r "System.IO.Compression.FileSystem"

open System
open System.IO
open Fake
open Fake.ReleaseNotesHelper
open Fake.Git
open Fake.Testing.Expecto
open Fake.YarnHelper

let dotnetcliVersion = "2.0.0"

let mutable dotnetExePath = "dotnet"
let runDotnet dir =
    DotNetCli.RunCommand (fun p -> { p with ToolPath = dotnetExePath
                                            WorkingDir = dir
                                            TimeOut =  TimeSpan.FromHours 12. } )
                                            // Extra timeout allow us to run watch mode
                                            // Otherwise, the process is stopped every 30 minutes by default

let runScript workingDir (fileName: string) args =
    printfn "CWD: %s" workingDir
    let fileName, args =
        if EnvironmentHelper.isUnix then 
            let fileName = fileName.Replace("\\","/")
            "bash", (fileName + ".sh " + args)
        else 
            "cmd", ("/C " + fileName + " " + args)
    let ok =
        execProcess (fun info ->
            info.FileName <- fileName
            info.WorkingDirectory <- workingDir
            info.Arguments <- args) TimeSpan.MaxValue
    if not ok then failwith (sprintf "'%s> %s %s' task failed" workingDir fileName args)

let currentDir = __SOURCE_DIRECTORY__
let sourceDir = currentDir </> "src"
let rootDir = currentDir </> ".."
let FCSFableFolderName = "FSharp.Compiler.Service_fable"
let FCSExportFolderName = "FSharp.Compiler.Service_export"
let FableFolderName = "Fable"

let FCSFableFolderPath = rootDir </> FCSFableFolderName
let FCSExportFolderPath = rootDir </> FCSExportFolderName
let FableFolderPath = rootDir </> FableFolderName

let rec waitUserResponse _ =
    let userInput = Console.ReadLine()
    match userInput.ToUpper() with
    | "O" -> true
    | "N" -> false
    | _ ->
        printfn "Invalid response"
        waitUserResponse ()

type RepoSetupInfo =
    { FolderPath : string
      FolderName : string
      GithubLink : string
      GithubBranch : string }

let ensureRepoSetup (info : RepoSetupInfo) =
    // Use getBuildParamOrDefault to force Y on CI server
    // See: http://fake.build/apidocs/fake-environmenthelper.html
    // and: https://stackoverflow.com/questions/26267601/can-i-pass-a-parameter-to-a-f-fake-build-script
    if not (Directory.Exists(info.FolderPath)) then
        printfn "Can't find %s at: %s" info.FolderName rootDir
        let setupMode = getBuildParamOrDefault "setup" "ask"

        if setupMode = "ask" then
            printfn "Do you want me to setup it for you ? (O/N)"
            let autoSetup = waitUserResponse ()
            if autoSetup then
                printfn "Installing %s for you" info.FolderName
                Repository.clone rootDir info.GithubLink info.FolderName
                runSimpleGitCommand info.FolderPath ("checkout " + info.GithubBranch) |> ignore
            else
                failwithf "You need to setup the %s project at %s yourself so." info.FolderName rootDir
        else
            printfn "You started with auto setup mode. Installing %s for you" info.FolderName            
            Repository.clone rootDir info.GithubLink info.FolderName
            runSimpleGitCommand info.FolderPath ("checkout " + info.GithubBranch) |> ignore
    else
        printfn "Directory %s found" info.FolderName

Target "Setup" (fun _ ->
    ensureRepoSetup
        { FolderPath = FCSFableFolderPath
          FolderName = FCSFableFolderName
          GithubLink = "git@github.com:ncave/FSharp.Compiler.Service.git"
          GithubBranch = "fable" }

    ensureRepoSetup
        { FolderPath = FableFolderPath
          FolderName = FableFolderName
          GithubLink = "git@github.com:fable-compiler/Fable.git"
          GithubBranch = "master" }
)

Target "Build.FCS_Fable" (fun _ ->
    runScript FCSFableFolderPath "fcs\\build" "CodeGen.Fable"
)

Target "Build.FCS_Export" (fun _ ->
    ensureRepoSetup
        { FolderPath = FCSExportFolderPath
          FolderName = FCSExportFolderName
          GithubLink = "git@github.com:ncave/FSharp.Compiler.Service.git"
          GithubBranch = "export" }

    runScript FCSExportFolderPath "fcs\\build" "Export.Metadata"
)

Target "Generate.Metadata" (fun _ ->
    let destination = currentDir </> "public" </> "metadata2"
    CleanDir destination
    CopyDir destination (FCSExportFolderPath </> "temp" </> "metadata2") (fun _ -> true)
    !! (destination </> "*.dll")
    |> Seq.iter(fun filename ->
        Rename (filename + ".txt") filename
    ) 
)

Target "Build.Fable" (fun _ ->
    runScript FableFolderPath "build" "FableCoreJS"
)

Target "Build.REPL" (fun _ ->
    runDotnet sourceDir "fable npm-rollup --port free --verbose"
)

Target "Build.REPL.Quick" (fun _ ->
    runDotnet sourceDir "fable npm-rollup --port free --verbose"
)


Target "InstallDotNetCore" (fun _ ->
   dotnetExePath <- DotNetCli.InstallDotNetSDK dotnetcliVersion
)

Target "Clean" (fun _ ->
    !! "public/metadata2"
    ++ "public/js"
  |> CleanDirs
)

// Dependencies

Target "Restore" (fun _ ->
    runDotnet currentDir "restore Fable.REPL.sln"
)

Target "YarnInstall" (fun _ ->
    Yarn (fun p ->
            { p with
                Command = Install Standard
            })
)

Target "Watch.App" (fun _ ->
    runDotnet sourceDir "fable yarn-start-app --port free"
)

// Client
Target "BuildClient" (fun _ ->
    runDotnet "Client" """fable yarn-build:client --port 3003"""
)

Target "WatchClient" (fun _ ->
    runDotnet "Client" """fable yarn-watch:client --port 3003"""
)

Target "All" DoNothing

// Build order
"Setup"
    ==> "Build.FCS_Fable"
    ==> "Build.Fable"
    ==> "Clean"
    ==> "InstallDotNetCore"
    ==> "Restore"
    ==> "YarnInstall"
    ==> "Build.REPL"
    ==> "All"

"Build.FCS_Export"
    // ==> "Generate.Metadata"

// start build
RunTargetOrDefault "YarnInstall"
