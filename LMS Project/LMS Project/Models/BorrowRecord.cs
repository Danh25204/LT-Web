using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Project.Models;

public enum BorrowStatus
{
    Pending,
    Approved,
    Returned,
    Late,
    Rejected
}

public class BorrowRecord
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Required]
    public int BookId { get; set; }

    [ForeignKey("BookId")]
    public Book? Book { get; set; }

    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public BorrowStatus Status { get; set; } = BorrowStatus.Pending;

    public int ExtendCount { get; set; } = 0;

    [MaxLength(500)]
    public string? Notes { get; set; }
}
