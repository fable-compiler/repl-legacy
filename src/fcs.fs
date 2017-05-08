module Fable.FCS

open System
open System.Collections.Generic
open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices


let createChecker readAllBytes references =
    InteractiveChecker(List.ofArray references, readAllBytes)

let parseFSharpProject (checker: InteractiveChecker) fileName source =
    let _,_,checkProjectResults = checker.ParseAndCheckScript (fileName, source)
    for er in checkProjectResults.Errors do
        let severity =
            match er.Severity with
            | FSharpErrorSeverity.Warning -> Severity.Warning
            | FSharpErrorSeverity.Error -> Severity.Error
        let range =
            { start={ line=er.StartLineAlternate; column=er.StartColumn}
            ; ``end``={ line=er.EndLineAlternate; column=er.EndColumn} }
        printfn "FSHARP: %s" er.Message
    checkProjectResults
