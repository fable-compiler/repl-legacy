module Main

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fulma.Components
open Fulma.Elements
open Fulma.Elements.Form
open Fulma.Extra.FontAwesome
open Fable.Import.Browser
open Mouse

importSideEffects "./scss/main.scss"

module Editor =
    let create(element: Browser.HTMLElement): monaco.editor.IStandaloneCodeEditor = importMember "editor"

    let compileAndRunCurrentResults (_:unit) : string * string = importMember "editor"
// We store a reference to the editor so we can access it
// Later we will probably wrap it inside a Cmd implementation
// For now, it's good enough for some proto
let mutable editorFsharp = Unchecked.defaultof<monaco.editor.IStandaloneCodeEditor>

let mutable editorHtml = Unchecked.defaultof<monaco.editor.IStandaloneCodeEditor>

let mutable editorCode = Unchecked.defaultof<monaco.editor.IStandaloneCodeEditor>

let outputHtml =
    """
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html;charset=utf-8">
    </head>
    <body>
    </body>
</html>
    """.Trim()

type State =
    | Compiling
    | Compiled
    | NoState

type ActiveTab =
    | CodeTab
    | LiveTab

type DragTarget =
    | NoTarget
    | EditorSplitter
    | PanelSplitter

type Model =
    { State : State
      Url : string
      ActiveTab : ActiveTab
      CodeES2015: string
      CodeAMD : string
      DragTarget : DragTarget
      EditorSplitRatio : float
      PanelSplitRatio : float }

type Msg =
    | StartCompile
    | EndCompile of string * string // codeES2015, codeAMD
    | SetActiveTab of ActiveTab
    | SetUrl of string
    | EditorDragStarted
    | EditorDrag of Position
    | EditorDragEnded
    | PanelDragStarted
    | PanelDrag of Position
    | PanelDragEnded
    | MouseUp
    | MouseMove of Mouse.Position

open Elmish

let generateHtmlUrl jsCode =
    let jsUrl = Generator.generateBlobURL jsCode Generator.JavaScript
    Generator.generateHtmlBlobUrl (editorHtml.getValue()) jsUrl

let clamp min max value =
    if value >= max then
        max
    elif value <= min then
        min
    else
        value

let update msg model =
    match msg with
    | StartCompile ->
        { model with State = Compiling }, Cmd.performFunc Editor.compileAndRunCurrentResults () EndCompile

    | EndCompile (codeES2015, codeAMD) ->
        console.log("code")
        console.log codeES2015
        { model with State = Compiled
                     CodeES2015 = codeES2015
                     CodeAMD = codeAMD }, Cmd.batch [ Cmd.performFunc generateHtmlUrl codeAMD SetUrl ]

    | SetUrl newUrl ->
        { model with Url = newUrl }, Cmd.none

    | SetActiveTab newTab ->
        { model with ActiveTab = newTab }, Cmd.none

    | EditorDragStarted ->
        { model with DragTarget = EditorSplitter }, Cmd.none

    | EditorDragEnded ->
        { model with DragTarget = NoTarget } , Cmd.none

    | MouseUp ->
        let cmd =
            match model.DragTarget with
            | NoTarget -> Cmd.none
            | EditorSplitter ->
                Cmd.ofMsg EditorDragEnded
            | PanelSplitter ->
                Cmd.ofMsg PanelDragEnded

        model, cmd

    | MouseMove position ->
        let cmd =
            match model.DragTarget with
            | NoTarget -> Cmd.none
            | EditorSplitter ->
                Cmd.ofMsg (EditorDrag position)
            | PanelSplitter ->
                Cmd.ofMsg (PanelDrag position)

        model, cmd

    | EditorDrag position ->
        { model with EditorSplitRatio =
                       position
                       |> (fun p -> p.Y - 54.)
                       |> (fun h -> h / (window.innerHeight - 54.))
                       |> clamp 0.2 0.8 }, Cmd.none

    | PanelDragStarted ->
        { model with DragTarget = PanelSplitter }, Cmd.none

    | PanelDragEnded ->
        { model with DragTarget = NoTarget }, Cmd.none

    | PanelDrag position ->
        { model with PanelSplitRatio =
                        position
                        |> (fun p -> p.X)
                        |> (fun w -> w / window.innerWidth)
                        |> clamp 0.2 0.8 }, Cmd.none

let init _ = { State = NoState
               Url = ""
               ActiveTab = LiveTab
               CodeES2015 = ""
               CodeAMD = ""
               DragTarget = NoTarget
               EditorSplitRatio = 0.7
               PanelSplitRatio = 0.5 }, Cmd.batch [ Cmd.ups MouseUp
                                                    Cmd.move MouseMove
                                                    Cmd.iframeMessage MouseMove MouseUp ]

open Fable.Helpers.React
open Fable.Helpers.React.Props

let numberToPercent number =
    string (number * 100.) + "%"

let menubar isCompiling dispatch =
    let iconView =
        if isCompiling then
            Icon.faIcon [ Icon.isSmall ]
                [ Fa.icon Fa.I.Spinner
                  Fa.spin ]
        else
            Icon.faIcon [ Icon.isSmall ]
                [ Fa.icon Fa.I.Play ]

    nav [ ClassName "navbar is-fixed-top is-dark" ]
        [ Navbar.brand_div [ ]
            [ Navbar.item_div [ ]
                [ img [ Src "img/fable_ionide.png" ] ] ]
          Navbar.menu [ ]
            [ Navbar.item_div [ ]
                [ Button.button_btn [ Button.onClick (fun _ -> dispatch StartCompile) ]
                    [ iconView
                      span [ ]
                        [ str "Compile" ] ] ]
              Navbar.item_div [ Navbar.Item.props [ Style [ Color "white" ] ] ]
                [ str "You can also press Alt+Enter from the editor" ] ] ]

