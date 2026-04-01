using LMS_Project.Models;
using LMS_Project.Repositories.Interfaces;
using LMS_Project.Services.Interfaces;
using LMS_Project.ViewModels;

namespace LMS_Project.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        => await _categoryRepository.GetAllAsync();

    public async Task<Category?> GetCategoryByIdAsync(int id)
        => await _categoryRepository.GetByIdAsync(id);

    public async Task CreateCategoryAsync(CategoryViewModel model)
    {
        var category = new Category
        {
            Name = model.Name,
            Description = model.Description
        };
        await _categoryRepository.AddAsync(category);
    }

    public async Task UpdateCategoryAsync(CategoryViewModel model)
    {
        var category = await _categoryRepository.GetByIdAsync(model.Id)
            ?? throw new InvalidOperationException("Category not found.");

        category.Name = model.Name;
        category.Description = model.Description;
        await _categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteCategoryAsync(int id)
        => await _categoryRepository.DeleteAsync(id);

    public async Task<int> GetTotalCountAsync()
        => await _categoryRepository.CountAsync();
}
