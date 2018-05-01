namespace EggsAndHoney.WebApi.FSharp.Controllers

open System
open Microsoft.AspNetCore.Mvc
open EggsAndHoney.WebApi.FSharp.ViewModels
open EggsAndHoney.WebApi.FSharp.Mapping
open EggsAndHoney.WebApi.FSharp.Filters
open EggsAndHoney.Domain.Services
open EggsAndHoney.Domain.Models

[<Route("api/v1/[controller]")>]
[<ValidateModel>]
type OrdersController (orderService: IOrderService) =
    inherit Controller()

    [<HttpGet>]
    [<ProducesResponseType(typeof<ItemCollectionResponseViewModel<OrderViewModel>>, 200)>]
    member this.Get() =
        async {
            let! orders = orderService.GetOrders() |> Async.AwaitTask
            return this.Ok(
                orders |>
                Seq.sortBy (fun o -> o.DatePlaced) |>
                toOrderCollectionResponseViewModel)
        }

    [<HttpGet("count")>]
    [<ProducesResponseType(typeof<ItemCountResponseViewModel>, 200)>]
    member this.GetCount() =
        async {
            let! numberOfOrders = orderService.GetNumberOfOrders() |> Async.AwaitTask
            return this.Ok({ count = numberOfOrders })
        }
        
    [<HttpPost("")>]
    [<ProducesResponseType(typeof<ItemIdentifierViewModel>, 201)>]
    [<ProducesResponseType(400)>]
    member this.Add([<FromBody>] addOrderViewModel: AddOrderViewModel) : Async<IActionResult> =
        async {
            try
                let! createdOrderId = orderService.AddOrder(addOrderViewModel.name, addOrderViewModel.order) |> Async.AwaitTask
                return this.StatusCode(201, { id = createdOrderId }) :> _
            with :? AggregateException as ex ->
                match ex.InnerException with
                | :? InvalidOperationException -> return this.BadRequest(ex.Message) :> _
                | _ -> return this.BadRequest() :> _
        }

    [<HttpPost("resolve")>]
    [<ProducesResponseType(typeof<ResolvedOrderViewModel>, 200)>]
    [<ProducesResponseType(400)>]
    [<ProducesResponseType(404)>]
    member this.Resolve([<FromBody>] itemIdentifier) : Async<IActionResult> =
        async {
            let! orderExists = orderService.OrderExists(itemIdentifier.id) |> Async.AwaitTask
            match orderExists with
            | false -> return this.NotFound() :> _
            | _ ->
                let! resolvedOrder = orderService.ResolveOrder(itemIdentifier.id) |> Async.AwaitTask
                return this.Ok(resolvedOrder |> toResolvedOrderViewModel) :> _
        }

