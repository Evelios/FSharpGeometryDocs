module App.App

open Elmish
open Fable.React

open App.Page

[<RequireQualifiedAccess>]
type Page = | Home

type Model = { ActivePage: Page }

type Msg = | NoMsg

let init () = { ActivePage = Page.Home }, Cmd.none

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | NoMsg -> model, Cmd.none

let view (model: Model) (dispatch: Msg -> unit) : ReactElement=
    match model.ActivePage with
    | Page.Home -> Home.view ()
