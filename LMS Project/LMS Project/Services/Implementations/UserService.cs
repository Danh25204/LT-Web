using LMS_Project.Models;
using LMS_Project.Repositories.Interfaces;
using LMS_Project.Services.Interfaces;
using LMS_Project.ViewModels;

namespace LMS_Project.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return null;
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
    }

    public async Task<bool> RegisterAsync(RegisterViewModel model)
    {
        if (await _userRepository.GetByEmailAsync(model.Email) != null)
            return false;

        var user = new User
        {
            FullName = model.FullName,
            Email = model.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        return true;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
        => await _userRepository.GetAllAsync();

    public async Task<User?> GetUserByIdAsync(int id)
        => await _userRepository.GetByIdAsync(id);

    public async Task DeleteUserAsync(int id)
        => await _userRepository.DeleteAsync(id);

    public async Task<int> GetTotalCountAsync()
        => await _userRepository.CountAsync();

    public async Task<bool> EmailExistsAsync(string email)
        => await _userRepository.GetByEmailAsync(email) != null;
}
