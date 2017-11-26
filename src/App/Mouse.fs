module Mouse

open Fable.Import.Browser
open Fable.Core.JsInterop
open System

type Position =
    { X : float
      Y : float }

[<RequireQualifiedAccess>]
module Cmd =

    let ups messageCtor =
        let handler dispatch =

            window.addEventListener_mouseup(Func<_, _>(fun ev ->
                dispatch messageCtor
                null
            ))

        [ handler ]

    let move messageCtor =
        let handler dispatch =

            window.addEventListener_mousemove(Func<_, _>(fun ev ->
                { X = ev.pageX
                  Y = ev.pageY }
                |> messageCtor
                |> dispatch
                null
            ))

        [ handler ]

    let iframeMessage moveCtor upCtor =
        let handler dispatch =

            window.addEventListener_message(Func<_,_>(fun ev ->
                let typ = ev.data?("type") |> unbox<string>
                match typ with
                | "mousemove" ->
                    { X = unbox ev.data?x
                      Y = unbox ev.data?y }
                    |> moveCtor
                    |> dispatch
                | "mouseup" ->
                    dispatch upCtor
                | _ -> ()
                null
            ))

        [ handler ]
