using LMS_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Project.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IBookService _bookService;

    public HomeController(IBookService bookService)
    {
        _bookService = bookService;
    }

    private const int PageSize = 6;

    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        var books = string.IsNullOrWhiteSpace(search)
            ? await _bookService.GetAllBooksAsync()
            : await _bookService.SearchBooksAsync(search);

        var totalCount = books.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

        var paged = books.Skip((page - 1) * PageSize).Take(PageSize);

        ViewBag.Search = search;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalCount = totalCount;
        return View(paged);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string keyword)
    {
        var books = await _bookService.SearchBooksAsync(keyword ?? string.Empty);
        var result = books.Select(b => new
        {
            b.Id,
            b.Title,
            b.Author,
            b.ISBN,
            b.AvailableQuantity,
            CoverImagePath = b.CoverImagePath ?? "/images/no-cover.svg",
            CategoryName = b.Category != null ? b.Category.Name : "Unknown"
        });
        return Json(result);
    }

    public IActionResult Privacy() => View();
}
