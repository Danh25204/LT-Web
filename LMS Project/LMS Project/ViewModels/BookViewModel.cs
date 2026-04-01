using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LMS_Project.ViewModels;

public class BookViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required.")]
    [MaxLength(150)]
    public string Author { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? ISBN { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "Cover Image")]
    public IFormFile? CoverImage { get; set; }

    public string? ExistingCoverImagePath { get; set; }
}
