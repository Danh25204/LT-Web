using Microsoft.EntityFrameworkCore;
using LMS_Project.Data;
using LMS_Project.Models;
using LMS_Project.Repositories.Interfaces;

namespace LMS_Project.Repositories.Implementations;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
        => await _context.Books
            .Include(b => b.Category)
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task<Book?> GetByIdAsync(int id)
        => await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<IEnumerable<Book>> SearchAsync(string keyword)
        => await _context.Books
            .Include(b => b.Category)
            .Where(b => b.Title.Contains(keyword)
                     || b.Author.Contains(keyword)
                     || (b.ISBN != null && b.ISBN.Contains(keyword)))
            .OrderBy(b => b.Title)
            .ToListAsync();

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> CountAsync()
        => await _context.Books.CountAsync();
}
