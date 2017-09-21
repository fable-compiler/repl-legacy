module Fable.REPL.Interfaces

open Fable.Core

[<StringEnum; RequireQualifiedAccess>]
type Glyph =
    | Class
    | Enum
    | Value
    | Variable
    | Interface
    | Module
    | Method
    | Property
    | Field
    | Function
    | Error
    | Event

[<Pojo>]
type Error =
    { StartLineAlternate: int
      StartColumn: int
      EndLineAlternate: int
      EndColumn: int
      Message: string
      IsWarning: bool }

[<Pojo>]
type Completion =
    { Name: string
      Glyph: Glyph }

type IChecker =
    interface end

type IParseResults =
    abstract Errors: Error[]

type IFableREPL =
    abstract CreateChecker: references:string[]*readAllBytes:(string->byte[])->IChecker
    abstract ParseFSharpProject: checker:IChecker*fileName:string*source:string->IParseResults
    abstract GetCompletionsAtLocation: parseResults:IParseResults*line:int*col:int*lineText:string->Async<Completion[]>
