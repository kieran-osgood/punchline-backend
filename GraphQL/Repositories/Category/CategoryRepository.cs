using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Repositories.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        public CategoryRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ??
                                throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<IList<int>> GetCategoryIdsByUserUid(string? uid)
        {
            await using ApplicationDbContext dbContext = _dbContextFactory.CreateDbContext();

            return await
                (from u in dbContext.Users
                    from c in u.Categories
                    where u.FirebaseUid == uid
                    select c.Id).ToListAsync();
        }
    }
}