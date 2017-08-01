using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EggsAndHoney.WebApi.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace EggsAndHoney.WebApi.Tests
{
    public class OrdersTests : OrdersApiTestBase
    {
        [Fact]
        public async Task GettingAllOrders_ShouldReturnNonEmptyData()
        {
            var response = await _client.GetAsync(_ordersEndpoint);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(responseString);
        }

        [Fact]
        public async Task AddedOrder_ShouldBeInTheList_FetchedByGet()
        {
            var orderName = Guid.NewGuid().ToString();
            var orderType = "Eggs";

            var addedOrderId = await AddOrderAndGetId(orderName, orderType);
            var fetchedOrders = await GetOrders();

            AssertOrderIsInList(fetchedOrders, addedOrderId, orderName, orderType);
        }

        [Fact]
        public async Task ResolvedOrder_ShouldDisappearFromOrderList_AndAppearInResolvedList()
        {
            var orderName = Guid.NewGuid().ToString();
            var orderType = "Honey";

            var addedOrderId = await AddOrderAndGetId(orderName, orderType);
            var resolvedOrderId = await ResolveOrderAndGetId(addedOrderId);

            var fetchedOrders = await GetOrders();
            var fetchedResolvedOrders = await GetResolvedOrders();

            AssertOrderIsNotInList(fetchedOrders, addedOrderId, orderName, orderType);
            AssertResolvedOrderIsInList(fetchedResolvedOrders, resolvedOrderId, orderName, orderType);
        }

        [Fact]
        public async Task UnresolvedOrder_ShouldDisappearFromResolvedOrderList_AndAppearInOrderList()
        {
            var orderName = Guid.NewGuid().ToString();
            var orderType = "Eggs";

            var addedOrderId = await AddOrderAndGetId(orderName, orderType);
            var resolvedOrderId = await ResolveOrderAndGetId(addedOrderId);
            var unresolvedOrderId = await UnresolveOrderAndGetId(resolvedOrderId);

            var fetchedOrders = await GetOrders();
            var fetchedResolvedOrders = await GetResolvedOrders();

            AssertResolvedOrderIsNotInList(fetchedResolvedOrders, resolvedOrderId, orderName, orderType);
            AssertOrderIsInList(fetchedOrders, unresolvedOrderId, orderName, orderType);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task OrdersCount_ShouldIncreaseBy_TheNumberOfAddedOrders(int numberOfOrdersToAdd)
        {
            var initialNumberOfOrders = await GetOrdersCount();

            for (var i = 0; i < numberOfOrdersToAdd; i++)
            {
                var orderName = Guid.NewGuid().ToString();
                var orderType = "Eggs";

                await AddOrderAndGetId(orderName, orderType);
            }

            var finalNumberOfOrders = await GetOrdersCount();
            var numberOfAddedOrders = finalNumberOfOrders - initialNumberOfOrders;

            Assert.Equal(numberOfOrdersToAdd, numberOfAddedOrders);
        }

        [Fact]
        public async Task GetOrders_ShouldReturnOrders_OrderedByDate()
        {
            var numberOfOrdersToAdd = 10;

            for (var i = 0; i < numberOfOrdersToAdd; i++)
            {
                var orderName = Guid.NewGuid().ToString();
                var orderType = "Honey";

                await AddOrderAndGetId(orderName, orderType);
            }

            var fetchedOrders = await GetOrders();
            var fetchedOrdersInExpectedOrder = fetchedOrders.OrderBy(o => o.DatePlaced).ToList();

            Assert.Equal(fetchedOrdersInExpectedOrder, fetchedOrders);
        }

        [Fact]
        public async Task GetResolvedOrders_ShouldReturnOrders_OrderedByDate()
        {
            var numberOfOrdersToAdd = 10;
            var addedOrderIds = new List<int>();

            for (var i = 0; i < numberOfOrdersToAdd; i++)
            {
                var orderName = Guid.NewGuid().ToString();
                var orderType = "Eggs";

                var addedOrderId = await AddOrderAndGetId(orderName, orderType);
                addedOrderIds.Add(addedOrderId);
            }

            // Resolve orders in reverse order just to make sure they are not sorted by datePlaced on the server
            for (var i = addedOrderIds.Count() - 1; i >= 0; i--)
            {
                await ResolveOrderAndGetId(addedOrderIds[i]);
            }

            var fetchedResolvedOrders = await GetResolvedOrders();
            var fetchedResolvedOrdersInExpectedOrder = fetchedResolvedOrders.OrderByDescending(o => o.DateResolved).ToList();

            Assert.Equal(fetchedResolvedOrdersInExpectedOrder, fetchedResolvedOrders);
        }

        /* Helper methods */
        private void AssertOrderIsNotInList(IList<OrderViewModel> orderList, int id, string name, string order)
        {
            Assert.False(orderList.Any(o => o.Id == id && o.Name == name && o.Order == order));
        }

        private void AssertOrderIsInList(IList<OrderViewModel> orderList, int id, string name, string order)
        {
            Assert.True(orderList.Single(o => o.Name == name && o.Order == order).Id == id);
        }

        private void AssertResolvedOrderIsNotInList(IList<ResolvedOrderViewModel> resolvedOrderList, int id, string name, string order)
        {
            Assert.False(resolvedOrderList.Any(o => o.Id == id && o.Name == name && o.Order == order));
        }

        private void AssertResolvedOrderIsInList(IList<ResolvedOrderViewModel> resolvedOrderList, int id, string name, string order)
        {
            Assert.True(resolvedOrderList.Single(o => o.Name == name && o.Order == order).Id == id);
        }

        private async Task<int> AddOrderAndGetId(string name, string order)
        {
            var jsonString = JsonConvert.SerializeObject(new { name, order });

            var postContent = new StringContent(jsonString, _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_addOrderEndpoint, postContent);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var createdOrderViewModel = JsonConvert.DeserializeObject<dynamic>(responseBody);

            return createdOrderViewModel.id;
        }

        private async Task<int> ResolveOrderAndGetId(int id)
        {
            var jsonString = JsonConvert.SerializeObject(new { id });

            var postContent = new StringContent(jsonString, _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_resolveOrderEndpoint, postContent);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var resolvedOrderViewModel = JsonConvert.DeserializeObject<ResolvedOrderViewModel>(responseBody);

            return resolvedOrderViewModel.Id;
        }

        private async Task<int> UnresolveOrderAndGetId(int id)
        {
            var jsonString = JsonConvert.SerializeObject(new { id });

            var postContent = new StringContent(jsonString, _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_unresolveOrderEndpoint, postContent);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var unresolvedOrderViewModel = JsonConvert.DeserializeObject<OrderViewModel>(responseBody);

            return unresolvedOrderViewModel.Id;
        }

        private async Task<IList<OrderViewModel>> GetOrders()
        {
            var getResponse = await _client.GetAsync(_ordersEndpoint);
            getResponse.EnsureSuccessStatusCode();

            var responseString = await getResponse.Content.ReadAsStringAsync();
            var fetchedOrderCollectionViewModel = JsonConvert.DeserializeObject<ItemCollectionResponseViewModel<OrderViewModel>>(responseString);

            return fetchedOrderCollectionViewModel.Items;
        }

        private async Task<IList<ResolvedOrderViewModel>> GetResolvedOrders()
        {
            var getResponse = await _client.GetAsync(_resolvedOrdersEndpoint);
            getResponse.EnsureSuccessStatusCode();

            var responseString = await getResponse.Content.ReadAsStringAsync();
            var fetchedOrderCollectionViewModel = JsonConvert.DeserializeObject<ItemCollectionResponseViewModel<ResolvedOrderViewModel>>(responseString);

            return fetchedOrderCollectionViewModel.Items;
        }

        private async Task<int> GetOrdersCount()
        {
            var getResponse = await _client.GetAsync(_ordersCountEndpoint);
            getResponse.EnsureSuccessStatusCode();

            var responseString = await getResponse.Content.ReadAsStringAsync();
            var itemCountResponseViewModel = JsonConvert.DeserializeObject<ItemCountResponseViewModel>(responseString);

            return itemCountResponseViewModel.Count;
        }
    }
}
