module App.Router

open Elmish
open Elmish.Navigation

open App

type Route =
    | Home
    | Article of Title

let inline (</>) a b = a + "/" + string b

let toHash route =
    "/#"
    </> match route with
        | Home -> ""
        | Article title -> "article" </> title

let navigateTo routeOpt =
    match routeOpt with
    | Some route -> Navigation.modifyUrl (toHash route)
    | None -> Cmd.none

open Elmish.UrlParser

let routeParser : Parser<Route -> Route, Route> =
    oneOf [ map Home top; map Article (s "article" </> str) ]

let mapCharacters (s: string) = s.Replace("%20", " ")
