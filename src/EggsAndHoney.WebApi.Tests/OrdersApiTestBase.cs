using System;
using System.Text;
using EggsAndHoney.WebApi.FSharp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace EggsAndHoney.WebApi.Tests
{
    public class OrdersApiTestBase : IDisposable
    {
        protected readonly TestServer _server;
        protected readonly System.Net.Http.HttpClient _client;

        protected const string __postContentType = "application/json";
        protected readonly Encoding _defaultEncoding = Encoding.UTF8;

        protected const string __apiPrefix = "/api/v1";
        protected static readonly string _ordersEndpoint = $"{__apiPrefix}/orders";
        protected static readonly string _ordersCountEndpoint = $"{__apiPrefix}/orders/count";
        protected static readonly string _resolvedOrdersEndpoint = $"{__apiPrefix}/resolvedorders";
        protected static readonly string _addOrderEndpoint = $"{_ordersEndpoint}/add";
        protected static readonly string _resolveOrderEndpoint = $"{_ordersEndpoint}/resolve";
        protected static readonly string _unresolveOrderEndpoint = $"{_resolvedOrdersEndpoint}/unresolve";

        public OrdersApiTestBase()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _server.Dispose();
            _client.Dispose();
        }
    }
}
