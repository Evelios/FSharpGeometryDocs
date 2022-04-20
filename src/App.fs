module App.App

open Elmish
open Fable.React
open Feliz.Bulma
open Feliz

open App.Page

[<RequireQualifiedAccess>]
type Page = | Home

type Model = { ActivePage: Page }

type Msg = | NoMsg

let init () = { ActivePage = Page.Home }, Cmd.none

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | NoMsg -> model, Cmd.none

let navbar () =
    let internalLinks = [ "Home"; "Documentation" ]

    let externalLinks =
        [ "API", Icon.api, "https://evelios.github.io/fsharp-geometry/reference/index.html"
          "Source Code", Icon.github, "https://github.com/Evelios/fsharp-geometry"
          "NuGet Package", Icon.nuget, "https://www.nuget.org/packages/Fsharp.Geometry/" ]

    let navbarStartItem (name: string) = Bulma.navbarItem.a [ prop.text name ]

    let navbarEndItem (name: string, icon: Icon.Icon, url: string) =
        Bulma.navbarItem.a [
            prop.target.blank
            prop.href url
            prop.children [ Icon.withText icon name ]
        ]

    Bulma.navbar [
        Bulma.color.isPrimary
        prop.children [
            Bulma.navbarBrand.div [
                Bulma.navbarItem.a [
                    Html.img [
                        prop.src "https://bulma.io/images/bulma-logo-white.png"
                        prop.height 28
                        prop.width 112
                    ]
                ]
            ]
            Bulma.navbarMenu [
                Bulma.navbarStart.div (List.map navbarStartItem internalLinks)
                Bulma.navbarEnd.div (List.map navbarEndItem externalLinks)
            ]
        ]
    ]


let view (model: Model) (dispatch: Msg -> unit) : ReactElement =
    let page =
        match model.ActivePage with
        | Page.Home -> Home.view ()

    Html.div [ prop.children [ navbar (); page ] ]
