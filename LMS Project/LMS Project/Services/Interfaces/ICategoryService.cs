using LMS_Project.Models;
using LMS_Project.ViewModels;

namespace LMS_Project.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task CreateCategoryAsync(CategoryViewModel model);
    Task UpdateCategoryAsync(CategoryViewModel model);
    Task DeleteCategoryAsync(int id);
    Task<int> GetTotalCountAsync();
}
