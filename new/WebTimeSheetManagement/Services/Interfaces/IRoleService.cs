using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Services.Interfaces;

public interface IRoleService
{
    Task<Role?> GetRoleByIdAsync(int roleId);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role> CreateRoleAsync(Role role);
    Task<Role> UpdateRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(int roleId);
    Task<bool> IsRoleNameUniqueAsync(string roleName, int? excludeRoleId = null);
    Task<bool> AssignRoleToUserAsync(int userId, int roleId);
    Task<bool> HasUsersAssignedAsync(int roleId);
}