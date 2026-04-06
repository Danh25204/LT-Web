using Microsoft.EntityFrameworkCore;
using LMS_Project.Models;

namespace LMS_Project.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }
    public DbSet<BookReview> BookReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<BorrowRecord>()
            .HasOne(br => br.User)
            .WithMany(u => u.BorrowRecords)
            .HasForeignKey(br => br.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BorrowRecord>()
            .HasOne(br => br.Book)
            .WithMany(b => b.BorrowRecords)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookReview>()
            .HasOne(r => r.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookReview>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookReview>()
            .HasIndex(r => new { r.BookId, r.UserId })
            .IsUnique(); // mỗi user chỉ review 1 lần / cuốn

        // Seed default admin user
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            FullName = "Administrator",
            Email = "admin@lms.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin",
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        // Seed default categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Công Nghệ", Description = "Sách về khoa học máy tính và lập trình" },
            new Category { Id = 2, Name = "Toán Học", Description = "Sách về toán học và thống kê" },
            new Category { Id = 3, Name = "Khoa Học", Description = "Sách về khoa học tự nhiên" },
            new Category { Id = 4, Name = "Văn Học", Description = "Sách văn học và tiểu thuyết" },
            new Category { Id = 5, Name = "Lịch Sử", Description = "Sách về lịch sử thế giới và văn hóa" }
        );
    }
}
