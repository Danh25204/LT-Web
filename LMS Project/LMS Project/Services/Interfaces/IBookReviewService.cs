using LMS_Project.Models;

namespace LMS_Project.Services.Interfaces;

public interface IBookReviewService
{
    Task<IEnumerable<BookReview>> GetReviewsByBookAsync(int bookId);
    Task<(bool Success, string Message)> AddReviewAsync(int userId, int bookId, int rating, string? comment);
    Task<bool> CanUserReviewAsync(int userId, int bookId);
    Task<BookReview?> GetUserReviewAsync(int userId, int bookId);
    Task<double> GetAverageRatingAsync(int bookId);
}
