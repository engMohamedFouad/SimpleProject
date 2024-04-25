using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Models;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        #endregion
        #region Constructors
        public ProductService(IFileService fileService, ApplicationDbContext context)
        {
            _context = context;
            _fileService = fileService;
        }
        #endregion
        #region Implement functions
        public async Task<string> AddProduct(Product product)
        {
            try
            {
                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "--" + ex.InnerException;
            }
        }

        public async Task<string> DeleteProduct(Product product)
        {
            try
            {
                string path = product.Path;
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                _fileService.DeletePhysicalFile(path);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "--" + ex.InnerException;
            }

        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Product.FindAsync(id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<bool> IsProductNameExistAsync(string productName)
        {
            return await _context.Product.AnyAsync(x => x.Name == productName);
        }

        public async Task<string> UpdateProduct(Product product)
        {
            try
            {
                _context.Product.Update(product);
                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "--" + ex.InnerException;
            }

        }
        #endregion
    }
}
