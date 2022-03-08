using System.Net;
using System.Threading.Tasks;
using GraphQL;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class SchemaTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        [Fact]
        public async Task Schema_Changed()
        {
            var factory = new CustomWebApplicationFactory<Program>().WithAuthentication(TestClaimsProvider.WithUserClaims());; 
            var executor = await factory.Services.GetRequestExecutorAsync();
            var schema = executor.Schema.Print(); 
            schema.MatchSnapshot();
        }
        
        [Fact]
        public async Task Schema_Download_Is_Secure()
        {
            var factory = new CustomWebApplicationFactory<Program>().WithAuthentication(TestClaimsProvider.WithUserClaims());;
            
            // Arrange
            var client = factory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync("/graphql?sdl");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}