module Main

open App
open Fable.Core.JsInterop

//importSideEffects "./styles/global.scss"

open Elmish
open Elmish.React

#if DEBUG
//open Elmish.Debug
//open Elmish.HMR
#endif

Program.mkProgram App.init App.update App.view
#if DEBUG
//|> Program.withConsoleTrace
#endif
//|> Program.toNavigable (UrlParser.parseHash Router.routeParser) App.setRoute
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
//|> Program.withDebugger
#endif
|> Program.run