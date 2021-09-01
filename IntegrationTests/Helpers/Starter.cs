using GraphQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Helpers
{
    public static class Starter
    {
        public static IServiceCollection ConfigureTestServices()
        {
            ServiceCollection services = new();
            new Startup('').ConfigureServices(services);
            return services;
        }
    }
}