using Microsoft.EntityFrameworkCore;
using SimpleProject.Models;
using SimpleProject.Services.Interfaces;
using SimpleProject.UnitOfWorks;

namespace SimpleProject.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        #region Constructors
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion
        #region Handle Function
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _unitOfWork.Repository<Category>().GetListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Category>().GetAsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> GetCategoryByIdWithoutIncludeAsync(int id)
        {
            return await _unitOfWork.Repository<Category>().GetByIdAsync(id);
        }

        public async Task<bool> IsCategoryNameArExistAsync(string nameAr)
        {
            return await _unitOfWork.Repository<Category>().GetAsQueryable().AnyAsync(x => x.NameAr==nameAr);
        }

        public async Task<bool> IsCategoryNameArExistExcludeItselfAsync(string nameAr, int id)
        {
            return await _unitOfWork.Repository<Category>().GetAsQueryable().AnyAsync(x => x.NameAr==nameAr&&x.Id!=id);
        }

        public async Task<bool> IsCategoryNameEnExistAsync(string nameEn)
        {
            return await _unitOfWork.Repository<Category>().GetAsQueryable().AnyAsync(x => x.NameEn==nameEn);
        }
        public async Task<bool> IsCategoryNameEnExistExcludeItselfAsync(string nameEn, int id)
        {
            return await _unitOfWork.Repository<Category>().GetAsQueryable().AnyAsync(x => x.NameEn==nameEn&&x.Id!=id);
        }
        public async Task<string> UpdateCategoryAsync(Category Category)
        {
            try
            {
                await _unitOfWork.Repository<Category>().Updatesync(Category);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message +"--"+ex.InnerException;
            }
        }
        public async Task<string> AddCategoryAsync(Category Category)
        {
            try
            {
                await _unitOfWork.Repository<Category>().AddAsync(Category);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message +"--"+ex.InnerException;
            }
        }

        public async Task<string> DeleteCategoryAsync(Category Category)
        {
            try
            {
                await _unitOfWork.Repository<Category>().Deletesync(Category);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message +"--"+ex.InnerException;
            }
        }

        public IQueryable<Category> GetCategoriesAsQuerayable(string? search)
        {
            return _unitOfWork.Repository<Category>().GetAsQueryable();
        }


        #endregion



    }
}
