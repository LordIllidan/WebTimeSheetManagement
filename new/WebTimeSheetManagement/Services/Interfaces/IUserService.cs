using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetUsersWithPaginationAsync(int page, int pageSize, string? searchTerm = null);
    Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
    Task<IEnumerable<User>> GetAdminsAsync();
    Task<IEnumerable<User>> GetUsersUnderAdminAsync(int adminId);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int userId);
    Task<bool> ChangePasswordAsync(int userId, string newPassword);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task<int> GetTotalUsersCountAsync();
    Task<int> GetTotalAdminsCountAsync();
    Task<bool> IsUsernameUniqueAsync(string username, int? excludeUserId = null);
    Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
}