let editorArea model dispatch =
    let isDragging =
        match model.DragTarget with
        | EditorSplitter
        | PanelSplitter -> true
        | NoTarget -> false

    div [ ClassName "editor-container"
          Style [ Width (numberToPercent model.PanelSplitRatio) ] ]
        [ div [ Key "editor"
                ClassName "editor-fsharp"
                Style [ Height (numberToPercent model.EditorSplitRatio) ]
                OnKeyDown (fun ev ->
                  if ev.altKey && ev.key = "Enter" then
                      dispatch StartCompile
                )
                Ref (fun element ->
                      if not (isNull element) then
                        if element.childElementCount = 0. then
                            editorFsharp <- Editor.create (element :?> Browser.HTMLElement)
                        else
                            if isDragging then
                                editorFsharp.layout()
                  ) ] [ ]
          div [ ClassName "vertical-resize"
                OnMouseDown (fun _ -> dispatch EditorDragStarted) ]
              [ ]
          div [ ClassName "editor-html"
                Style [ Height (numberToPercent (1. - model.EditorSplitRatio)) ]
                Ref (fun element ->
                        if not (isNull element) then
                            if element.childElementCount = 0. then
                                let options = jsOptions<monaco.editor.IEditorConstructionOptions>(fun o ->
                                    let minimapOptions =  jsOptions<monaco.editor.IEditorMinimapOptions>(fun oMinimap ->
                                        oMinimap.enabled <- Some false
                                    )

                                    o.language <- Some "html"
                                    o.theme <- Some "vs-dark"
                                    o.value <- Some outputHtml
                                    o.minimap <- Some minimapOptions
                                )

                                editorHtml <- monaco.editor.Globals.create((element :?> Browser.HTMLElement), options)
                            else
                                if isDragging then
                                    editorHtml.layout()
                ) ]
            [ ] ]

let outputTabs (activeTab : ActiveTab) dispatch =
    Tabs.tabs [ Tabs.isCentered ]
        [ Tabs.tab [ if (activeTab = LiveTab) then
                        yield Tabs.Tab.isActive
                     yield Tabs.Tab.props [
                         OnClick (fun _ -> SetActiveTab LiveTab |> dispatch)
                     ] ]
            [ a [ ] [ str "Live sample" ] ]
          Tabs.tab [ if (activeTab = CodeTab) then
                        yield Tabs.Tab.isActive
                     yield Tabs.Tab.props [
                         OnClick (fun _ -> SetActiveTab CodeTab |> dispatch)
                     ] ]
            [ a [ ] [ str "Code" ] ] ]

let toggleDisplay cond =
    if cond then
        ""
    else
        "is-hidden"

let viewIframe isShown url =
    iframe [ Src url
             ClassName (toggleDisplay isShown) ]
        [ ]

let viewCodeEditor isShown code =
    div [ ClassName ("editor-output " + toggleDisplay isShown)
          Ref (fun element ->
                    if not (isNull element) then
                        if element.childElementCount = 0. then
                            let options = jsOptions<monaco.editor.IEditorConstructionOptions>(fun o ->
                                let minimapOptions =  jsOptions<monaco.editor.IEditorMinimapOptions>(fun oMinimap ->
                                    oMinimap.enabled <- Some false
                                )

                                o.language <- Some "javascript"
                                o.theme <- Some "vs-dark"
                                o.minimap <- Some minimapOptions
                            )

                            editorCode <- monaco.editor.Globals.create((element :?> Browser.HTMLElement), options)

                        editorCode.setValue(code)
                        // This is needed because Monaco don't itialize well when being hidden by default
                        // I believe then aren't reponsive by default and recalculate every position as needed...
                        editorCode.layout()
            ) ]
        [ ]

let outputArea model dispatch =
    let content =
        match model.State with
        | Compiling ->
            [ str "Compile in progress" ]
        | Compiled ->
            [ outputTabs model.ActiveTab dispatch
              viewIframe (model.ActiveTab = LiveTab) model.Url
              viewCodeEditor (model.ActiveTab = CodeTab) model.CodeES2015 ]
        | NoState ->
            [ str "No apps compiled yet" ]

    div [ ClassName "output-container"
          Style [ Width (numberToPercent (1. - model.PanelSplitRatio)) ] ]
        content

let view model dispatch =
    let isDragging =
        match model.DragTarget with
        | EditorSplitter
        | PanelSplitter -> true
        | NoTarget -> false

    div [ classList [ "is-unselectable", isDragging ] ]
        [ menubar (model.State = Compiling) dispatch
          div [ ClassName "page-content" ]
            [ editorArea model dispatch
              div [ ClassName "horizontal-resize"
                    OnMouseDown (fun _ -> dispatch PanelDragStarted) ]
                [ ]
              outputArea model dispatch ] ]

open Elmish.React

Program.mkProgram init update view
#if DEBUG
// |> Program.withConsoleTrace
#endif
|> Program.withReact "app-container"
|> Program.run


// open Fable.Core
// open Fable.Core.JsInterop
// open Fable.Import.Browser

// let canvas = document.querySelector(".view") :?> HTMLCanvasElement

// let ctx = canvas.getContext_2d()
// // The (!^) operator checks and casts a value to an Erased Union type
// // See http://fable.io/docs/interacting.html#Erase-attribute
// ctx.fillStyle <- !^"rgb(200,0,0)"
// ctx.fillRect (10., 10., 55., 50.)
// ctx.fillStyle <- !^"rgba(0, 0, 200, 0.5)"
// ctx.fillRect (30., 30., 55., 50.)

// <html>
//     <head>
//     </head>
//     <body>
//         <canvas class="view" width="800" height="600"></canvas>
//     </body>
// </html>
