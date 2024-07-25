using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Models;
using SimpleProject.Repositories.Interfaces;
using SimpleProject.SharedRepositories;

namespace SimpleProject.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        #region Fields
        private readonly DbSet<Category> _categories;
        #endregion
        #region Constructors
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _categories=context.Set<Category>();
        }
        #endregion
    }
}
