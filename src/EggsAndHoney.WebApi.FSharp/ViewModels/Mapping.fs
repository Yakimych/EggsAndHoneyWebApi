module EggsAndHoney.WebApi.FSharp.Mapping

open EggsAndHoney.WebApi.FSharp.ViewModels
open EggsAndHoney.Domain.Models

let toResolvedOrderViewModel (resolvedOrder: ResolvedOrder) =
    {
        id = resolvedOrder.Id
        name = resolvedOrder.Name
        order = resolvedOrder.OrderType.Name
        datePlaced = resolvedOrder.DatePlaced
        dateResolved = resolvedOrder.DateResolved
    }

let toOrderViewModel (order: Order) =
    {
        id = order.Id
        name = order.Name
        order = order.OrderType.Name
        datePlaced = order.DatePlaced
    }

let mapToList f s = s |> Seq.map f |> Seq.toList

let toOrderCollectionResponseViewModel (orders: Order seq) =
    { items = orders |> mapToList toOrderViewModel }

let toResolvedOrderCollectionResponseViewModel (resolvedOrders: ResolvedOrder seq) =
    { items = resolvedOrders |> mapToList toResolvedOrderViewModel }
