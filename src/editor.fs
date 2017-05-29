// source: https://github.com/ionide/ionide-web/blob/master/src/editor.fsx

module Fable.Editor

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import

//---------------------------------------------------
// DTOs
//---------------------------------------------------
[<AutoOpen>]
module DTO =

    type ParseRequest = { FileName: string; IsAsync: bool; Lines: string[] }
    type ProjectRequest = { FileName: string }
    type DeclarationsRequest = { FileName: string }
    type HelptextRequest = { Symbol: string }
    type PositionRequest = { FileName: string; Line: int; Column: int; Filter: string }
    type CompletionRequest = { FileName: string; SourceLine: string; Line: int; Column: int; Filter: string }

    type OverloadSignature = {
        Signature: string
        Comment: string
    }
    type Error = {
        StartLine: int      // 1-indexed first line of the error block
        StartColumn: int    // 1-indexed first column of the error block
        EndLine: int        // 1-indexed last line of the error block
        EndColumn: int      // 1-indexed last column of the error block
        Message: string     // Description of the error
        Severity: string    // Severity of the error - warning or error
        Subcategory: string // Type of the Error
        FileName: string    // File Name
    }
    type Declaration = {
        File: string
        Line: int
        Column: int
    }
    type Completion = {
        Name: string
        ReplacementText: string
        Glyph: string
        GlyphChar: string
    }
    type SymbolUse = {
        FileName: string
        StartLine: int
        StartColumn: int
        EndLine: int
        EndColumn: int
        IsFromDefinition: bool
        IsFromAttribute: bool
        IsFromComputationExpression: bool
        IsFromDispatchSlotImplementation: bool
        IsFromPattern: bool
        IsFromType: bool
    }
    type SymbolUses = {
        Name: string
        Uses: SymbolUse array
    }
    type Helptext = {
        Name: string
        Overloads: OverloadSignature [] []
    }
    type OverloadParameter = {
        Name: string
        CanonicalTypeTextForSorting: string
        Display: string
        Description: string
    }
    type Overload = {
        Tip: OverloadSignature [] []
        TypeText: string
        Parameters: OverloadParameter []
        IsStaticArguments: bool
    }
    type Method = {
        Name: string
        CurrentParameter: int
        Overloads: Overload []
    }
    type CompilerLocation = {
        Fcs: string
        Fsi: string
        MSBuild: string
    }
    type Range = {
        StartColumn: int
        StartLine: int
        EndColumn: int
        EndLine: int
    }
    type Symbol = {
        UniqueName: string
        Name: string
        Glyph: string
        GlyphChar: string
        IsTopLevel: bool
        Range: Range
        BodyRange: Range
    }
    type Symbols = {
        Declaration: Symbol
        Nested: Symbol []
    }
    type Result<'T> = { Kind: string; Data: 'T }
    type CompilerLocationResult = Result<CompilerLocation>
    type HelptextResult = Result<Helptext>
    type CompletionResult = Result<Completion[]>
    type SymbolUseResult = Result<SymbolUses>
    type TooltipResult = Result<OverloadSignature[][]>
    type ParseResult = Result<Error[]>
    type FindDeclarationResult = Result<Declaration>
    type MethodResult = Result<Method>
    type DeclarationResult = Result<Symbols[]>

