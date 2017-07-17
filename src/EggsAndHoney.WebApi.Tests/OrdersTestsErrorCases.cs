using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace EggsAndHoney.WebApi.Tests
{
    public class OrdersTestsErrorCases : OrdersApiTestBase
    {
		[Fact]
		public async Task ResolvingNonExistingOrder_ShouldReturn_404NotFound()
		{
			var jsonString = JsonConvert.SerializeObject(new { id = int.MaxValue });

			var postContent = new StringContent(jsonString, _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_resolveOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task ResolvingOrderWithInvalidId_ShouldReturn_400BadRequest()
		{
			var jsonString = JsonConvert.SerializeObject(new { id = 0 });

			var postContent = new StringContent(jsonString, _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_resolveOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task UnresolvingNonExistingOrder_ShouldReturn_404NotFound()
		{
			var jsonString = JsonConvert.SerializeObject(new { id = int.MaxValue });

			var postContent = new StringContent(jsonString, _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_unresolveOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task UnresolvingOrderWithInvalidId_ShouldReturn_400BadRequest()
		{
			var jsonString = JsonConvert.SerializeObject(new { id = 0 });

			var postContent = new StringContent(jsonString, _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_unresolveOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task AddOrder_WithEmptyBody_ShouldRetun_400BadRequest()
		{
			var postContent = new StringContent("", _defaultEncoding, __postContentType);
            var addResponse = await _client.PostAsync(_addOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.BadRequest, addResponse.StatusCode);
		}

        [Theory]
		[InlineData("TestName", null)]
		[InlineData("TestName", "")]
		[InlineData(null, "TestOrder")]
		[InlineData("", "TestOrder")]
		[InlineData(null, null)]
		[InlineData("", "")]
		public async Task AddOrder_WithInvalidPostData_ShouldReturn_400BadRequest(string name, string order)
		{
			var requestBodyString = JsonConvert.SerializeObject(new { name, order });

			var postContent = new StringContent(requestBodyString, _defaultEncoding, __postContentType);
            var addResponse = await _client.PostAsync(_addOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.BadRequest, addResponse.StatusCode);
		}
	}
}
