using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LMS_Project.Data;

/// <summary>
/// Used by EF Core CLI tools (dotnet ef migrations) at design time.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(
            "Server=localhost;Port=3306;Database=lms_db;User=root;Password=;",
            new MySqlServerVersion(new Version(8, 0, 21)));

        return new AppDbContext(optionsBuilder.Options);
    }
}
