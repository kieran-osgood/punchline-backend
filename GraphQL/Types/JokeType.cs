using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Data;
using GraphQL.DataLoader;
using GraphQL.Extensions;
using GreenDonut;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class JokeType : ObjectType<Joke>
    {
        protected override void Configure(IObjectTypeDescriptor<Joke> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<JokeByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.Categories)
                .ResolveWith<JokeResolvers>(t => t.GetCategoriesAsync(default!, default!, default!, default!))
                .UseDbContext<ApplicationDbContext>()
                .Name("categories");
        }

        private class JokeResolvers
        {
            public async Task<IEnumerable<Category>> GetCategoriesAsync(
                Joke joke,
                CategoryByIdDataLoader dataLoader,
                [ScopedService] ApplicationDbContext dbContext,
                CancellationToken cancellationToken)
            {
                var ids = await (
                        from j in dbContext.Jokes
                        where j.Id == joke.Id
                        select j.Categories.Select(x=>x.Id).ToList())
                    .FirstOrDefaultAsync(cancellationToken);

                return await dataLoader.LoadAsync(ids, cancellationToken);
            }
        }
    }
}