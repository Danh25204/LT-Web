using LMS_Project.Models;

namespace LMS_Project.Services.Interfaces;

public interface IBorrowService
{
    Task<(bool Success, string Message)> BorrowBookAsync(int userId, int bookId);
    Task<(bool Success, string Message)> ApproveAsync(int borrowId);
    Task<(bool Success, string Message)> ReturnBookAsync(int borrowId);
    Task<(bool Success, string Message)> RejectAsync(int borrowId);
    Task<(bool Success, string Message)> CancelAsync(int borrowId, int userId);
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task<IEnumerable<BorrowRecord>> GetUserHistoryAsync(int userId);
    Task<BorrowRecord?> GetByIdAsync(int id);
    Task<int> GetTotalCountAsync();
    Task<int> GetActiveCountAsync();
}
