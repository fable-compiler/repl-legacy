module Mouse

open Fable.Import.Browser
open System
open Thot.Json

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
                let iframeMessageDecoder =
                    Decode.field "type" Decode.string
                    |> Decode.andThen
                        (function
                        | "mousemove" ->
                            Decode.decode
                                (fun x y ->
                                    { X = x
                                      Y = y })
                                |> Decode.required "x" Decode.float
                                |> Decode.required "y" Decode.float
                                |> Decode.map moveCtor

                        | "mouseup" ->
                            Decode.succeed upCtor
                        | unkown ->
                            // Discard the message we don't know how to handle it
                            sprintf "Invalid message `%s` from iframe" unkown
                            |> Decode.fail
                        )

                iframeMessageDecoder ev.data
                |> function
                    | Ok msg ->
                        dispatch msg
                    | Error error ->
                        console.warn error

                null
            ))

        [ handler ]

    let compile (obs: IObservable<string*string>) messageCtor =
        let handler dispatch =
            obs |> Observable.add (messageCtor >> dispatch)
        [ handler ]
