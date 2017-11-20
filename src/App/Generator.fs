[<RequireQualifiedAccess>]
module Generator

open Fable.Core.JsInterop
open Fable.Import.Browser

let private requireConfig =
    """
<body>
    <script src="http://localhost:8080/libs/requirejs/require.js"></script>
    <script>
        require.config({
        paths: {
            'fable-core': 'http://localhost:8080/build/fable-core'
        }
        });
    </script>
    """.Trim()

let private bundleScriptTag url = sprintf "<script src=\"%s\"></script>\n</body>" url

type MimeType =
    | Html
    | JavaScript

let generateBlobURL content mimeType =
    let parts = [ content ] |> unbox<ResizeArray<obj>>

    let options =
        jsOptions<BlobPropertyBag>(fun o ->
            o.``type`` <-
                match mimeType with
                | Html -> Some "text/html"
                | JavaScript -> Some "text/javascript"
        )

    Blob.Create(parts, options)
    |> URL.createObjectURL

let generateHtmlBlobUrl (htmlCode : string) (scriptUrl : string) =
    let code =
        htmlCode
            .Replace("<body>", requireConfig)
            .Replace("</body>", bundleScriptTag scriptUrl)
    generateBlobURL code Html
