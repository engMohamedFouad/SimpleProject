using SimpleProject.Models;

namespace SimpleProject.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetProducts();
        public IQueryable<Product> GetProductsAsQuerayable(string? search);
        public Task<Product?> GetProductByIdAsync(int id);
        public Task<Product?> GetProductByIdWithoutIncludeAsync(int id);
        public Task<string> AddProduct(Product product, List<IFormFile>? files);
        public Task<string> UpdateProduct(Product product, List<IFormFile>? files);
        public Task<string> DeleteProduct(Product product);
        public Task<bool> IsProductNameArExistAsync(string nameAr);
        public Task<bool> IsProductNameArExistExcludeItselfAsync(string nameAr, int id);
        public Task<bool> IsProductNameEnExistAsync(string nameEn);
        public string GetTitle();
    }
}
