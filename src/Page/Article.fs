module App.Page.Article

open Feliz.Bulma
open Feliz
open Fable.Formatting.Markdown

let document = """
# F# Hello world
Hello world in [F#](http://fsharp.net) looks like this:

    printfn "Hello world!"

For more see [fsharp.org][fsorg].

  [fsorg]: http://fsharp.org "The F# organization." """

// ---- Markdown Views ----

let articleView (source: string) =
    Html.div [
        prop.dangerouslySetInnerHTML (Markdown.ToHtml source)
    ]


// ---- Main View ----

let view (title: string) : ReactElement =
    let body = articleView document

    Bulma.hero [
        color.isInfo
        prop.children [
            Bulma.heroHead []
            Bulma.heroBody [
                Bulma.title.h1 [
                    prop.text $"Article: {title}"
                    text.hasTextCentered
                ]
                body
            ]
        ]
    ]
