using LMS_Project.Models;
using LMS_Project.Repositories.Interfaces;
using LMS_Project.Services.Interfaces;

namespace LMS_Project.Services.Implementations;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepository;
    private readonly IBookRepository _bookRepository;
    private const int MaxActiveBorrows = 3;

    public BorrowService(IBorrowRepository borrowRepository, IBookRepository bookRepository)
    {
        _borrowRepository = borrowRepository;
        _bookRepository = bookRepository;
    }

    public async Task<(bool Success, string Message)> BorrowBookAsync(int userId, int bookId)
    {
        var activeCount = await _borrowRepository.CountActiveByUserIdAsync(userId);
        if (activeCount >= MaxActiveBorrows)
            return (false, string.Format("Bạn không thể mượn quá {0} cuốn sách cùng lúc.", MaxActiveBorrows));

        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book == null)
            return (false, "Không tìm thấy sách.");

        if (book.AvailableQuantity <= 0)
            return (false, "Sách hiện không còn bản nào khả dụng.");

        var record = new BorrowRecord
        {
            UserId = userId,
            BookId = bookId,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14), // placeholder, will be reset on approval
            Status = BorrowStatus.Pending
        };

        await _borrowRepository.AddAsync(record);
        return (true, "Yêu cầu mượn sách đã được gửi. Đang chờ quản trị viên duyệt.");
    }

    public async Task<(bool Success, string Message)> ApproveAsync(int borrowId)
    {
        var record = await _borrowRepository.GetByIdAsync(borrowId);
        if (record == null)
            return (false, "Không tìm thấy phiếu mượn.");

        if (record.Status != BorrowStatus.Pending)
            return (false, "Chỉ có thể duyệt yêu cầu ở trạng thái chờ.");

        var book = await _bookRepository.GetByIdAsync(record.BookId);
        if (book == null || book.AvailableQuantity <= 0)
            return (false, "Sách không còn khả dụng.");

        book.AvailableQuantity--;
        await _bookRepository.UpdateAsync(book);

        record.Status = BorrowStatus.Approved;
        record.BorrowDate = DateTime.UtcNow;
        record.DueDate = DateTime.UtcNow.AddDays(14);
        await _borrowRepository.UpdateAsync(record);

        return (true, "Yêu cầu mượn sách đã được duyệt.");
    }

    public async Task<(bool Success, string Message)> ReturnBookAsync(int borrowId)
    {
        var record = await _borrowRepository.GetByIdAsync(borrowId);
        if (record == null)
            return (false, "Không tìm thấy phiếu mượn.");

        if (record.Status != BorrowStatus.Approved)
            return (false, "Chỉ có thể trả sách đã được duyệt.");

        var book = await _bookRepository.GetByIdAsync(record.BookId);
        if (book != null)
        {
            book.AvailableQuantity++;
            await _bookRepository.UpdateAsync(book);
        }

        record.ReturnDate = DateTime.UtcNow;
        record.Status = record.DueDate < DateTime.UtcNow ? BorrowStatus.Late : BorrowStatus.Returned;
        await _borrowRepository.UpdateAsync(record);

        return (true, "Trả sách thành công.");
    }

    public async Task<(bool Success, string Message)> RejectAsync(int borrowId)
    {
        var record = await _borrowRepository.GetByIdAsync(borrowId);
        if (record == null)
            return (false, "Không tìm thấy phiếu mượn.");

        if (record.Status != BorrowStatus.Pending)
            return (false, "Chỉ có thể từ chối yêu cầu ở trạng thái chờ.");

        record.Status = BorrowStatus.Rejected;
        await _borrowRepository.UpdateAsync(record);

        return (true, "Yêu cầu mượn sách đã bị từ chối.");
    }

    public async Task<(bool Success, string Message)> CancelAsync(int borrowId, int userId)
    {
        var record = await _borrowRepository.GetByIdAsync(borrowId);
        if (record == null)
            return (false, "Không tìm thấy phiếu mượn.");

        if (record.UserId != userId)
            return (false, "Bạn không có quyền hủy yêu cầu này.");

        if (record.Status != BorrowStatus.Pending)
            return (false, "Chỉ có thể hủy yêu cầu ở trạng thái chờ duyệt.");

        record.Status = BorrowStatus.Rejected;
        await _borrowRepository.UpdateAsync(record);

        return (true, "Yêu cầu mượn sách đã được hủy.");
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
        => await _borrowRepository.GetAllAsync();

    public async Task<IEnumerable<BorrowRecord>> GetUserHistoryAsync(int userId)
        => await _borrowRepository.GetByUserIdAsync(userId);

    public async Task<BorrowRecord?> GetByIdAsync(int id)
        => await _borrowRepository.GetByIdAsync(id);

    public async Task<int> GetTotalCountAsync()
        => await _borrowRepository.CountAsync();

    public async Task<int> GetActiveCountAsync()
        => await _borrowRepository.CountActiveAsync();
}

