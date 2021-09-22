using System.Net;
using System.Threading.Tasks;
using GraphQL;
using Xunit;

namespace IntegrationTests.Tests
{
    public class DeepLinkTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task Apple_App_Site_Association_Is_200()
        {
            var factory =
                new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithUserClaims());
            var client = factory.CreateDefaultClient();
            var response = await client.GetAsync(".well-known/apple-app-site-association");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Android_Asset_Links_200()
        {
            var factory =
                new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithUserClaims());
            var client = factory.CreateDefaultClient();
            var response = await client.GetAsync(".well-known/assetlinks.json");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}