using SimpleProject.Models;

namespace SimpleProject.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategoriesAsync();
    }
}
