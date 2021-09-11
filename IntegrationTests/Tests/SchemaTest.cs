using System.Net;
using System.Threading.Tasks;
using GraphQL;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class SchemaTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task Schema_Changed()
        {
            var factory = new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithUserClaims());; 

            // Arrange
            var client = factory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync("/graphql?sdl");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var schema = await response.Content.ReadAsStringAsync();
            Snapshot.Match(schema);
        }
    }
}