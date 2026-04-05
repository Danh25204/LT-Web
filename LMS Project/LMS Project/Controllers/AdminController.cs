using LMS_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Project.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IBorrowService _borrowService;
    private readonly ICategoryService _categoryService;

    public AdminController(
        IBookService bookService,
        IUserService userService,
        IBorrowService borrowService,
        ICategoryService categoryService)
    {
        _bookService = bookService;
        _userService = userService;
        _borrowService = borrowService;
        _categoryService = categoryService;
    }

    private const int BookPageSize = 10;

    public async Task<IActionResult> Index(string tab = "dashboard", int bookPage = 1, string? authorFilter = null, string? categoryFilter = null)
    {
        ViewBag.ActiveTab = tab;
        ViewBag.AuthorFilter = authorFilter;
        ViewBag.CategoryFilter = categoryFilter;

        var allBooks = await _bookService.GetAllBooksAsync();

        // Khi lọc theo tác giả: bỏ phân trang, hiển thị tất cả sách của tác giả đó
        if (!string.IsNullOrWhiteSpace(authorFilter))
        {
            var filtered = allBooks.Where(b => b.Author == authorFilter);
            ViewBag.Books = filtered;
            ViewBag.BookPage = 1;
            ViewBag.TotalBookPages = 1;
        }
        // Khi lọc theo danh mục: bỏ phân trang, hiển thị tất cả sách của danh mục đó
        else if (!string.IsNullOrWhiteSpace(categoryFilter))
        {
            var filtered = allBooks.Where(b => b.Category != null && b.Category.Name == categoryFilter);
            ViewBag.Books = filtered;
            ViewBag.BookPage = 1;
            ViewBag.TotalBookPages = 1;
        }
        else
        {
            var totalBooks2 = allBooks.Count();
            var totalBookPages2 = (int)Math.Ceiling(totalBooks2 / (double)BookPageSize);
            bookPage = Math.Max(1, Math.Min(bookPage, Math.Max(1, totalBookPages2)));
            ViewBag.Books = allBooks.Skip((bookPage - 1) * BookPageSize).Take(BookPageSize);
            ViewBag.BookPage = bookPage;
            ViewBag.TotalBookPages = totalBookPages2;
        }

        var totalBooks = allBooks.Count();

        var users = await _userService.GetAllUsersAsync();
        var borrows = await _borrowService.GetAllAsync();
        var categories = await _categoryService.GetAllCategoriesAsync();

        ViewBag.Users = users;
        ViewBag.Borrows = borrows;
        ViewBag.Categories = categories;
        ViewBag.TotalBooks = totalBooks;
        ViewBag.TotalUsers = await _userService.GetTotalCountAsync();
        ViewBag.TotalBorrows = await _borrowService.GetTotalCountAsync();
        ViewBag.ActiveBorrows = await _borrowService.GetActiveCountAsync();

        // Thống kê theo tác giả
        ViewBag.AuthorStats = allBooks
            .GroupBy(b => b.Author)
            .OrderByDescending(g => g.Count())
            .ToDictionary(g => g.Key, g => g.Count());

        // Thống kê số sách theo danh mục
        ViewBag.CategoryBookCounts = allBooks
            .GroupBy(b => b.CategoryId)
            .ToDictionary(g => g.Key, g => g.Count());

        // Thống kê theo tên danh mục (dùng cho autocomplete)
        ViewBag.CategoryStats = allBooks
            .Where(b => b.Category != null)
            .GroupBy(b => b.Category!.Name)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Count());

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> UserBorrowHistory(int id)
    {
        var user = (await _userService.GetAllUsersAsync()).FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        var history = await _borrowService.GetUserHistoryAsync(id);
        ViewBag.TargetUser = user;
        return View(history);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        TempData["Success"] = "Xóa người dùng thành công.";
        return RedirectToAction("Index", new { tab = "users" });
    }

    // Dashboard chart data endpoint
    [HttpGet]
    public async Task<IActionResult> DashboardData()
    {
        var totalBooks = await _bookService.GetTotalCountAsync();
        var totalUsers = await _userService.GetTotalCountAsync();
        var totalBorrows = await _borrowService.GetTotalCountAsync();
        var activeBorrows = await _borrowService.GetActiveCountAsync();

        return Json(new
        {
            totalBooks,
            totalUsers,
            totalBorrows,
            activeBorrows
        });
    }
}
