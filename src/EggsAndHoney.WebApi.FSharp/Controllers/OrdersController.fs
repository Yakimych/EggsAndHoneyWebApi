namespace EggsAndHoney.WebApi.FSharp.Controllers

open System
open Microsoft.AspNetCore.Mvc
open EggsAndHoney.WebApi.FSharp.ViewModels

[<Route("api/v1/[controller]")>]
type OrdersController () =
    inherit Controller()

    [<HttpGet>]
    member this.Get() =
        let fakeOrdersViewModel = { id = 1; name = "Rita"; order = "Honey"; datePlaced = DateTime.Now }
        let fakeOrdersViewModel' = { id = 2; name = "YaK"; order = "Eggs"; datePlaced = DateTime.Now }
        let ordersViewModels = [fakeOrdersViewModel; fakeOrdersViewModel']
        this.Ok(ordersViewModels)

    [<HttpGet("count")>]
    member this.GetCount() =
        this.Ok(4)
