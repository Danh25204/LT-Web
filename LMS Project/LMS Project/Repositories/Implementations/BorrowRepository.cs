using Microsoft.EntityFrameworkCore;
using LMS_Project.Data;
using LMS_Project.Models;
using LMS_Project.Repositories.Interfaces;

namespace LMS_Project.Repositories.Implementations;

public class BorrowRepository : IBorrowRepository
{
    private readonly AppDbContext _context;

    public BorrowRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
        => await _context.BorrowRecords
            .Include(br => br.User)
            .Include(br => br.Book)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync();

    public async Task<IEnumerable<BorrowRecord>> GetByUserIdAsync(int userId)
        => await _context.BorrowRecords
            .Include(br => br.Book)
            .Where(br => br.UserId == userId)
            .OrderByDescending(br => br.BorrowDate)
            .ToListAsync();

    public async Task<BorrowRecord?> GetByIdAsync(int id)
        => await _context.BorrowRecords
            .Include(br => br.User)
            .Include(br => br.Book)
            .FirstOrDefaultAsync(br => br.Id == id);

    public async Task<int> CountActiveByUserIdAsync(int userId)
        => await _context.BorrowRecords
            .CountAsync(br => br.UserId == userId
                && (br.Status == BorrowStatus.Pending || br.Status == BorrowStatus.Approved));

    public async Task<bool> HasActiveBorrowAsync(int userId, int bookId)
        => await _context.BorrowRecords
            .AnyAsync(br => br.UserId == userId && br.BookId == bookId
                && (br.Status == BorrowStatus.Pending || br.Status == BorrowStatus.Approved));

    public async Task AddAsync(BorrowRecord record)
    {
        await _context.BorrowRecords.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
        => await _context.BorrowRecords.CountAsync();

    public async Task<int> CountActiveAsync()
        => await _context.BorrowRecords
            .CountAsync(br => br.Status == BorrowStatus.Approved);
}
