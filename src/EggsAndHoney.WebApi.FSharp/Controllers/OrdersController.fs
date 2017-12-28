namespace EggsAndHoney.WebApi.FSharp.Controllers

open System
open Microsoft.AspNetCore.Mvc
open EggsAndHoney.WebApi.FSharp.ViewModels

[<Route("api/v1/[controller]")>]
type OrdersController () =
    inherit Controller()

    [<HttpGet>]
    member this.Get() =
        let fakeOrdersViewModel = { Id = 1; Name = "Rita"; Order = "Honey"; DatePlaced = DateTime.Now }
        let fakeOrdersViewModel' = { Id = 2; Name = "YaK"; Order = "Eggs"; DatePlaced = DateTime.Now }
        let ordersViewModels = [fakeOrdersViewModel; fakeOrdersViewModel']
        this.Ok(ordersViewModels)

    [<HttpGet("count")>]
    member this.GetCount() =
        this.Ok(4)
