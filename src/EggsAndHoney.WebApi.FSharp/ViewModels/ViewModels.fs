namespace EggsAndHoney.WebApi.FSharp.ViewModels

open System

type OrderViewModel = {
    id: int
    name: string
    order: string
    datePlaced: DateTime
}

type ResolvedOrderViewModel = {
    id: int
    name: string
    order: string
    datePlaced: DateTime
    dateResolved: DateTime
}

type AddOrderViewModel = {
    name: string
    order: string
}

type ItemCollectionResponseViewModel<'a> = {
    items: list<'a>
}

type ItemCountResponseViewModel = {
    count: int
}

type ItemIdentifierViewModel = {
    id: int
}
