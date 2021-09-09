using System.Net;
using System.Threading.Tasks;
using GraphQL;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class SchemaTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public SchemaTest(CustomWebApplicationFactory<Startup> factory) => _factory = factory;

        [Fact]
        public async Task Schema_Changed()
        {
            // Arrange
            var client = _factory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync("/graphql?sdl");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var schema = await response.Content.ReadAsStringAsync();
            Snapshot.Match(schema);
        }
    }
}