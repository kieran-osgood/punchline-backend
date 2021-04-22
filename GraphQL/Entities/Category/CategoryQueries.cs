using System.Linq;
using GraphQL.Data;
using GraphQL.Extensions;
using GraphQL.Static;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;

namespace GraphQL.Entities.Category
{
    [ExtendObjectType(ObjectTypes.Query)]
    public class CategoryQueries
    {
        [Authorize]
        [UseApplicationDbContext]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Category> GetCategories(
            [ScopedService] ApplicationDbContext context) =>
            context.Categories;
    }
}