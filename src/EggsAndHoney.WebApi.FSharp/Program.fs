namespace EggsAndHoney.WebApi.FSharp

open System
open System.IO
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open EggsAndHoney.Domain.Models
open Microsoft.Extensions.DependencyInjection

module Program =
    let exitCode = 0
    
    let ensureInMemoryDataExists (serviceProvider: IServiceProvider) =
        do
            // TODO: Do this only if the UseInMemoryDatabase config setting is true
            use context = serviceProvider.GetService<OrderContext>()
            let orderTypeSet = context.Set<OrderType>()
            
            orderTypeSet.Add (new OrderType (Id = 1, Name = "Eggs")) |> ignore
            orderTypeSet.Add (new OrderType (Id = 2, Name = "Honey")) |> ignore
            context.SaveChanges() |> ignore

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build()

    [<EntryPoint>]
    let main args =
        let webHost = BuildWebHost(args)
        
        ensureInMemoryDataExists webHost.Services |> ignore
        
        webHost.Run()

        exitCode
