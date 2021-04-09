using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GraphQL.Extensions
{
    public static class IsNullOrEmptyExtension
    {
        public static bool IsNullOrEmpty<T>([NotNull] this IEnumerable<T>? data)
        {
#pragma warning disable 8777
            if (data is null) return true;
#pragma warning restore 8777

            return !data.Any();
        }
    }
}