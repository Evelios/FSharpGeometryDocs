module App.Page.Article

open Elmish
open Fable.Core
open Feliz.Bulma
open Feliz
open Fable.Formatting.Markdown

open App


// -- Types ----

type Model =
    | Loading of Title
    | LoadedModel of LoadedModel

and LoadedModel =
    { Title: Title
      Markdown: MarkdownDocument }

type Msg =
    | Loaded of string
    | FailedToLoad of exn


// ---- Module Functions ----

let init (articleTitle: string) : Model * Cmd<Msg> =
    Loading articleTitle, Router.fetchArticle articleTitle Loaded FailedToLoad

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match model, msg with
    | Loading title, Loaded markdown ->
        LoadedModel
            { Title = title
              Markdown = Markdown.Parse(markdown) },
        Cmd.none

    | Loading title, FailedToLoad exn ->
        JS.console.warn ($"Failed to load article: {title}", string exn.Message)
        model, Cmd.none

    | _, msg ->
        JS.console.warn ("Article Message discarded:\n", string msg)
        model, Cmd.none


// ---- Markdown Views ----

let rec spanView (span: MarkdownSpan) : ReactElement =
    match span with
    | Literal (text, _) -> Bulma.text.span [ prop.text text ]

    | InlineCode (code, _) ->
        Bulma.text.span [
            prop.text code
            text.isFamilyMonospace
            Bulma.color.hasBackgroundDangerLight
        ]

    | Strong (body, _) ->
        Bulma.text.span [
            text.hasTextWeightBold
            prop.children (List.map spanView body)
        ]

    | Emphasis (body, _) ->
        Bulma.text.span [
            text.isItalic
            text.hasTextWeightLight
            prop.children (List.map spanView body)
        ]

    | AnchorLink (link, _) -> Html.div []

    | DirectLink (body, link, title, _) -> Html.div []
    | IndirectLink (body, original, key, _) -> Html.div []
    | DirectImage (body, link, title, _) -> Html.div []
    | IndirectImage (body, link, key, _) -> Html.div []

    | HardLineBreak _ -> Html.br []

    | LatexInlineMath (code, _) -> Html.div []
    | LatexDisplayMath (code, _) -> Html.div []
    | EmbedSpans (customSpans, range) -> Html.div []

let rec paragraphView paragraph : ReactElement =
    match paragraph with
    | Heading (size, body, _) ->
        let titleSize =
            match size with
            | 1 -> title.is1
            | 2 -> title.is2
            | 3 -> title.is3
            | 4 -> title.is4
            | 5 -> title.is5
            | _ -> title.is6

        Bulma.title [
            titleSize
            prop.children (List.map spanView body)
        ]

    | Paragraph (body, _) -> Bulma.section (List.map spanView body)

    /// A code block, whether fenced or via indentation
    | CodeBlock (code, executionCount, language, ignoredLine, range) -> Html.div []

    /// A HTML block
    | InlineHtmlBlock (code, executionCount, range) -> Html.div []

    /// A Markdown List block
    | ListBlock (kind, items, _) ->
        let listType : ReactElement list -> ReactElement =
            match kind with
            | Ordered -> Html.orderedList
            | Unordered -> Html.unorderedList

        let paragraphsView (paragraphs: MarkdownParagraphs) : ReactElement =
            Html.li (List.map paragraphView paragraphs)

        listType (List.map paragraphsView items)

    /// A Markdown Quote block
    | QuotedBlock (paragraphs, _) ->
        Bulma.block (List.map paragraphView paragraphs)

    /// A Markdown Span block
    | Span (spans, _) -> Html.span (List.map spanView spans)

    /// A Markdown Latex block
    | LatexBlock (env, body, range) -> Html.div []

    /// A Markdown Horizontal rule
    | HorizontalRule _ -> Html.hr []

    /// A Markdown Table
    | TableBlock (headers, alignments, rows, range) -> Html.div []

    /// Represents a block (markdown produced when parsing of code or tables or quoted blocks is suppressed
    | OtherBlock (lines, range) -> Html.div []

    /// A special addition for computing paragraphs
    | EmbedParagraphs (customParagraphs, range) -> Html.div []

    /// A special addition for YAML-style frontmatter
    | YamlFrontmatter (yaml, range) -> Html.div []

    /// A special addition for inserted outputs
    | OutputBlock (output, kind, executionCount) -> Html.div []


let articleView (document: MarkdownDocument) : ReactElement =
    Bulma.container (List.map paragraphView document.Paragraphs)


// ---- Main View ----

let header title =
    Bulma.hero [
        color.isInfo
        prop.children [
            Bulma.heroBody [
                Bulma.title.h1 [
                    prop.text $"Article: {title}"
                    text.hasTextCentered
                ]
            ]
        ]
    ]

let view (model: Model) (dispatch: Msg -> unit) : ReactElement =
    match model with
    | Loading title -> header title

    | LoadedModel loadedModel ->
        let body = articleView loadedModel.Markdown

        Html.div [ header loadedModel.Title; body ]
