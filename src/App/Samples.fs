module Samples

open Fable.Core.JsInterop
open Thot.Json
open Fulma.Components
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma.Common

type FSharpSource =
    | FromUrl of string
    | FromCode of string

type HtmlSource =
    | Default
    | Custom of string

type SampleInfo =
    { FSharpCode : FSharpSource
      HtmlCode : HtmlSource }

type CategoryInfo =
    { Label : string
      Children : SampleType list }

and SimpleInfo =
    { Label : string
      Sample : SampleInfo }

and SubCategoryInfo =
    { Label : string
      Children : SampleType list }

and SampleType =
    | Category of CategoryInfo
    | SubCategory of SubCategoryInfo
    | Simple of SimpleInfo

let samples =
    [ Category
        { Label = "Learn Fable"
          Children =
              [ Simple
                    { Label = "Dashboard"
                      Sample =
                        { FSharpCode = FromUrl ""
                          HtmlCode = Default
                        }
                    }
                Simple
                    { Label = "Customers"
                      Sample =
                        { FSharpCode = FromUrl ""
                          HtmlCode = Default
                        }
                    }
                SubCategory
                    { Label = "Manage your team"
                      Children =
                        [ Simple
                            { Label = "Customers"
                              Sample =
                                { FSharpCode = FromUrl ""
                                  HtmlCode = Default
                                }
                            }
                          Simple
                            { Label = "Customers"
                              Sample =
                                { FSharpCode = FromUrl ""
                                  HtmlCode = Default
                                }
                            }
                          Simple
                            { Label = "Customers"
                              Sample =
                                { FSharpCode = FromUrl ""
                                  HtmlCode = Default
                                }
                            }
                        ]
                    }
                ]
        }
      Category
        { Label = "Samples"
          Children =
              [ Simple
                    { Label = "Dashboard"
                      Sample =
                        { FSharpCode = FromUrl ""
                          HtmlCode = Default
                        }
                    }
                Simple
                    { Label = "Customers"
                      Sample =
                        { FSharpCode = FromUrl ""
                          HtmlCode = Default
                        }
                    }
                SubCategory
                    { Label = "Manage your team"
                      Children =
                        [ Simple
                            { Label = "Customers"
                              Sample =
                                { FSharpCode = FromUrl ""
                                  HtmlCode = Default
                                }
                            }
                          Simple
                            { Label = "Customers"
                              Sample =
                                { FSharpCode = FromUrl ""
                                  HtmlCode = Default
                                }
                            }
                          Simple
                            { Label = "Customers"
                              Sample =
                                { FSharpCode = FromUrl ""
                                  HtmlCode = Default
                                }
                            }
                        ]
                    }
                ]
        }
    ]

let inline genKey key = Props [ Key key ]

let menuItem txt =
    li [ ]
       [ a [ ] [ str txt ] ]

let subMenu label children =
    li []
       [ a [ ]
                 [ str label ]
         ul [ ] children ]

let rec render (sample : SampleType) =
    let renderCategory (info : CategoryInfo) =
        [ Menu.label [ genKey "label" ]
            [ str info.Label ]
          Menu.list [ genKey "list" ]
            (info.Children |> List.map render) ]
        |> ofList

    match sample with
    | Category info ->
        renderCategory info
    | SubCategory info ->
        subMenu info.Label (info.Children |> List.map render)
    | Simple info ->
        menuItem info.Label

let view _ =
    Menu.menu [ ]
        (samples |> List.map render)