using LMS_Project.Models;

namespace LMS_Project.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<IEnumerable<Book>> SearchAsync(string keyword);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}
