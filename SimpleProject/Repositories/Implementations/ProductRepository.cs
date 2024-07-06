using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Models;
using SimpleProject.Repositories.Interfaces;
using SimpleProject.SharedRepositories;

namespace SimpleProject.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        #region Fields
        private readonly DbSet<Product> _products;
        #endregion
        #region Constructors
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _products=context.Set<Product>();
        }
        #endregion
    }
}
