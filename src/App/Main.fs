module Main

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fulma.Layouts
open Fulma.Components
open Fulma.BulmaClasses
open Fulma.Elements
open Fulma.Extra.FontAwesome
open Fable.Import.Browser

importSideEffects "./scss/main.scss"

module Editor =
    let create(element: Browser.HTMLElement): monaco.editor.IStandaloneCodeEditor = importMember "editor"

    let compileAndRunCurrentResults (_:unit) : string = importMember "editor"
// We store a reference to the editor so we can access it
// Later we will probably wrap it inside a Cmd implementation
// For now, it's good enough for some proto
let mutable editor = Unchecked.defaultof<monaco.editor.IStandaloneCodeEditor>

type Model =
    { IsCompiling : bool
      IFrameUrl : string option }

type Msg =
    | StartCompile
    | EndCompile of string

open Elmish

let update msg model =
    match msg with
    | StartCompile ->
        { model with IsCompiling = true }, Cmd.performFunc Editor.compileAndRunCurrentResults () EndCompile

    | EndCompile url ->
        { model with IsCompiling = false
                     IFrameUrl = Some url }, Cmd.none

let init _ = { IsCompiling = false
               IFrameUrl = None }, Cmd.none

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma.BulmaClasses.Bulma
open Fable.Import.Browser
open System.Text.RegularExpressions

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
                        [ str "Compile" ] ] ] ] ]

let editorArea dispatch =
    div [ ClassName "editor-container"
          Key "editor"
          OnKeyDown (fun ev ->
            if ev.altKey && ev.key = "Enter" then
                dispatch StartCompile
          )
          Ref (fun element ->
                if not (isNull element) && element.childElementCount = 0. then
                    editor <- Editor.create (element :?> Browser.HTMLElement)
                    window?ed <- editor
            ) ]
        [ ]

let outputArea (optUrl : string option) =
    match optUrl with
    | Some url ->
        div [ ClassName "output-container" ]
            [ iframe [ Src url ]
                [ ] ]
    | None -> str "No url yet for the iframe"

let view model dispatch =
    div [ ]
        [ menubar model.IsCompiling dispatch
          div [ ClassName "page-content" ]
            [ editorArea dispatch
              outputArea model.IFrameUrl ] ]

open Elmish.React
open Elmish.Debug
open Elmish.HMR

Program.mkProgram init update view
#if DEBUG
|> Program.withHMR
|> Program.withDebugger
#endif
|> Program.withReact "app-container"
|> Program.run


// open Fable.Core
// open Fable.Core.JsInterop
// open Fable.Import.Browser

// let canvas = document.createElement_canvas()
// document.body.appendChild(canvas) |> ignore

// canvas.width <- 800.
// canvas.height <- 600.

// canvas.style.width <- "800px"
// canvas.style.height <- "600px"

// let ctx = canvas.getContext_2d()
// // The (!^) operator checks and casts a value to an Erased Union type
// // See http://fable.io/docs/interacting.html#Erase-attribute
// ctx.fillStyle <- !^"rgb(200,0,0)"
// ctx.fillRect (10., 10., 55., 50.)
// ctx.fillStyle <- !^"rgba(0, 0, 200, 0.5)"
// ctx.fillRect (30., 30., 55., 50.)
