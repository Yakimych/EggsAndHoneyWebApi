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

			// TODO: Add test (and code) for preventing adding two orders of same type with the same name
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
	}
}
