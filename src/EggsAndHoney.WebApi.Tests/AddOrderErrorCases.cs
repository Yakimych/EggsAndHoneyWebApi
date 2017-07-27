using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace EggsAndHoney.WebApi.Tests
{
    public class AddOrderErrorCases : OrdersApiTestBase
    {
        [Fact]
		public async Task AddOrder_WithEmptyBody_ShouldRetun_400BadRequest()
		{
			var postContent = new StringContent("", _defaultEncoding, __postContentType);
			var response = await _client.PostAsync(_addOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task AddOrder_WithNullBody_ShouldRetun_400BadRequest()
		{
			var postContent = new StringContent("null", _defaultEncoding, __postContentType);
			var response = await _client.PostAsync(_addOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task AddOrder_WithSameNameAndOrderType_SecondTime_ShouldRetun_400BadRequest()
		{
			var requestBodyString = JsonConvert.SerializeObject(new { name = "TestName", order = "Eggs" });

			var postContent = new StringContent(requestBodyString, _defaultEncoding, __postContentType);
			var response = await _client.PostAsync(_addOrderEndpoint, postContent);

			response.EnsureSuccessStatusCode();

			postContent = new StringContent(requestBodyString, _defaultEncoding, __postContentType);
			var secondResponse = await _client.PostAsync(_addOrderEndpoint, postContent);

			Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
		}

		[Theory]
		[InlineData("TestName", "ReallyLongTestOrderWithLengthOver50Characters123456")]
		[InlineData("TestName", null)]
		[InlineData("TestName", "")]
		[InlineData("ReallyLongTestNameWithLengthOver50Characters1234567890", "Eggs")]
		[InlineData(null, "Honey")]
		[InlineData("", "Eggs")]
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
