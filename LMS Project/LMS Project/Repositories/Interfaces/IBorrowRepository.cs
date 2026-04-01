using LMS_Project.Models;

namespace LMS_Project.Repositories.Interfaces;

public interface IBorrowRepository
{
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task<IEnumerable<BorrowRecord>> GetByUserIdAsync(int userId);
    Task<BorrowRecord?> GetByIdAsync(int id);
    Task<int> CountActiveByUserIdAsync(int userId);
    Task AddAsync(BorrowRecord record);
    Task UpdateAsync(BorrowRecord record);
    Task<int> CountAsync();
    Task<int> CountActiveAsync();
}
