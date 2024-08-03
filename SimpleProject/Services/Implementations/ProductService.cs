using Microsoft.EntityFrameworkCore;
using SimpleProject.Models;
using SimpleProject.Services.Interfaces;
using SimpleProject.UnitOfWorks;

namespace SimpleProject.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        #endregion
        #region Constructors
        public ProductService(IFileService fileService, IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _unitOfWork=unitOfWork;
        }
        #endregion
        #region Implement functions
        public async Task<string> AddProduct(Product product, List<IFormFile>? files)
        {
            var pathList = new List<string>();
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Repository<Product>().AddAsync(product);

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
                await _unitOfWork.Repository<Product>().Deletesync(product);
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
            return await _unitOfWork.Repository<Product>().GetAsQueryable().Include(x => x.ProductsImages).FirstOrDefaultAsync(x => x.Id==id);
        }
        public async Task<Product?> GetProductByIdWithoutIncludeAsync(int id)
        {
            return await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        }
        public async Task<List<Product>> GetProducts()
        {
            return await _unitOfWork.Repository<Product>().GetListAsync();
        }

        public IQueryable<Product> GetProductsAsQuerayable(string? search)
        {
            var products = _unitOfWork.Repository<Product>().GetAsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                products=products.Where(x => x.NameAr.Contains(search)||x.NameEn.Contains(search));
            }
            return products;
        }

        public string GetTitle()
        {
            return "Home Title";
        }

        public async Task<bool> IsProductNameArExistAsync(string nameAr)
        {
            return await _unitOfWork.Repository<Product>().GetAsQueryable().AnyAsync(x => x.NameAr == nameAr);
        }

        public async Task<bool> IsProductNameArExistExcludeItselfAsync(string nameAr, int id)
        {
            return await _unitOfWork.Repository<Product>().GetAsQueryable().AnyAsync(x => x.NameAr == nameAr&&x.Id!=id);
        }

        public async Task<bool> IsProductNameEnExistAsync(string nameEn)
        {
            return await _unitOfWork.Repository<Product>().GetAsQueryable().AnyAsync(x => x.NameEn == nameEn);
        }
        public async Task<string> UpdateProduct(Product product, List<IFormFile>? files)
        {
            var pathList = new List<string>();
            var trans = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Repository<Product>().Updatesync(product);

                if (files!= null&&files.Count()>0)
                {
                    var productImages = await _unitOfWork.Repository<ProductImages>().GetAsQueryable().Where(x => x.ProductId==product.Id).ToListAsync();
                    if (productImages.Count()>0)
                    {
                        var pathes = productImages.Select(x => x.Path).ToList();
                        await _unitOfWork.Repository<ProductImages>().DeleteRangeAsync(productImages);
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
                await _unitOfWork.Repository<ProductImages>().AddRangeAsync(productImages);
            }
            return (pathList, "Success");
        }
        #endregion
    }
}
