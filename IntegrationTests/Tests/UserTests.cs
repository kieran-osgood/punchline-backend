using System.Threading.Tasks;
using GraphQL;
using GraphQL.Entities.User;
using HotChocolate;
using HotChocolate.Execution;
using IntegrationTests.Helpers;
using Snapshooter.Xunit;
using Xunit;

namespace IntegrationTests.Tests
{
    public class UserTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task Get_User()
        {
            var factory =
                new CustomWebApplicationFactory<Startup>().WithAuthentication(TestClaimsProvider.WithUserClaims());
            var user = Utilities.GetValidAuthUser();
            var input = new UserLoginInput(user.FirebaseUid, user.Name);

            var query =
                @"mutation Login($input: UserLoginInput!) {
					login(input: $input) {
						user {
							id
							firebaseUid
							name
							jokeCount
							categories {
								id
								name
								image				
							}
						}
					}
				}";

            var executor = await factory.Services.GetRequestExecutorAsync();
            var request = QueryRequestBuilder
                .New()
                .AddAuthorizedUser()
                .SetQuery(query)
                .SetVariableValue("input", input)
                .Create();

            var result = await executor.ExecuteAsync(request);
            
            Assert.Null(result.Errors);
            
            (await result.ToJsonAsync()).MatchSnapshot();
        }
    }
}