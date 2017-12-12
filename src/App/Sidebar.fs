module Sidebar

open Elmish

type Model =
    { IsExpanded : bool
      Samples : Samples.Model }

type Msg =
    | Expand
    | Collapse
    | SamplesMsg of Samples.Msg

type ExternalMsg =
    | LoadSample of string
    | NoOp

let init _ =
    { IsExpanded = true
      Samples = Samples.init () }

let update msg model =
    match msg with
    | Expand ->
        { model with IsExpanded = true }, Cmd.none, NoOp

    | Collapse ->
        { model with IsExpanded = false }, Cmd.none,NoOp

    | SamplesMsg msg ->
        let (samplesModel, samplesCmd) = Samples.update msg model.Samples

        { model with Samples = samplesModel }, Cmd.map SamplesMsg samplesCmd, NoOp

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
                [ Samples.view model.Samples (SamplesMsg >> dispatch)
                ] ]
        ]
