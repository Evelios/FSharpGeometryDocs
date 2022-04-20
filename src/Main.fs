module Main

open Fable.Core.JsInterop

open Elmish
open Elmish.Navigation
open Elmish.React

open App

#if DEBUG
//open Elmish.Debug
//open Elmish.HMR
#endif

Program.mkProgram App.init App.update App.view
#if DEBUG
//|> Program.withConsoleTrace
#endif
|> Program.toNavigable (UrlParser.parseHash Router.routeParser) App.setRoute
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
//|> Program.withDebugger
#endif
|> Program.run