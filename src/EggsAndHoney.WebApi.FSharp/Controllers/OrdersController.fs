namespace EggsAndHoney.WebApi.FSharp.Controllers

open System
open Microsoft.AspNetCore.Mvc
open EggsAndHoney.WebApi.FSharp.ViewModels
open EggsAndHoney.Domain.Services
open EggsAndHoney.Domain.Models

[<Route("api/v1/[controller]")>]
// TODO: Remove OrderContext as soon as the issue with default in-memory data is solved
type OrdersController (orderService: IOrderService, orderContext: OrderContext) =
    inherit Controller()

    member private this.mapOrderToViewModel (order: Order) =
        {
            id = order.Id;
            name = order.Name;
            order = order.OrderType.Name;
            datePlaced = order.DatePlaced
        }

    [<HttpGet>]
    [<ProducesResponseType(typeof<ItemCollectionResponseViewModel<OrderViewModel>>, 200)>]
    member this.Get() =
        // TODO: Make controllers async
        let orders = orderService.GetOrders().Result
        let ordersViewModels = orders |> Seq.map this.mapOrderToViewModel
        this.Ok(ordersViewModels)

    [<HttpGet("count")>]
    [<ProducesResponseType(typeof<ItemCountResponseViewModel>, 200)>]
    member this.GetCount() =
        // TODO: Make controllers async
        let numberOfOrders = orderService.GetNumberOfOrders().Result
        this.Ok({ count = numberOfOrders })
        
    [<HttpPost("add")>]
    [<ProducesResponseType(typeof<ItemIdentifierViewModel>, 201)>]
    member this.Add([<FromBody>] addOrderViewModel: AddOrderViewModel) =
        // HACK: Quickfix for now: calling it here, since adding the
        // default order types on Startup doesn't work in dotnet core 2.0
        this.ensureDefaultOrderTypesExist()
        
        // TODO: Make controllers async
        let createdOrderId = orderService.AddOrder(addOrderViewModel.name, addOrderViewModel.order).Result
        this.StatusCode(201, { id = createdOrderId })
    
    // TODO: Remove as soon as the issue with default in-memory data is solved
    member private this.ensureDefaultOrderTypesExist () =
        do
            let orderTypeSet = orderContext.Set<OrderType>()
            orderTypeSet.Add(new OrderType( Id = 1, Name = "Eggs" )) |> ignore
            orderTypeSet.Add(new OrderType( Id = 2, Name = "Honey" )) |> ignore
            orderContext.SaveChanges() |> ignore