//---------------------------------------------------
// PromisesExt (by Dave)
//---------------------------------------------------
module Promise =
    let success (a: 'T -> 'R) (pr: monaco.Promise<'T>): monaco.Promise<'R> =
        pr?``then`` $ a |> unbox

    let bind (a: 'T -> monaco.Promise<'R>) (pr: monaco.Promise<'T>): monaco.Promise<'R> =
        pr?``then`` $ a |> unbox

    let lift<'T> (a: 'T): monaco.Promise<'T> =
        monaco.Promise<'T>.wrap(a)

type PromiseBuilder() =
        member inline x.Bind(m,f) = Promise.bind f m
        member inline x.Return(a) = Promise.lift a
        //member inline x.ReturnFrom(a) = a
        //member inline x.Zero() = Promise.lift ()

[<AutoOpen>]
module PromiseBuilderImp =
    let promise = PromiseBuilder()

//---------------------------------------------------
// Communication
//---------------------------------------------------
let request<'a, 'b> (obj: 'a) endpoint _id =
    printfn "endpoint: %s" endpoint
    // let hn = Browser.window.location.hostname
    // let ep = sprintf "http://%s:81/%s" hn endpoint
    // Globals.axios.post (ep, obj)
    //|> Promise.success(fun r -> (r.data |> unbox<string[]>).[id] |> JS.JSON.parse |> unbox<'b>)
    promise {
        //let json = "TODO"
        //let res = json |> JS.JSON.parse |> unbox<'b>
        let res = createEmpty<'b> |> unbox<'b>
        return res
    }

let parse s = request<ParseRequest, ParseResult> s "parse" 0
let completion s = request<CompletionRequest, CompletionResult> s "completion" 1
let tooltip s = request<PositionRequest, TooltipResult> s "tooltip" 0
let helptext s = request<HelptextRequest, HelptextResult> s "helptext" 0
let methods s = request<PositionRequest, MethodResult> s "methods" 0
let symbolUse s = request<PositionRequest, SymbolUseResult> s "symboluse" 0
let declaration s = request<PositionRequest, FindDeclarationResult> s "finddeclaration" 0


//---------------------------------------------------
// Features providers
//---------------------------------------------------

let hoverProvider = {
    new monaco.languages.HoverProvider with

        member __.provideHover(model, position, token) =
            promise {
                let! o = tooltip { FileName = "test.fsx"; Line = position.lineNumber |> unbox; Column = position.column |> unbox; Filter = "" }
                let res = (o.Data |> Array.fold (fun acc n -> (n |> Array.toList) @ acc ) []).Head

                let h = createEmpty<monaco.languages.Hover>
                let ctn = res.Signature |> U2.Case1

                let w = model.getWordAtPosition(position |> unbox)

                let range = createEmpty<monaco.IRange>
                range.startLineNumber <- position.lineNumber
                range.endLineNumber <- position.lineNumber
                range.startColumn <- w.startColumn
                range.endColumn <- w.endColumn

                h.contents <- ResizeArray([ctn])
                h.range <- range

                return h
            } |> U2.Case2
}

let convertToInt code =
    match code with
    | "C" -> monaco.languages.CompletionItemKind.Class
    | "E" -> monaco.languages.CompletionItemKind.Enum
    | "S" -> monaco.languages.CompletionItemKind.Value
    | "I" -> monaco.languages.CompletionItemKind.Interface
    | "N" -> monaco.languages.CompletionItemKind.Module
    | "M" -> monaco.languages.CompletionItemKind.Method
    | "P" -> monaco.languages.CompletionItemKind.Property
    | "F" -> monaco.languages.CompletionItemKind.Field
    | "T" -> monaco.languages.CompletionItemKind.Class
    | _   -> monaco.languages.CompletionItemKind.Text

let completionProvider = {
    new monaco.languages.CompletionItemProvider with

        member __.provideCompletionItems(model, position, token) =
            promise {
                let! o = completion {
                    FileName = "test.fsx"
                    SourceLine = model.getLineContent position.lineNumber
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = "Contains"
                }
                return
                    o.Data |> Array.map (fun d ->
                        let ci = createEmpty<monaco.languages.CompletionItem>
                        ci.kind <- d.GlyphChar |> convertToInt |> unbox
                        ci.label <- d.Name
                        ci.insertText <- Some (d.ReplacementText |> U2.Case1)
                        ci)
                    |> ResizeArray
            } |> U4.Case2

        member __.resolveCompletionItem(item, token) =
            promise {
                let! o = helptext { Symbol = item.label }
                let res = (o.Data.Overloads |> Array.fold (fun acc n -> (n |> Array.toList) @ acc ) []).Head
                item.documentation <- Some res.Comment
                item.detail <- Some res.Signature
                return item
            } |> U2.Case2

        member __.triggerCharacters
            with get () = ResizeArray(["."]) |> Some
            and set v = ()
}

let parseEditor (model: monaco.editor.IModel) =
    promise {
        let content = model.getValue (monaco.editor.EndOfLinePreference.TextDefined, true)
        let! res = parse {
            FileName = "test.fsx"
            IsAsync = true
            Lines = content.Split('\n')
        }
        return ()
    }

let signatureProvider = {
    new monaco.languages.SignatureHelpProvider with

        member __.signatureHelpTriggerCharacters
            with get () = ResizeArray(["("; ","])
            and set v = ()

        member __.provideSignatureHelp(model, position, token) =
            promise {
                let! o = methods {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                let res = createEmpty<monaco.languages.SignatureHelp>
                let sigs = o.Data.Overloads |> Array.map (fun c ->
                    let tip = c.Tip.[0].[0]
                    let si = createEmpty<monaco.languages.SignatureInformation>
                    si.label <- tip.Signature
                    si.documentation <- Some tip.Comment
                    si.parameters <- ResizeArray ()
                    c.Parameters |> Array.iter (fun p ->
                        let pi = createEmpty<monaco.languages.ParameterInformation>
                        pi.label <- p.Name
                        pi.documentation <- Some p.CanonicalTypeTextForSorting
                        si.parameters.Add(pi )
                    )
                    si)

                res.activeParameter <- float (o.Data.CurrentParameter)
                res.activeSignature <-
                    sigs
                    |> Array.sortBy (fun n -> n.parameters.Count)
                    |> Array.findIndex (fun s -> s.parameters.Count >= o.Data.CurrentParameter )
                    |> (+) 1
                    |> float
                res.signatures <- ResizeArray sigs

                return res
            } |> U2.Case2
}

let highlighterProvider = {
    new monaco.languages.DocumentHighlightProvider with

        member __.provideDocumentHighlights(model, position, token) =
            promise {
                let! o = symbolUse {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                return
                    o.Data.Uses |> Array.map (fun d ->
                        let res = createEmpty<monaco.languages.DocumentHighlight>
                        res.range <- monaco.Range (float d.StartLine, float d.StartColumn, float d.EndLine, float d.EndColumn) |> unbox
                        res.kind <- (0 |> unbox)
                        res)
                    |> ResizeArray
            } |> U2.Case2
}

let renameProvider = {
    new monaco.languages.RenameProvider with

        member __.provideRenameEdits(model, position, newName, token) =
            promise {
                let! o = symbolUse {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                let we = createEmpty<monaco.languages.WorkspaceEdit>
                we.edits <-
                    o.Data.Uses |> Array.map (fun s ->
                        let range = monaco.Range(float s.StartLine, (float s.EndColumn) - (float o.Data.Name.Length), float s.EndLine, float s.EndColumn)
                        let re = createEmpty<monaco.languages.IResourceEdit>
                        re.newText <- newName
                        re.resource <- model.uri
                        re.range <- range |> unbox
                        re)
                    |> ResizeArray
                return we
            } |> U2.Case2
}

let definitionProvider = {
    new monaco.languages.DefinitionProvider with

        member __.provideDefinition(model, position, token) =
            promise {
                let! o = declaration {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                let d = createEmpty<monaco.languages.Location>
                d.range <- monaco.Range(float o.Data.Line, float o.Data.Column, float o.Data.Line, float o.Data.Column) |> unbox
                d.uri <- model.uri
                return d |> U2.Case1
            } |> U2.Case2
}

let referenceProvider = {
    new monaco.languages.ReferenceProvider with

        member __.provideReferences(model, position, ctx, token) =
            promise {
                let! o = symbolUse {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                return
                    o.Data.Uses |> Array.map (fun d ->
                        let res = createEmpty<monaco.languages.Location>
                        res.range <- monaco.Range (float d.StartLine - 1., float d.StartColumn, float d.EndLine - 1., float d.EndColumn) |> unbox
                        res)
                    |> ResizeArray
            } |> U2.Case2
}

//---------------------------------------------------
// Register providers
//---------------------------------------------------
// monaco.languages.Globals.registerHoverProvider("fsharp", hoverProvider) |> ignore
// monaco.languages.Globals.registerCompletionItemProvider("fsharp", completionProvider) |> ignore
// monaco.languages.Globals.registerSignatureHelpProvider("fsharp", signatureProvider) |> ignore
// monaco.languages.Globals.registerDocumentHighlightProvider("fsharp", highlighterProvider) |> ignore
// monaco.languages.Globals.registerRenameProvider("fsharp", renameProvider) |> ignore
// monaco.languages.Globals.registerDefinitionProvider("fsharp", definitionProvider) |> ignore
// monaco.languages.Globals.registerReferenceProvider("fsharp", referenceProvider) |> ignore

//---------------------------------------------------
// Create editor
//---------------------------------------------------
let domEditor = Browser.document.getElementById("editor")

let options = createEmpty<monaco.editor.IEditorConstructionOptions>
options.language <- Some "fsharp"
options.theme <- Some "vs-dark"
options.value <- Some "let t = 1"

let services = createEmpty<monaco.editor.IEditorOverrideServices>
let ed = monaco.editor.Globals.create(domEditor |> unbox, options, services)
let md = ed.getModel()
md.onDidChangeContent(fun k -> md |> parseEditor |> ignore) |> ignore

// todo on resize:
//     ed.layout()

// FCS references to avoid treeshaking
let parseAndCheck source =
    let references = [|"FSharp.Core";"mscorlib";"System";"System.Core";"System.Data";"System.IO";"System.Xml";"System.Numerics";"Fable.Core"|]
    let readAllBytes: string -> byte[] = jsNative
    let fileName = "stdin.fsx"
    let checker = Fable.FCS.createChecker references readAllBytes
    let results = Fable.FCS.parseFSharpProject checker fileName source
    results
