// source: https://github.com/ionide/ionide-web/blob/master/src/editor.fsx

module Fable.Editor

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import

//---------------------------------------------------
// Features providers
//---------------------------------------------------

let getChecker(f: string[] -> (string->byte[]) -> FCS.Checker): FCS.Checker option =
    importMember "./util.js"

let mutable fcsChecker: FCS.Checker option = None
let mutable fcsResults: FCS.ParseResults option = None

let convertGlyph glyph =
    match glyph with
    | FCS.Glyph.Class | FCS.Glyph.Struct | FCS.Glyph.Union
    | FCS.Glyph.Type | FCS.Glyph.Typedef ->
        monaco.languages.CompletionItemKind.Class
    | FCS.Glyph.Enum | FCS.Glyph.EnumMember ->
        monaco.languages.CompletionItemKind.Enum
    | FCS.Glyph.Constant ->
        monaco.languages.CompletionItemKind.Value
    | FCS.Glyph.Variable ->
        monaco.languages.CompletionItemKind.Variable
    | FCS.Glyph.Interface ->
        monaco.languages.CompletionItemKind.Interface
    | FCS.Glyph.Module | FCS.Glyph.NameSpace ->
        monaco.languages.CompletionItemKind.Module
    | FCS.Glyph.Method | FCS.Glyph.OverridenMethod | FCS.Glyph.ExtensionMethod ->
        monaco.languages.CompletionItemKind.Method
    | FCS.Glyph.Property ->
        monaco.languages.CompletionItemKind.Property
    | FCS.Glyph.Field ->
        monaco.languages.CompletionItemKind.Field
    | FCS.Glyph.Delegate ->
        monaco.languages.CompletionItemKind.Function
    | FCS.Glyph.Error | FCS.Glyph.Exception | FCS.Glyph.Event ->
        monaco.languages.CompletionItemKind.Text

let completionProvider = {
    new monaco.languages.CompletionItemProvider with

        member __.provideCompletionItems(model, position, token) =
           async {
                let items = ResizeArray()
                match fcsResults with
                | Some res ->
                    let! decls = FCS.getCompletionsAtLocation res.ParseFile res.CheckFile
                                    !!position.lineNumber !!position.column (model.getLineContent(position.lineNumber))
                    for d in decls.Items do
                        let ci = createEmpty<monaco.languages.CompletionItem>
                        ci.kind <- convertGlyph d.Glyph
                        ci.label <- d.Name
                        // ci.insertText <- Some !^d.ReplacementText
                        items.Add(ci)
                | None -> ()
                return items
            } |> Async.StartAsPromise |> U4.Case2

        member __.resolveCompletionItem(item, token) =
            !^item
            // promise {
            //     let! o = helptext { Symbol = item.label }
            //     let res = (o.Data.Overloads |> Array.fold (fun acc n -> (n |> Array.toList) @ acc ) []).Head
            //     item.documentation <- Some res.Comment
            //     item.detail <- Some res.Signature
            //     return item
            // } |> U2.Case2

        member __.triggerCharacters
            with get () = ResizeArray(["."]) |> Some
            and set v = ()
}

let parseEditor (model: monaco.editor.IModel) =
    match fcsChecker with
    | None ->
        fcsChecker <- getChecker FCS.createChecker
    | Some fcsChecker ->
        let content = model.getValue (monaco.editor.EndOfLinePreference.TextDefined, true)
        let res = FCS.parseFSharpProject fcsChecker "test.fsx" content
        fcsResults <- Some res
        let markers = ResizeArray()
        for err in res.CheckProject.Errors do
            let m = createEmpty<monaco.editor.IMarkerData>
            m.startLineNumber <- float err.StartLineAlternate
            m.endLineNumber <- float err.EndLineAlternate
            m.startColumn <- float err.StartColumn
            m.endColumn <- float err.EndColumn
            m.message <- err.Message
            m.severity <-
                match err.Severity with
                | FCS.Severity.Error -> monaco.Severity.Error 
                | FCS.Severity.Warning -> monaco.Severity.Warning
            markers.Add(m)
        monaco.editor.Globals.setModelMarkers(model, "test", markers)

//---------------------------------------------------
// Register providers
//---------------------------------------------------
monaco.languages.Globals.registerCompletionItemProvider("fsharp", completionProvider) |> ignore

//---------------------------------------------------
// Create editor
//---------------------------------------------------
let createEditor() =
    let domEditor = Browser.document.getElementById("editor")

    let options = createEmpty<monaco.editor.IEditorConstructionOptions>
    options.language <- Some "fsharp"
    options.theme <- Some "vs-dark"
    options.value <- Some "let t = 1"

    let services = createEmpty<monaco.editor.IEditorOverrideServices>
    let ed = monaco.editor.Globals.create(!!domEditor, options, services)
    let md = ed.getModel()

    Util.createObservable(fun trigger ->
        md.onDidChangeContent(fun _ -> trigger md) |> ignore)
    |> Util.debounce 1000
    |> Observable.add parseEditor

createEditor()

// todo on resize:
//     ed.layout()
