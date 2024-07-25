using Microsoft.EntityFrameworkCore;
using SimpleProject.Models;
using SimpleProject.Repositories.Interfaces;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        #region Fields
        private readonly ICategoryRepository _categoryRepository;
        #endregion
        #region Constructors
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        #endregion
        #region Handle Function
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetAsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> GetCategoryByIdWithoutIncludeAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<bool> IsCategoryNameArExistAsync(string nameAr)
        {
            return await _categoryRepository.GetAsQueryable().AnyAsync(x => x.NameAr==nameAr);
        }

        public async Task<bool> IsCategoryNameArExistExcludeItselfAsync(string nameAr, int id)
        {
            return await _categoryRepository.GetAsQueryable().AnyAsync(x => x.NameAr==nameAr&&x.Id!=id);
        }

        public async Task<bool> IsCategoryNameEnExistAsync(string nameEn)
        {
            return await _categoryRepository.GetAsQueryable().AnyAsync(x => x.NameEn==nameEn);
        }
        public async Task<bool> IsCategoryNameEnExistExcludeItselfAsync(string nameEn, int id)
        {
            return await _categoryRepository.GetAsQueryable().AnyAsync(x => x.NameEn==nameEn&&x.Id!=id);
        }
        public async Task<string> UpdateCategoryAsync(Category Category)
        {
            try
            {
                await _categoryRepository.Updatesync(Category);
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
                await _categoryRepository.AddAsync(Category);
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
                await _categoryRepository.Deletesync(Category);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message +"--"+ex.InnerException;
            }
        }

        public IQueryable<Category> GetCategoriesAsQuerayable(string? search)
        {
            return _categoryRepository.GetAsQueryable();
        }


        #endregion



    }
}
