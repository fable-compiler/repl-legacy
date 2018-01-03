module Sidebar

open Elmish

type Model =
    { IsExpanded : bool
      Samples : Samples.Model }

type Msg =
    | SamplesMsg of Samples.Msg

type ExternalMsg =
    | LoadSample of string * string
    | NoOp

let init _ =
    { IsExpanded = false
      Samples = Samples.init () }

let update msg model =
    match msg with
    | SamplesMsg msg ->
        let (samplesModel, samplesCmd, externalMsg) = Samples.update msg model.Samples

        let extraMsg =
            match externalMsg with
            | Samples.NoOp -> NoOp
            | Samples.LoadSample (fsharpCode, htmlCode) -> LoadSample (fsharpCode, htmlCode)

        { model with Samples = samplesModel }, Cmd.map SamplesMsg samplesCmd, extraMsg

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma.Components

let view (model: Model) dispatch =
    if model.IsExpanded then
        div [ ClassName "sidebar" ]
            [ Card.card [ ]
                [ Card.content [ ]
                    [ Samples.view model.Samples (SamplesMsg >> dispatch)
                    ] ]
            ]
    else
        div [] []
