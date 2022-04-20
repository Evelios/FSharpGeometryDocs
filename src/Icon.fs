module App.Icon

open Feliz
open Feliz.Bulma

type Icon = string

let private fas (text: string) : Icon = $"fas fa-{text}"
let private faBrand (text: string) : Icon = $"fab fa-{text}"

let api = fas "code"
let github = faBrand "github"
let nuget = fas "cube"

let basic (icon: string) : ReactElement =
    Bulma.icon [ Html.i [ prop.className icon ] ]

let withText (icon: Icon) (text: string) : ReactElement =
    Html.span [
        prop.className "icon-text"
        prop.children [
            Bulma.icon [
                spacing.pr4
                spacing.pl3
                prop.children [ Html.i [
                    size.isSize1
                    prop.className icon
                ] ]
            ]
            Html.span text
        ]
    ]

let textWithIcon (text: string) (icon: Icon) =
    Html.span [
        prop.className "icon-text"
        prop.children [
            Html.span text
            Bulma.icon [
                spacing.pr5
                spacing.pl3
                prop.children [ Html.i [ prop.className icon ] ]
            ]
        ]
    ]
