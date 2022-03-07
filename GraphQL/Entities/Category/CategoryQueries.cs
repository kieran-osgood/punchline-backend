using System.Linq;
using GraphQL.Data;
using GraphQL.Extensions;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;

namespace GraphQL.Entities.Category
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class CategoryQueries
    {
        [Authorize]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Category> GetCategories(
            [ScopedService] ApplicationDbContext context) =>
            context.Categories;

        [Authorize]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Category> GetUserCategories(
            [GlobalState(GlobalStates.HttpContext.UserUid)]
            string? userUid,
            [ScopedService] ApplicationDbContext context)
        {
            return from c in context.Categories
                where c.Users.Any(x => x.FirebaseUid == userUid)
                select c;
        }
    }
}