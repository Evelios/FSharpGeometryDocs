module App.Page.Home

open Feliz.Bulma
open Feliz

let view () : ReactElement =
    Bulma.hero [
        color.isPrimary
        prop.children [
            Bulma.heroHead []
            Bulma.heroBody [
                Bulma.title.h1 [
                    prop.text "FSharp Geometry Documentation"
                    text.hasTextCentered
                ]
            ]
        ]
    ]
