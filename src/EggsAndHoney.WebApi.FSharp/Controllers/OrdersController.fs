namespace EggsAndHoney.WebApi.FSharp.Controllers

open System
open Microsoft.AspNetCore.Mvc
open EggsAndHoney.WebApi.FSharp.ViewModels
open EggsAndHoney.Domain.Services
open EggsAndHoney.Domain.Models

[<Route("api/v1/[controller]")>]
type OrdersController (orderService: IOrderService) =
    inherit Controller()

    member private this.mapOrderToViewModel (order: Order) =
        {
            id = order.Id;
            name = order.Name;
            order = order.OrderType.ToString();
            datePlaced = order.DatePlaced
        }

    [<HttpGet>]
    [<ProducesResponseType(typeof<ItemCollectionResponseViewModel<OrderViewModel>>, 200)>]
    member this.Get() =
        let orders = orderService.GetOrders().Result
        let ordersViewModels = orders |> Seq.map this.mapOrderToViewModel
        this.Ok(ordersViewModels)

    [<HttpGet("count")>]
    [<ProducesResponseType(typeof<ItemCountResponseViewModel>, 200)>]
    member this.GetCount() =
        let numberOfOrders = orderService.GetNumberOfOrders().Result
        this.Ok({ count = numberOfOrders })
