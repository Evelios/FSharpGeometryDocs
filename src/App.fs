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
                Bulma.navbarStart.div [
                    Bulma.navbarItem.a [ prop.text "Home" ]
                    Bulma.navbarItem.a [ prop.text "Documentation" ]
                ]
                Bulma.navbarEnd.div [
                    Bulma.navbarItem.a [ Icon.withText Icon.api "API" ]
                    Bulma.navbarItem.a [ Icon.withText Icon.github "Github" ]
                    Bulma.navbarItem.a [ Icon.withText Icon.nuget "NuGet Package" ]
                ]
            ]
        ]
    ]


let view (model: Model) (dispatch: Msg -> unit) : ReactElement =
    let page =
        match model.ActivePage with
        | Page.Home -> Home.view ()

    Html.div [ prop.children [ navbar (); page ] ]
