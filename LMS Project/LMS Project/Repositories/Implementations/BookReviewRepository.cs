using Microsoft.EntityFrameworkCore;
using LMS_Project.Data;
using LMS_Project.Models;
using LMS_Project.Repositories.Interfaces;

namespace LMS_Project.Repositories.Implementations;

public class BookReviewRepository : IBookReviewRepository
{
    private readonly AppDbContext _context;

    public BookReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookReview>> GetByBookIdAsync(int bookId)
        => await _context.BookReviews
            .Include(r => r.User)
            .Where(r => r.BookId == bookId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<BookReview?> GetByUserAndBookAsync(int userId, int bookId)
        => await _context.BookReviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);

    public async Task AddAsync(BookReview review)
    {
        await _context.BookReviews.AddAsync(review);
        await _context.SaveChangesAsync();
    }

    public async Task<double> GetAverageRatingAsync(int bookId)
    {
        var hasReviews = await _context.BookReviews.AnyAsync(r => r.BookId == bookId);
        if (!hasReviews) return 0;
        return await _context.BookReviews
            .Where(r => r.BookId == bookId)
            .AverageAsync(r => (double)r.Rating);
    }
}
