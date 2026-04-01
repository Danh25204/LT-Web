using LMS_Project.Models;
using LMS_Project.ViewModels;

namespace LMS_Project.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task<IEnumerable<Book>> SearchBooksAsync(string keyword);
    Task CreateBookAsync(BookViewModel model, IFormFile? coverImage, string webRootPath);
    Task UpdateBookAsync(BookViewModel model, IFormFile? coverImage, string webRootPath);
    Task DeleteBookAsync(int id, string webRootPath);
    Task<int> GetTotalCountAsync();
}
