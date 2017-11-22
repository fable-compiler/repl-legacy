module Mouse

open Fable.Import.Browser
open System

type Position =
    { X : float
      Y : float }

[<RequireQualifiedAccess>]
module Cmd =

    let ups messageCtor =
        let handler dispatch =

            document.addEventListener_mouseup(Func<_, _>(fun ev -> messageCtor ev |> dispatch; null))

        [ handler ]

    let move messageCtor =
        let handler dispatch =

            document.addEventListener_mousemove(Func<_, _>(fun ev -> messageCtor ev |> dispatch; null))

        [ handler ]
