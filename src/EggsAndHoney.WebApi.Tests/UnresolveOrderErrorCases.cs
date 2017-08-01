using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace EggsAndHoney.WebApi.Tests
{
    public class UnresolveOrderErrorCases : OrdersApiTestBase
    {
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
        public async Task UnresolvingOrderWithEmptyBody_ShouldReturn_400BadRequest()
        {
            var postContent = new StringContent("", _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_unresolveOrderEndpoint, postContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UnresolvingOrderWithNullBody_ShouldReturn_400BadRequest()
        {
            var postContent = new StringContent("null", _defaultEncoding, __postContentType);
            var response = await _client.PostAsync(_unresolveOrderEndpoint, postContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
