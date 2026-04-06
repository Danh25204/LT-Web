using LMS_Project.Services.Interfaces;
using LMS_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace LMS_Project.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _env;
    private readonly IBookReviewService _reviewService;

    public BookController(IBookService bookService, ICategoryService categoryService, IWebHostEnvironment env, IBookReviewService reviewService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _env = env;
        _reviewService = reviewService;
    }

    public async Task<IActionResult> Index()
    {
        var books = await _bookService.GetAllBooksAsync();
        return View(books);
    }

    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null) return NotFound();

        var reviews = await _reviewService.GetReviewsByBookAsync(id);
        var avgRating = await _reviewService.GetAverageRatingAsync(id);
        ViewBag.Reviews = reviews;
        ViewBag.AvgRating = avgRating;

        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdStr, out var uid) && !User.IsInRole("Admin"))
        {
            ViewBag.CanReview = await _reviewService.CanUserReviewAsync(uid, id);
            ViewBag.UserReview = await _reviewService.GetUserReviewAsync(uid, id);
        }
        return View(book);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        await PopulateCategoriesAsync();
        return View(new BookViewModel());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync();
            return View(model);
        }

        try
        {
            await _bookService.CreateBookAsync(model, model.CoverImage, _env.WebRootPath);
            TempData["Success"] = "Thêm sách thành công.";
            return RedirectToAction("Index", "Admin", new { tab = "books" });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateCategoriesAsync();
            return View(model);
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null) return NotFound();

        var model = new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            Description = book.Description,
            CategoryId = book.CategoryId,
            Quantity = book.Quantity,
            ExistingCoverImagePath = book.CoverImagePath
        };

        await PopulateCategoriesAsync(book.CategoryId);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(model.CategoryId);
            return View(model);
        }

        try
        {
            await _bookService.UpdateBookAsync(model, model.CoverImage, _env.WebRootPath);
            TempData["Success"] = "Cập nhật sách thành công.";
            return RedirectToAction("Index", "Admin", new { tab = "books" });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateCategoriesAsync(model.CategoryId);
            return View(model);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.DeleteBookAsync(id, _env.WebRootPath);
        TempData["Success"] = "Xóa sách thành công.";
        return RedirectToAction("Index", "Admin", new { tab = "books" });
    }

    private async Task PopulateCategoriesAsync(int? selectedId = null)
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(int bookId, int rating, string? comment)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var (success, message) = await _reviewService.AddReviewAsync(userId, bookId, rating, comment);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction("Details", new { id = bookId });
    }
}
