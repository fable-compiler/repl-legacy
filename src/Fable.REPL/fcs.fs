module Fable.REPL.FCS

open System
open System.Collections.Generic
open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices
open FsAutoComplete
open Interfaces

type ParseResults =
    { ParseFile: FSharpParseFileResults
      CheckFile: FSharpCheckFileResults
      CheckProject: FSharpCheckProjectResults
      Errors: Error[] }

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

let convertGlyph glyph =
    match glyph with
    | FSharpGlyph.Class | FSharpGlyph.Struct | FSharpGlyph.Union
    | FSharpGlyph.Type | FSharpGlyph.Typedef ->
        Glyph.Class
    | FSharpGlyph.Enum | FSharpGlyph.EnumMember ->
        Glyph.Enum
    | FSharpGlyph.Constant ->
        Glyph.Value
    | FSharpGlyph.Variable ->
        Glyph.Variable
    | FSharpGlyph.Interface ->
        Glyph.Interface
    | FSharpGlyph.Module | FSharpGlyph.NameSpace ->
        Glyph.Module
    | FSharpGlyph.Method | FSharpGlyph.OverridenMethod | FSharpGlyph.ExtensionMethod ->
        Glyph.Method
    | FSharpGlyph.Property ->
        Glyph.Property
    | FSharpGlyph.Field ->
        Glyph.Field
    | FSharpGlyph.Delegate ->
        Glyph.Function
    | FSharpGlyph.Error | FSharpGlyph.Exception ->
        Glyph.Error
    | FSharpGlyph.Event ->
        Glyph.Event

let CreateChecker references readAllBytes =
    InteractiveChecker.Create(List.ofArray references, readAllBytes)

let ParseFSharpProject (checker: InteractiveChecker) fileName source =
    let parseResults, typeCheckResults, projectResults = checker.ParseAndCheckScript (fileName, source)
    let errors =
        projectResults.Errors |> Array.map (fun er ->
            { StartLineAlternate = er.StartLineAlternate
              StartColumn = er.StartColumn
              EndLineAlternate = er.EndLineAlternate
              EndColumn = er.EndColumn
              Message = er.Message
              IsWarning =
                match er.Severity with
                | FSharpErrorSeverity.Error -> false
                | FSharpErrorSeverity.Warning -> true
            })        
    { ParseFile = parseResults
      CheckFile = typeCheckResults
      CheckProject = projectResults
      Errors = errors }

/// Get tool tip at the specified location
let GetToolTipAtLocation (typeCheckResults: FSharpCheckFileResults) line col lineText =
    typeCheckResults.GetToolTipText(line, col, lineText, [], FSharpTokenTag.IDENT)

let GetCompletionsAtLocation (parseResults: ParseResults) line col lineText = async {
    let longName, residue = findLongIdentsAndResidue(col - 1, lineText)
    let! decls = parseResults.CheckFile.GetDeclarationListInfo(Some parseResults.ParseFile, line, col, lineText, longName, residue, (fun () -> []))
    return decls.Items |> Array.map (fun decl ->
        { Name = decl.Name; Glyph = convertGlyph decl.Glyph })
}

