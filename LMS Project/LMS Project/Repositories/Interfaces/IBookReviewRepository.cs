using LMS_Project.Models;

namespace LMS_Project.Repositories.Interfaces;

public interface IBookReviewRepository
{
    Task<IEnumerable<BookReview>> GetByBookIdAsync(int bookId);
    Task<BookReview?> GetByUserAndBookAsync(int userId, int bookId);
    Task AddAsync(BookReview review);
    Task<double> GetAverageRatingAsync(int bookId);
}
