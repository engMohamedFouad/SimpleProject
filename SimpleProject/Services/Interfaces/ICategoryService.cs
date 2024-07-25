using SimpleProject.Models;

namespace SimpleProject.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategoriesAsync();
        public IQueryable<Category> GetCategoriesAsQuerayable(string? search);
        public Task<Category?> GetCategoryByIdAsync(int id);
        public Task<Category?> GetCategoryByIdWithoutIncludeAsync(int id);
        public Task<string> AddCategoryAsync(Category Category);
        public Task<string> UpdateCategoryAsync(Category Category);
        public Task<string> DeleteCategoryAsync(Category Category);
        public Task<bool> IsCategoryNameArExistAsync(string nameAr);
        public Task<bool> IsCategoryNameArExistExcludeItselfAsync(string nameAr, int id);
        public Task<bool> IsCategoryNameEnExistAsync(string nameEn);
        public Task<bool> IsCategoryNameEnExistExcludeItselfAsync(string nameEn, int id);
    }
}
