module Sidebar

open Elmish

type Model =
    { IsExpanded : bool }

type Msg =
    | Expand
    | Collapse

type ExternalMsg =
    | LoadSample of string
    | NoOp

let init _ =
    { IsExpanded = true }

let update msg model =
    match msg with
    | Expand ->
        { model with IsExpanded = true }, Cmd.none, NoOp

    | Collapse ->
        { model with IsExpanded = false }, Cmd.none,NoOp

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma.Elements
open Fulma.Extra.FontAwesome
open Fulma.Components

let view model dispatch =
    div [ ClassName "sidebar" ]
        [ Card.card [ ]
            [ Card.header [ ]
                [ Card.Header.title [ ]
                    [ str "Samples" ]
                  Card.Header.icon [ ]
                    [ Icon.faIcon [ ]
                        [ Fa.icon Fa.I.AngleUp
                          Fa.faLg ] ] ]
              Card.content [ ]
                [ Samples.view ()



                ] ]
        ]
