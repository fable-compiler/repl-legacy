module Fable.FCS

open System
open System.Collections.Generic
open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices
open FsAutoComplete

type Glyph = FSharpGlyph
type Checker = InteractiveChecker
type Severity = FSharpErrorSeverity

type ParseResults =
    { ParseFile: FSharpParseFileResults
      CheckFile: FSharpCheckFileResults
      CheckProject: FSharpCheckProjectResults }

let findLongIdentsAndResidue (col, lineStr:string) =
  let lineStr = lineStr.Substring(0, col)

  match Lexer.getSymbol 0 col lineStr Lexer.SymbolLookupKind.ByLongIdent [||] with
  | Some sym ->
      match sym.Text with
      | "" -> [], ""
      | text ->
          let res = text.Split '.' |> List.ofArray |> List.rev
          if lineStr.[col - 1] = '.' then res |> List.rev, ""
          else
              match res with
              | head :: tail -> tail |> List.rev, head
              | [] -> [], ""
  | _ -> [], ""

let createChecker references readAllBytes =
    InteractiveChecker.Create(List.ofArray references, readAllBytes)

let parseFSharpProject (checker: InteractiveChecker) fileName source =
    let parseResults, typeCheckResults, projectResults = checker.ParseAndCheckScript (fileName, source)
    { ParseFile = parseResults
      CheckFile = typeCheckResults
      CheckProject = projectResults }

/// Get tool tip at the specified location
let getToolTipAtLocation (typeCheckResults: FSharpCheckFileResults) line col lineText =
    typeCheckResults.GetToolTipText(line, col, lineText, [], FSharpTokenTag.IDENT)

let getCompletionsAtLocation (parseResults: FSharpParseFileResults) (typeCheckResults: FSharpCheckFileResults) line col lineText =
    let longName, residue = findLongIdentsAndResidue(col - 1, lineText)
    typeCheckResults.GetDeclarationListInfo(Some parseResults, line, col, lineText, longName, residue, (fun () -> []))
