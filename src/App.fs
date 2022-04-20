module App.App

open Elmish
open Fable.Core
open Fable.React
open Feliz.Bulma
open Feliz.Router
open Feliz

open App.Page

[<RequireQualifiedAccess>]
type Page =
    | Home
    | Article of Article.Model
    | NotFound

type Model =
    { CurrentRoute: Router.Route option
      ActivePage: Page }

type Msg =
    | NavigateTo of Router.Route
    | ArticleMsg of Article.Msg

let rec setRoute (optRoute: Router.Route option) model =
    let model = { model with CurrentRoute = optRoute }
    let navigate = Router.navigateTo optRoute

    match optRoute with
    | None ->
        { model with
              ActivePage = Page.NotFound },
        Cmd.none
    | Some route ->
        match route with
        | Router.Route.Home -> { model with ActivePage = Page.Home }, navigate

        | Router.Route.Article title ->
            let articleModel, articleCmd = (Article.init title)

            { model with
                  ActivePage = Page.Article articleModel },
            Cmd.batch [ Cmd.map ArticleMsg articleCmd; navigate ]

let init (location: Router.Route option) =
    setRoute
        location
        { CurrentRoute = None
          ActivePage = Page.Home }

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match model.ActivePage, msg with

    // ---- High Level Updates ----

    | _, NavigateTo route -> setRoute (Some route) model


    // ---- Page Updates ----

    | Page.Article articleModel, ArticleMsg articleMsg ->
        let newArticleModel, articleCmd = Article.update articleMsg articleModel

        { model with
              ActivePage = Page.Article newArticleModel },
        Cmd.map ArticleMsg articleCmd


    // ---- Default Catch All ----
    | _, msg ->
        JS.console.warn ("Message discarded:\n", string msg)
        model, Cmd.none

let navbar dispatch =
    let internalLinks =
        [ "Home", Router.Home
          "Documentation", Router.Article "Documentation" ]

    let externalLinks =
        [ "API", Icon.api, "https://evelios.github.io/fsharp-geometry/reference/index.html"
          "Source Code", Icon.github, "https://github.com/Evelios/fsharp-geometry"
          "NuGet Package", Icon.nuget, "https://www.nuget.org/packages/Fsharp.Geometry/" ]

    let navbarStartItem (name: string, route: Router.Route) =
        Bulma.navbarItem.a [
            prop.onClick (fun _ -> NavigateTo route |> dispatch)
            prop.text name
        ]

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
        | Page.Article articleModel -> Article.view articleModel (ArticleMsg >> dispatch)
        | Page.NotFound -> NotFound.view ()

    Html.div [ prop.children [ navbar dispatch; page ] ]
