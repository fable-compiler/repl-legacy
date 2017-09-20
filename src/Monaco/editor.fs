// source: https://github.com/ionide/ionide-web/blob/master/src/editor.fsx

module Fable.Editor

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.REPL.Interfaces

//---------------------------------------------------
// Features providers
//---------------------------------------------------

let [<Global>] FableREPL: IFableREPL = jsNative

let getChecker(f: string[] -> (string->byte[]) -> IChecker): IChecker option =
    importMember "./util.js"

let mutable fcsChecker: IChecker option = None
let mutable fcsResults: IParseResults option = None

let convertGlyph glyph =
    match glyph with
    | Glyph.Class ->
        monaco.languages.CompletionItemKind.Class
    | Glyph.Enum ->
        monaco.languages.CompletionItemKind.Enum
    | Glyph.Value ->
        monaco.languages.CompletionItemKind.Value
    | Glyph.Variable ->
        monaco.languages.CompletionItemKind.Variable
    | Glyph.Interface ->
        monaco.languages.CompletionItemKind.Interface
    | Glyph.Module ->
        monaco.languages.CompletionItemKind.Module
    | Glyph.Method ->
        monaco.languages.CompletionItemKind.Method
    | Glyph.Property ->
        monaco.languages.CompletionItemKind.Property
    | Glyph.Field ->
        monaco.languages.CompletionItemKind.Field
    | Glyph.Function ->
        monaco.languages.CompletionItemKind.Function
    | Glyph.Error | Glyph.Event ->
        monaco.languages.CompletionItemKind.Text

let completionProvider = {
    new monaco.languages.CompletionItemProvider with

        member __.provideCompletionItems(model, position, token) =
           async {
                let items = ResizeArray()
                match fcsResults with
                | Some res ->
                    let! decls =
                        model.getLineContent(position.lineNumber)
                        |> FableREPL.GetCompletionsAtLocation res position.lineNumber position.column
                    for d in decls do
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
        fcsChecker <- getChecker FableREPL.CreateChecker
    | Some fcsChecker ->
        let content = model.getValue (monaco.editor.EndOfLinePreference.TextDefined, true)
        let res = FableREPL.ParseFSharpProject fcsChecker "test.fsx" content
        fcsResults <- Some res
        let markers = ResizeArray()
        for err in res.Errors do
            let m = createEmpty<monaco.editor.IMarkerData>
            m.startLineNumber <- err.StartLineAlternate
            m.endLineNumber <- err.EndLineAlternate
            m.startColumn <- err.StartColumn
            m.endColumn <- err.EndColumn
            m.message <- err.Message
            m.severity <-
                match err.IsWarning with
                | false -> monaco.Severity.Error 
                | true -> monaco.Severity.Warning
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
