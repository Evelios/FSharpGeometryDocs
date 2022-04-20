module App.Page.NotFound

open Feliz.Bulma
open Feliz

let view () : ReactElement =
    Bulma.hero [
        hero.isFullHeightWithNavbar
        prop.children [
            Bulma.heroBody [
                Bulma.container [
                    Bulma.title.h1 [
                        prop.text "404: Page Not Found"
                        text.hasTextCentered
                    ]
                ]
            ]
        ]
    ]
