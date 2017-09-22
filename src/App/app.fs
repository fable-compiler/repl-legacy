module App

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Recharts
open Fable.Recharts.Props
open Fulma.Layouts
open Fulma.BulmaClasses
open Fulma.Elements
module R = Fable.Helpers.React
module RProps = Fable.Helpers.React.Props

[<Emit("window[$0] = $1")>]
let makeGlobal (key: string) (value: obj): unit = jsNative
let create(elId: string): unit = importMember "editor"

type [<Pojo>] MyData =
    { name: string; uv: float; pv: float; amt: float }

let data =
    [| { name = "Page A"; uv = 4000.; pv = 2400.; amt = 2400. }
       { name = "Page B"; uv = 3000.; pv = 1398.; amt = 2210. }
       { name = "Page C"; uv = 2000.; pv = 9800.; amt = 2290. }
       { name = "Page D"; uv = 2780.; pv = 3908.; amt = 2000. }
       { name = "Page E"; uv = 1890.; pv = 4800.; amt = 2181. }
       { name = "Page F"; uv = 2390.; pv = 3800.; amt = 2500. }
       { name = "Page G"; uv = 3490.; pv = 4300.; amt = 2100. }
    |]

type [<Pojo>] Props<'T> = { InitData: 'T; Render: 'T->React.ReactElement }

type [<Pojo>] State<'T> = { Data: 'T }

type Component<'T>(props) =
    inherit React.Component<Props<'T>,State<'T>>(props)
    do base.setInitState({ Data = props.InitData })
    member this.componentDidMount() =
        makeGlobal "setData" (fun v -> this.setState({ Data = v }))

    member this.render() =
        this.props.Render(this.state.Data)

let barChart(data) =
    let margin = { top=5.; bottom=5.; right=20.; left=0. }
    barChart [Width 600.; Height 300.; Margin margin; Data data] [
        xaxis [DataKey "name"]
        yaxis []
        tooltip []
        legend []
        cartesianGrid [RProps.StrokeDasharray "3 3"]
        bar [DataKey "uv"; RProps.Fill "#8884d8"]
    ]

let root() =
    R.div [] [
        Hero.hero [ (*Hero.isSuccess; Hero.isMedium*) ] [
            Hero.body [ ] [
                Level.level [] [
                    Level.item [] [
                        R.img [
                            RProps.Src "img/fable_ionide.png"
                            RProps.Style [RProps.MaxWidth "200px"]
                        ]
                    ]
                    Level.item [] [
                        Heading.h1 [] [ R.str "Fable Ionide" ]  
                    ]
                ]
            ]
        ]
        Columns.columns [] [
            Column.column [] [
                R.div [RProps.ClassName "editor-frame"] [
                    R.div [
                        RProps.Id "fable-repl"
                        RProps.Style [RProps.CSSProp.Height "600px"]
                    ] []
                ]
            ]
            Column.column [] [
                R.div [RProps.Style [RProps.MarginTop "100px"]] [
                    R.com<Component<Array>,_,_> { InitData = data; Render = barChart } []
                ]
            ]
        ]
    ]

let renderApp() =
    let domEl = Browser.document.getElementById("app-container")
    ReactDom.render(root(), domEl)
    create "fable-repl"

renderApp()