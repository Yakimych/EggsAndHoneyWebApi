namespace EggsAndHoney.WebApi.FSharp.Controllers

open System
open Microsoft.AspNetCore.Mvc
open EggsAndHoney.WebApi.FSharp.ViewModels
open EggsAndHoney.WebApi.FSharp.Mapping
open EggsAndHoney.Domain.Services
open EggsAndHoney.Domain.Models

// TODO: Add model validation attribute in order to get the rest of the tests to pass
[<Route("api/v1/[controller]")>]
type ResolvedOrdersController (orderService: IOrderService) =
    inherit Controller()

    [<HttpGet>]
    [<ProducesResponseType(typeof<ItemCollectionResponseViewModel<OrderViewModel>>, 200)>]
    member this.Get() =
        async {
            let! resolvedOrders = orderService.GetResolvedOrders() |> Async.AwaitTask
            return this.Ok(
                resolvedOrders |>
                Seq.sortByDescending (fun o -> o.DateResolved) |>
                toResolvedOrderCollectionResponseViewModel)
        }

    [<HttpPost("unresolve")>]
    [<ProducesResponseType(typeof<OrderViewModel>, 200)>]
    [<ProducesResponseType(400)>]
    [<ProducesResponseType(404)>]
    member this.Unresolve([<FromBody>] itemIdentifier) : Async<IActionResult> =
        async {
            let! resolvedOrderExists = orderService.ResolvedOrderExists(itemIdentifier.id) |> Async.AwaitTask
            match resolvedOrderExists with
            | false -> return this.NotFound() :> _
            | _ ->
                let! unresolvedOrder = orderService.UnresolveOrder(itemIdentifier.id) |> Async.AwaitTask
                return this.Ok(unresolvedOrder |> toOrderViewModel) :> _
        }

