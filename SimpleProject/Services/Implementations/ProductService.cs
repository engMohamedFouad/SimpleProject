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
        public async Task<string> AddProduct(Product product, List<IFormFile>? files)
        {
            var pathList = new List<string>();
            var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();

                var result = await AddProductImages(files, product.Id);
                if (result.Item1==null&&result.Item2!="Success") return result.Item2;
                pathList=result.Item1;
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                foreach (var file in pathList)
                {
                    _fileService.DeletePhysicalFile(file);
                }
                return ex.Message + "--" + ex.InnerException;
            }
        }

        public async Task<string> DeleteProduct(Product product)
        {
            try
            {
                //string path = product.Path;
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                //_fileService.DeletePhysicalFile(path);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message + "--" + ex.InnerException;
            }

        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Product.Include(x => x.ProductsImages).FirstOrDefaultAsync(x => x.Id==id);
        }
        public async Task<Product?> GetProductByIdWithoutIncludeAsync(int id)
        {
            return await _context.Product.FindAsync(id);
        }
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Product.ToListAsync();
        }

        public string GetTitle()
        {
            return "Home Title";
        }

        public async Task<bool> IsProductNameArExistAsync(string nameAr)
        {
            return await _context.Product.AnyAsync(x => x.NameAr == nameAr);
        }

        public async Task<bool> IsProductNameArExistExcludeItselfAsync(string nameAr, int id)
        {
            return await _context.Product.AnyAsync(x => x.NameAr == nameAr&&x.Id!=id);
        }

        public async Task<bool> IsProductNameEnExistAsync(string nameEn)
        {
            return await _context.Product.AnyAsync(x => x.NameEn == nameEn);
        }
        public async Task<string> UpdateProduct(Product product, List<IFormFile>? files)
        {
            var pathList = new List<string>();
            var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Product.Update(product);
                await _context.SaveChangesAsync();

                if (files!= null&&files.Count()>0)
                {
                    var productImages = await _context.ProductsImages.Where(x => x.ProductId==product.Id).ToListAsync();
                    if (productImages.Count()>0)
                    {
                        var pathes = productImages.Select(x => x.Path).ToList();
                        _context.ProductsImages.RemoveRange(productImages);
                        await _context.SaveChangesAsync();
                        //delete Files Physically
                        foreach (var file in pathes)
                        {
                            _fileService.DeletePhysicalFile(file);
                        }
                    }
                    var result = await AddProductImages(files, product.Id);
                    if (result.Item1==null&&result.Item2!="Success") return result.Item2;
                    pathList=result.Item1;
                }
                await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                foreach (var file in pathList)
                {
                    _fileService.DeletePhysicalFile(file);
                }
                return ex.Message + "--" + ex.InnerException;
            }

        }

        private async Task<(List<string>?, string)> AddProductImages(List<IFormFile>? files, int productId)
        {
            var pathList = new List<string>();
            if (files != null && files.Count() > 0)
            {
                foreach (var file in files)
                {
                    var path = await _fileService.Upload(file, "/images/");
                    if (!path.StartsWith("/images/"))
                    {
                        return (null, path);
                    }
                    pathList.Add(path);
                }

                var productImages = new List<ProductImages>();
                foreach (var file in pathList)
                {
                    var productImage = new ProductImages();
                    productImage.ProductId = productId;
                    productImage.Path = file;
                    productImages.Add(productImage);
                }
                _context.ProductsImages.AddRange(productImages);

                await _context.SaveChangesAsync();
            }
            return (pathList, "Success");
        }
        #endregion
    }
}
