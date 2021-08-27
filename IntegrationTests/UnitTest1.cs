using System;
using System.Linq;
using GraphQL;
using GraphQL.Data;
using Xunit;

namespace IntegrationTests
{
    public class UnitTest1 :
        IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        private readonly ApplicationDbContext _context;

        public UnitTest1(ApplicationDbContext context, CustomWebApplicationFactory<Startup> factory)
        {
            _context = context;
            _factory = factory;
        }

        [Fact]
        public void Test1()
        {
            var category = _context.Categories.Where(x => x.Id == 1);
        }
    }
}