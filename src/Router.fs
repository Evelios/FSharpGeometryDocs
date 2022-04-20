module App.Router

open Elmish
open Elmish.Navigation

open Fetch

open App

type Route =
    | Home
    | Article of Title


// ---- Url Creation ----

let inline (</>) a b = a + "/" + string b

let toHash route =
    "/#"
    </> match route with
        | Home -> ""
        | Article title -> "article" </> title

// ---- Navigation ----

let getRequest (url: string) (onSuccess: string -> 'Msg) (onFail: exn -> 'Msg) : Cmd<'Msg> =
    let promise () =
        fetch url [ ]
        |> Promise.bind (fun res -> res.text())
        
    Cmd.OfPromise.either promise () onSuccess onFail
     
let fetchArticle (title: Title) (onSuccess: string -> 'Msg) (onFail: exn -> 'Msg) : Cmd<'Msg> =
    getRequest $"/articles/{title}.md" onSuccess onFail


let navigateTo routeOpt =
    match routeOpt with
    | Some route -> Navigation.modifyUrl (toHash route)
    | None -> Cmd.none

// ---- Url Parsing ----

open Elmish.UrlParser

let routeParser : Parser<Route -> Route, Route> =
    oneOf [ map Home top; map Article (s "article" </> str) ]

let mapCharacters (s: string) = s.Replace("%20", " ")
