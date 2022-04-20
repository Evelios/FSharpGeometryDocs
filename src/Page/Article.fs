module App.Page.Article

open Elmish
open Fable.Core
open Feliz.Bulma
open Feliz
open Fable.Formatting.Markdown

open App


// -- Types ----

type Model =
    | Loading of Title
    | LoadedModel of LoadedModel

and LoadedModel = { Title: Title; Markdown: string }

type Msg =
    | Loaded of string
    | FailedToLoad of exn


// ---- Module Functions ----

let init (articleTitle: string) : Model * Cmd<Msg> =
    Loading articleTitle, Router.fetchArticle articleTitle Loaded FailedToLoad

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match model, msg with
    | Loading title, Loaded markdown -> LoadedModel { Title = title; Markdown = markdown }, Cmd.none

    | Loading title, FailedToLoad exn ->
        JS.console.warn ("Failed to load article:", string exn.Message)
        model, Cmd.none

    | _, msg ->
        JS.console.warn ("Article Message discarded:\n", string msg)
        model, Cmd.none


// ---- Markdown Views ----

let articleView (source: string) =
    Html.div [
        prop.dangerouslySetInnerHTML (Markdown.ToHtml source)
    ]


// ---- Main View ----

let header title =
    Bulma.hero [
        color.isInfo
        prop.children [
            Bulma.heroBody [
                Bulma.title.h1 [
                    prop.text $"Article: {title}"
                    text.hasTextCentered
                ]
            ]
        ]
    ]

let view (model: Model) (dispatch: Msg -> unit) : ReactElement =
    match model with
    | Loading title -> header title

    | LoadedModel loadedModel ->
        let body = articleView loadedModel.Markdown

        Html.div [
            header loadedModel.Title
            Bulma.section [ body ]
        ]
