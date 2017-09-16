module Fable.FCS

open System
open System.Collections.Generic
open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices

type Checker = InteractiveChecker
type Results = FSharpParseFileResults * FSharpCheckFileResults * FSharpCheckProjectResults

let createChecker references readAllBytes =
    InteractiveChecker.Create(List.ofArray references, readAllBytes)

let parseFSharpProject (checker: InteractiveChecker) fileName source =
    let parseResults, typeCheckResults, projectResults = checker.ParseAndCheckScript (fileName, source)
    for er in projectResults.Errors do
        printfn "FSHARP: %s" er.Message
    parseResults, typeCheckResults, projectResults

/// Get tool tip at the specified location
let getToolTipAtLocation (typeCheckResults: FSharpCheckFileResults) line col lineText =
    typeCheckResults.GetToolTipText(line, col, lineText, [], FSharpTokenTag.IDENT)

let getCompletionsAtLocation (parseResults: FSharpParseFileResults) (typeCheckResults: FSharpCheckFileResults)
                                                                        line col lineText =
    typeCheckResults.GetDeclarationListInfo(Some parseResults, line, col, lineText, [], "msg", (fun _ -> []), fun _ -> false)

(*
    let fileName = "test_script.fsx"
    let source = readAllText fileName

    let checker = InteractiveChecker.Create(references, readAllBytes)
    // let parseResults = checker.ParseScript(fileName,source)
    let parseResults, typeCheckResults, projectResults = checker.ParseAndCheckScript(fileName,source)
    
    printfn "parseResults.ParseHadErrors: %A" parseResults.ParseHadErrors
    printfn "parseResults.Errors: %A" parseResults.Errors
    //printfn "parseResults.ParseTree: %A" parseResults.ParseTree
    
    printfn "typeCheckResults Errors: %A" typeCheckResults.Errors
    printfn "typeCheckResults Entities: %A" typeCheckResults.PartialAssemblySignature.Entities
    //printfn "typeCheckResults Attributes: %A" typeCheckResults.PartialAssemblySignature.Attributes

    printfn "projectResults Errors: %A" projectResults.Errors
    //printfn "projectResults Contents: %A" projectResults.AssemblyContents

    printfn "Typed AST:"
    projectResults.AssemblyContents.ImplementationFiles
    |> Seq.iter (fun file -> AstPrint.printFSharpDecls "" file.Declarations |> Seq.iter (printfn "%s"))

    let inputLines = source.Split('\n')
    async {
        // Get tool tip at the specified location
        let! tip = typeCheckResults.GetToolTipTextAlternate(3, 7, inputLines.[2], ["foo"], FSharpTokenTag.IDENT)
        (sprintf "%A" tip).Replace("\n","") |> printfn "---> ToolTip Text = %A" // should be "FSharpToolTipText [...]"
    } |> Async.StartImmediate
    async {
        // Get declarations (autocomplete) for a location
        let! decls = typeCheckResults.GetDeclarationListInfo(Some parseResults, 6, 25, inputLines.[5], [], "msg", (fun _ -> []), fun _ -> false)
        [ for item in decls.Items -> item.Name ] |> printfn "---> AutoComplete = %A" // should be string methods
    } |> Async.StartImmediate
*)