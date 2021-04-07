using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Repositories.Category
{
    public interface ICategoryRepository
    {
        public Task<IList<int>> GetCategoryIdsByUserUid(string? uid);
    }
}