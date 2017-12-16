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
    | LoadSample of string * string
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
        let (samplesModel, samplesCmd, externalMsg) = Samples.update msg model.Samples

        let extraMsg =
            match externalMsg with
            | Samples.NoOp -> NoOp
            | Samples.LoadSample (fsharpCode, htmlCode) -> LoadSample (fsharpCode, htmlCode)

        { model with Samples = samplesModel }, Cmd.map SamplesMsg samplesCmd, extraMsg

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
