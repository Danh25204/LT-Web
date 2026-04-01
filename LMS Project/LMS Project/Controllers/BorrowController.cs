using LMS_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS_Project.Controllers;

[Authorize]
public class BorrowController : Controller
{
    private readonly IBorrowService _borrowService;

    public BorrowController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(int bookId)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Challenge();

        var (success, message) = await _borrowService.BorrowBookAsync(userId.Value, bookId);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { success, message });

        if (success)
            TempData["Success"] = message;
        else
            TempData["Error"] = message;

        return RedirectToAction("Details", "Book", new { id = bookId });
    }

    public async Task<IActionResult> History()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Challenge();

        var records = await _borrowService.GetUserHistoryAsync(userId.Value);
        return View(records);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> All()
    {
        var records = await _borrowService.GetAllAsync();
        return View(records);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var (success, message) = await _borrowService.ApproveAsync(id);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction("Index", "Admin", new { tab = "borrows" });
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserReturn(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Challenge();

        // Verify the record belongs to this user
        var history = await _borrowService.GetUserHistoryAsync(userId.Value);
        if (!history.Any(r => r.Id == id))
            return Forbid();

        var (success, message) = await _borrowService.ReturnBookAsync(id);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction("History");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id)
    {
        var (success, message) = await _borrowService.ReturnBookAsync(id);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction("Index", "Admin", new { tab = "borrows" });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var (success, message) = await _borrowService.RejectAsync(id);
        TempData[success ? "Success" : "Error"] = message;
        return RedirectToAction("Index", "Admin", new { tab = "borrows" });
    }

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
    }
}
