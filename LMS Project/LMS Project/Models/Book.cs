using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Project.Models;

public class Book
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Author { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? ISBN { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public string? CoverImagePath { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    [Range(0, int.MaxValue)]
    public int AvailableQuantity { get; set; } = 1;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
