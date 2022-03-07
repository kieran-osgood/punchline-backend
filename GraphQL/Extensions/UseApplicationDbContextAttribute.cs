using System.Reflection;
using GraphQL.Data;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace GraphQL.Extensions
{
    public class UseApplicationDbContextAttribute : UseDbContextAttribute
    {
        public UseApplicationDbContextAttribute() : base(typeof(ApplicationDbContext))
        {
        }
    }
}