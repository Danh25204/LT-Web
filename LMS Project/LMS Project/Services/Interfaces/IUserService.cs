using LMS_Project.Models;
using LMS_Project.ViewModels;

namespace LMS_Project.Services.Interfaces;

public interface IUserService
{
    Task<User?> AuthenticateAsync(string email, string password);
    Task<bool> RegisterAsync(RegisterViewModel model);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task DeleteUserAsync(int id);
    Task<int> GetTotalCountAsync();
    Task<bool> EmailExistsAsync(string email);
}
