module App

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Recharts
open Fable.Recharts.Props
open Fulma.Layouts
module R = Fable.Helpers.React
module RProps = Fable.Helpers.React.Props

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
                barChart data
            ]
        ]
    ]

let renderApp() =
    let domEl = Browser.document.getElementById("app-container")
    ReactDom.render(root(), domEl)
    create "fable-repl"

renderApp()