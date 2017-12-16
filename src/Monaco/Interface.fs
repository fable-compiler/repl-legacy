module Fable.Editor.Interfaces

open Fable.Import

type IExports =
    abstract CreateFSharpEditor : Browser.HTMLElement -> monaco.editor.IStandaloneCodeEditor
    abstract ParseEditor : monaco.editor.IModel -> unit
    abstract CompileAndRunCurrentResults : unit -> string * string
