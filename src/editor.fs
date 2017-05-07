// source: https://github.com/ionide/ionide-web/blob/master/src/editor.fsx

module Fable.Repl

open Fable.Core  
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Import.monaco

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
        /// 1-indexed first line of the error block
        StartLine: int
        /// 1-indexed first column of the error block
        StartColumn: int
        /// 1-indexed last line of the error block
        EndLine: int
        /// 1-indexed last column of the error block
        EndColumn: int
        /// Description of the error
        Message: string
        ///Severity of the error - warning or error
        Severity: string
        /// Type of the Error
        Subcategory: string
        ///File Name
        FileName: string
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
// Initialization
//---------------------------------------------------

[<Emit("_monaco = monaco")>]
let hack: unit = jsNative
hack

let dom = Browser.document.getElementsByClassName("container").[0]

let options = createEmpty<editor.IEditorConstructionOptions>
options.value <- Some "let t = 1"
options.language <- Some "fsharp"

let services = createEmpty<editor.IEditorOverrideServices>

//---------------------------------------------------
// PromisesExt (by Dave)
//---------------------------------------------------
module Promise =
    let success (a: 'T -> 'R) (pr: Promise<'T>): Promise<'R> =
        pr?``then`` $ a |> unbox 

    let bind (a: 'T -> Promise<'R>) (pr: Promise<'T>): Promise<'R> =
        pr?``then`` $ a |> unbox

    let lift<'T> (a: 'T): Promise<'T> =
        Promise<'T>.wrap(a)

type PromiseBuilder() =
        member inline x.Bind(m,f) = Promise.bind f m
        member inline x.Return(a) = Promise.lift a
        member inline x.ReturnFrom(a) = a
        member inline x.Zero() = Fable.Import.JS.Promise.resolve()

[<AutoOpen>]
module PromiseBuilderImp =
    let promise = PromiseBuilder()

//---------------------------------------------------
// Communication
//---------------------------------------------------
let request<'a, 'b> (obj: 'a) endpoint _id = 
    // let hn = Browser.window.location.hostname
    // let ep = sprintf "http://%s:81/%s" hn endpoint
    // Globals.axios.post (ep, obj)
    //|> Promise.success(fun r -> (r.data |> unbox<string[]>).[id] |> JS.JSON.parse |> unbox<'b>)
    promise {
        let json = "aaa"
        let res = json |> JS.JSON.parse |> unbox<'b>
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
    new languages.HoverProvider with
        member this.provideHover(model, position, token) = 
            promise {
                let! o = tooltip { FileName = "test.fsx"; Line = position.lineNumber |> unbox; Column = position.column |> unbox; Filter = "" }
                let res = (o.Data |> Array.fold (fun acc n -> (n |> Array.toList) @ acc ) []).Head

                let h = createEmpty<languages.Hover>
                let ctn = Some res.Signature |> MarkedString.Case1
                
                let w = model.getWordAtPosition(position |> unbox)

                let range = createEmpty<IRange>
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
    | "C" -> languages.CompletionItemKind.Class
    | "E" -> languages.CompletionItemKind.Enum
    | "S" -> languages.CompletionItemKind.Value
    | "I" -> languages.CompletionItemKind.Interface
    | "N" -> languages.CompletionItemKind.Module
    | "M" -> languages.CompletionItemKind.Method
    | "P" -> languages.CompletionItemKind.Property
    | "F" -> languages.CompletionItemKind.Field
    | "T" -> languages.CompletionItemKind.Class
    | _   -> languages.CompletionItemKind.Text

let completionProvider = {
    new languages.CompletionItemProvider with

        member this.provideCompletionItems(model, position, token) = 
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
                        let ci = createEmpty<languages.CompletionItem>
                        ci.kind <- d.GlyphChar |> convertToInt |> unbox
                        ci.label <- d.Name
                        ci.insertText <- Some (d.ReplacementText |> U2.Case1)
                        ci)
                    |> ResizeArray
            } |> U4.Case2

        member this.resolveCompletionItem(item, token) = 
            promise {
                let! o = helptext { Symbol = item.label }
                let res = (o.Data.Overloads |> Array.fold (fun acc n -> (n |> Array.toList) @ acc ) []).Head
                item.documentation <- Some res.Comment
                item.detail <- Some res.Signature
                return item
            } |> U2.Case2

        member this.triggerCharacters with get () = ResizeArray(["."]) |> Some
}

let parseEditor (model: editor.IModel) =
    promise {
        let content = model.getValue (editor.EndOfLinePreference.TextDefined, true)
        let! res = parse {
            FileName = "test.fsx"
            IsAsync = true
            Lines = content.Split('\n')
        }
        return ()
    }

let signatureProvider = {
    new languages.SignatureHelpProvider with

        member this.signatureHelpTriggerCharacters with get () = ResizeArray(["("; ","])

        member this.provideSignatureHelp(model, position, token) = 
            promise {
                let! o = methods {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                let res = createEmpty<languages.SignatureHelp>
                let sigs = o.Data.Overloads |> Array.map (fun c ->
                    let tip = c.Tip.[0].[0]
                    let si = createEmpty<languages.SignatureInformation>
                    si.label <- tip.Signature
                    si.documentation <- Some tip.Comment
                    si.parameters <- ResizeArray ()
                    c.Parameters |> Array.iter (fun p ->
                        let pi = createEmpty<languages.ParameterInformation>
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
    new languages.DocumentHighlightProvider with

        member this.provideDocumentHighlights(model, position, token) = 
            promise {
                let! o = symbolUse {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                return
                    o.Data.Uses |> Array.map (fun d ->
                        let res = createEmpty<languages.DocumentHighlight> 
                        res.range <- Range (float d.StartLine, float d.StartColumn, float d.EndLine, float d.EndColumn) |> unbox
                        res.kind <- (0 |> unbox)
                        res)
                    |> ResizeArray
            } |> U2.Case2
}

let renameProvider = {
    new languages.RenameProvider with

        member this.provideRenameEdits(model, position, newName, token) = 
            promise {
                let! o = symbolUse {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                let we = createEmpty<languages.WorkspaceEdit>
                we.edits <-
                    o.Data.Uses |> Array.map (fun s ->
                        let range = Range(float s.StartLine, (float s.EndColumn) - (float o.Data.Name.Length), float s.EndLine, float s.EndColumn)
                        let re = createEmpty<languages.IResourceEdit>
                        re.newText <- newName
                        re.resource <- model.uri
                        re.range <- range |> unbox
                        re)
                    |> ResizeArray
                return we
            } |> U2.Case2
}

let definitionProvider = {
    new languages.DefinitionProvider with

        member this.provideDefinition(model, position, token) = 
            promise {
                let! o = declaration {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                let d = createEmpty<languages.Location>
                d.range <- Range(float o.Data.Line, float o.Data.Column, float o.Data.Line, float o.Data.Column) |> unbox
                d.uri <- model.uri
                return d |> U2.Case1
            } |> U2.Case2
}

let referenceProvider = {
    new languages.ReferenceProvider with

        member this.provideReferences(model, position, ctx, token) = 
            promise {
                let! o = symbolUse {
                    FileName = "test.fsx"
                    Line = position.lineNumber |> unbox
                    Column = position.column |> unbox
                    Filter = ""
                }
                return
                    o.Data.Uses |> Array.map (fun d ->
                        let res = createEmpty<languages.Location> 
                        res.range <- Range (float d.StartLine - 1., float d.StartColumn, float d.EndLine - 1., float d.EndColumn) |> unbox
                        res)
                    |> ResizeArray
            } |> U2.Case2
}

//---------------------------------------------------
// Create editor
//---------------------------------------------------

monaco.languages.Globals.registerHoverProvider("fsharp", hoverProvider) |> ignore
monaco.languages.Globals.registerCompletionItemProvider("fsharp", completionProvider) |> ignore
monaco.languages.Globals.registerSignatureHelpProvider("fsharp", signatureProvider) |> ignore
monaco.languages.Globals.registerDocumentHighlightProvider("fsharp", highlighterProvider) |> ignore
monaco.languages.Globals.registerRenameProvider("fsharp", renameProvider) |> ignore
monaco.languages.Globals.registerDefinitionProvider("fsharp", definitionProvider) |> ignore
monaco.languages.Globals.registerReferenceProvider("fsharp", referenceProvider) |> ignore

let ed = monaco.editor.Globals.create(dom |> unbox, options, services )
let md = ed.getModel()
md.onDidChangeContent(fun k -> md |> parseEditor |> ignore) |> ignore
