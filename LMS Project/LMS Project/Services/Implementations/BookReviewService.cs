using LMS_Project.Models;
using LMS_Project.Repositories.Interfaces;
using LMS_Project.Services.Interfaces;

namespace LMS_Project.Services.Implementations;

public class BookReviewService : IBookReviewService
{
    private readonly IBookReviewRepository _reviewRepository;
    private readonly IBorrowRepository _borrowRepository;

    public BookReviewService(IBookReviewRepository reviewRepository, IBorrowRepository borrowRepository)
    {
        _reviewRepository = reviewRepository;
        _borrowRepository = borrowRepository;
    }

    public async Task<IEnumerable<BookReview>> GetReviewsByBookAsync(int bookId)
        => await _reviewRepository.GetByBookIdAsync(bookId);

    public async Task<bool> CanUserReviewAsync(int userId, int bookId)
    {
        // Chỉ user đã trả sách mới được review
        var history = await _borrowRepository.GetByUserIdAsync(userId);
        var hasReturned = history.Any(b => b.BookId == bookId &&
            (b.Status == BorrowStatus.Returned || b.Status == BorrowStatus.Late));
        return hasReturned;
    }

    public async Task<BookReview?> GetUserReviewAsync(int userId, int bookId)
        => await _reviewRepository.GetByUserAndBookAsync(userId, bookId);

    public async Task<(bool Success, string Message)> AddReviewAsync(int userId, int bookId, int rating, string? comment)
    {
        if (rating < 1 || rating > 5)
            return (false, "Đánh giá phải từ 1 đến 5 sao.");

        var canReview = await CanUserReviewAsync(userId, bookId);
        if (!canReview)
            return (false, "Bạn cần trả sách trước khi đánh giá.");

        var existing = await _reviewRepository.GetByUserAndBookAsync(userId, bookId);
        if (existing != null)
            return (false, "Bạn đã đánh giá cuốn sách này rồi.");

        var review = new BookReview
        {
            BookId = bookId,
            UserId = userId,
            Rating = rating,
            Comment = comment?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _reviewRepository.AddAsync(review);
        return (true, "Cảm ơn bạn đã đánh giá!");
    }

    public async Task<double> GetAverageRatingAsync(int bookId)
        => await _reviewRepository.GetAverageRatingAsync(bookId);
}
