using Microsoft.EntityFrameworkCore;
using WebTimeSheetManagement.Data;
using WebTimeSheetManagement.Models;
using WebTimeSheetManagement.Services.Interfaces;

namespace WebTimeSheetManagement.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _context;

    public RoleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _context.Roles.FindAsync(roleId);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == roleName);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles
            .OrderBy(r => r.RoleName)
            .ToListAsync();
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        role.CreatedOn = DateTime.UtcNow;
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        // Check if role has users assigned
        var hasUsers = await HasUsersAssignedAsync(roleId);
        if (hasUsers) return false;

        var role = await _context.Roles.FindAsync(roleId);
        if (role == null) return false;

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsRoleNameUniqueAsync(string roleName, int? excludeRoleId = null)
    {
        var query = _context.Roles.Where(r => r.RoleName == roleName);

        if (excludeRoleId.HasValue)
        {
            query = query.Where(r => r.RoleId != excludeRoleId.Value);
        }

        return !await query.AnyAsync();
    }

    public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        var role = await _context.Roles.FindAsync(roleId);
        if (role == null) return false;

        user.RoleId = roleId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasUsersAssignedAsync(int roleId)
    {
        return await _context.Users.AnyAsync(u => u.RoleId == roleId);
    }
}