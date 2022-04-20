module App.Page.Article

open Feliz.Bulma
open Feliz

let view (title: string) : ReactElement =
    Bulma.hero [
        color.isInfo
        prop.children [
            Bulma.heroHead []
            Bulma.heroBody [
                Bulma.title.h1 [
                    prop.text $"Article: {title}"
                    text.hasTextCentered
                ]
            ]
        ]
    ]

