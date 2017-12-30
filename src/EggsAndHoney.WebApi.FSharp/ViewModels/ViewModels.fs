namespace EggsAndHoney.WebApi.FSharp.ViewModels

open System
open System.ComponentModel.DataAnnotations

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
    [<Required; MaxLength(50)>]
    name: string
    
    [<Required; MaxLength(50)>]
    order: string
}

type ItemCollectionResponseViewModel<'a> = {
    items: list<'a>
}

type ItemCountResponseViewModel = {
    count: int
}

type ItemIdentifierViewModel = {
    [<Required; Range(1, Int32.MaxValue)>]
    id: int
}
