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

    public async Task<IActionResult> Index(string tab = "dashboard", int bookPage = 1)
    {
        ViewBag.ActiveTab = tab;

        var allBooks = await _bookService.GetAllBooksAsync();
        var totalBooks = allBooks.Count();
        var totalBookPages = (int)Math.Ceiling(totalBooks / (double)BookPageSize);
        bookPage = Math.Max(1, Math.Min(bookPage, Math.Max(1, totalBookPages)));

        ViewBag.Books = allBooks.Skip((bookPage - 1) * BookPageSize).Take(BookPageSize);
        ViewBag.BookPage = bookPage;
        ViewBag.TotalBookPages = totalBookPages;

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

        return View();
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
