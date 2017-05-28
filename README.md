# EggsAndHoneyWebApi
A Web API for the Eggs&amp;Honey project written with ASP.NET Core and Entity Framework Core

Design decisions

* Use Entity Framework Code First with migrations
* Store resolved orders in a separate table, since this is historical data that has the potential to grow forever without being queried often. NOTE: this turned out to not be as straightforward to model with EF code first as one would like. Especially compared to document-based storage, such as MongoDB, or even Azure storage
* No authentication (not even for admin) - a tradeoff for the sake of ease of use
* After considering the simplest and quickest possible code, decided to go for async querying anyway, because it is easy and fun to implement ;)
