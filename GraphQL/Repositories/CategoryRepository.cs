using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Repositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<int>> GetCategoryIdsByUserUid(string? uid)
        {
            return await
                (from u in _context.Users
                    from c in u.Categories
                    where u.FirebaseUid == uid
                    select c.Id).ToListAsync();
        }
    }
}