namespace EggsAndHoney.WebApi.FSharp.ViewModels

open System

type OrderViewModel = {
    Id: int
    Name: string
    Order: string
    DatePlaced: DateTime
}

type ResolvedOrderViewModel = {
    Id: int
    Name: string
    Order: string
    DatePlaced: DateTime
    DateResolved: DateTime
}

type ItemCollectionResponseViewModel<'a> = {
    items: list<'a>
}

type ItemCountResponseViewModel = {
    count: int
}
