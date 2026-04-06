using LMS_Project.Models;
using LMS_Project.Repositories.Interfaces;
using LMS_Project.Services.Interfaces;
using LMS_Project.ViewModels;

namespace LMS_Project.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
        => await _bookRepository.GetAllAsync();

    public async Task<Book?> GetBookByIdAsync(int id)
        => await _bookRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Book>> SearchBooksAsync(string keyword)
        => string.IsNullOrWhiteSpace(keyword)
            ? await _bookRepository.GetAllAsync()
            : await _bookRepository.SearchAsync(keyword);

    public async Task CreateBookAsync(BookViewModel model, IFormFile? coverImage, string webRootPath)
    {
        var book = new Book
        {
            Title = model.Title,
            Author = model.Author,
            ISBN = model.ISBN,
            Description = model.Description,
            CategoryId = model.CategoryId,
            Quantity = model.Quantity,
            AvailableQuantity = model.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        if (coverImage != null && coverImage.Length > 0)
            book.CoverImagePath = await SaveCoverImageAsync(coverImage, webRootPath);

        await _bookRepository.AddAsync(book);
    }

    public async Task UpdateBookAsync(BookViewModel model, IFormFile? coverImage, string webRootPath)
    {
        var book = await _bookRepository.GetByIdAsync(model.Id)
            ?? throw new InvalidOperationException("Book not found.");

        int quantityDiff = model.Quantity - book.Quantity;
        book.Title = model.Title;
        book.Author = model.Author;
        book.ISBN = model.ISBN;
        book.Description = model.Description;
        book.CategoryId = model.CategoryId;
        book.Quantity = model.Quantity;
        book.AvailableQuantity = Math.Min(model.Quantity, Math.Max(0, book.AvailableQuantity + quantityDiff));

        if (coverImage != null && coverImage.Length > 0)
        {
            DeleteCoverImage(book.CoverImagePath, webRootPath);
            book.CoverImagePath = await SaveCoverImageAsync(coverImage, webRootPath);
        }

        await _bookRepository.UpdateAsync(book);
    }

    public async Task DeleteBookAsync(int id, string webRootPath)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book != null)
        {
            DeleteCoverImage(book.CoverImagePath, webRootPath);
            await _bookRepository.DeleteAsync(id);
        }
    }

    public async Task<int> GetTotalCountAsync()
        => await _bookRepository.CountAsync();

    private static async Task<string> SaveCoverImageAsync(IFormFile file, string webRootPath)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException("Only image files (jpg, jpeg, png, gif, webp) are allowed.");

        if (file.Length > 5 * 1024 * 1024)
            throw new InvalidOperationException("File size must not exceed 5MB.");

        var imagesFolder = Path.Combine(webRootPath, "images", "books");
        Directory.CreateDirectory(imagesFolder);

        var fileName = string.Concat(Guid.NewGuid().ToString(), extension);
        var filePath = Path.Combine(imagesFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return string.Concat("/images/books/", fileName);
    }

    private static void DeleteCoverImage(string? imagePath, string webRootPath)
    {
        if (string.IsNullOrEmpty(imagePath)) return;
        var fullPath = Path.Combine(webRootPath, imagePath.TrimStart('/'));
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
