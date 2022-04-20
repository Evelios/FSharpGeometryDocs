module App.App

open Feliz
open Feliz.Bulma
open Elmish

type State = { Count: int }

type Msg =
    | Increment
    | Decrement

let init() = { Count = 0 }, Cmd.none

let update (msg: Msg) (state: State) =
    match msg with
    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none

let view (state: State) (dispatch: Msg -> unit) =
     Bulma.hero [
        hero.isMedium
        color.isPrimary
        prop.children [
            Bulma.heroHead  []
            Bulma.heroBody [
                Bulma.text.p [
                    prop.text "FSharp Geometry Documentation"
                ]
            ]
        ]
    ]