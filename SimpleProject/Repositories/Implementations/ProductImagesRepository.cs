using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Models;
using SimpleProject.Repositories.Interfaces;
using SimpleProject.SharedRepositories;

namespace SimpleProject.Repositories.Implementations
{
    public class ProductImagesRepository : GenericRepository<ProductImages>, IProductImagesRepository
    {
        #region Fields
        private readonly DbSet<ProductImages> _productImages;
        #endregion
        #region Constructors
        public ProductImagesRepository(ApplicationDbContext context) : base(context)
        {
            _productImages=context.Set<ProductImages>();
        }
        #endregion
    }
}
