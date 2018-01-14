module Widgets.About

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma.Elements

let [<Literal>] VERSION = "0.1.0"

let view =
    Content.content [ ]
        [ str ("Version: " + VERSION)
          br [ ]
          a [ Href "https://github.com/fable-compiler/fable-repl" ] [ str "Found a bug ?" ] ]
