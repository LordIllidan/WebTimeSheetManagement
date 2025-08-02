using Microsoft.EntityFrameworkCore;
using WebTimeSheetManagement.Data;
using WebTimeSheetManagement.Models;
using WebTimeSheetManagement.Services.Interfaces;

namespace WebTimeSheetManagement.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetProjectByIdAsync(int projectId)
    {
        return await _context.Projects.FindAsync(projectId);
    }

    public async Task<Project?> GetProjectByCodeAsync(string projectCode)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.ProjectCode == projectCode);
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .OrderBy(p => p.ProjectName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
    {
        return await _context.Projects
            .Where(p => p.IsActive)
            .OrderBy(p => p.ProjectName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetProjectsWithPaginationAsync(int page, int pageSize, string? searchTerm = null)
    {
        var query = _context.Projects.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.ProjectName.Contains(searchTerm) ||
                                   p.ProjectCode.Contains(searchTerm) ||
                                   p.NatureOfIndustry.Contains(searchTerm));
        }

        return await query
            .OrderBy(p => p.ProjectName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        project.CreatedOn = DateTime.UtcNow;
        project.IsActive = true;

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteProjectAsync(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return false;

        // Check if project has associated timesheets or expenses
        var hasTimesheets = await _context.TimeSheets.AnyAsync(t => t.ProjectId == projectId);
        var hasExpenses = await _context.Expenses.AnyAsync(e => e.ProjectId == projectId);

        if (hasTimesheets || hasExpenses)
        {
            // Instead of deleting, deactivate the project
            project.IsActive = false;
            await _context.SaveChangesAsync();
        }
        else
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> DeactivateProjectAsync(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return false;

        project.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateProjectAsync(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return false;

        project.IsActive = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsProjectCodeUniqueAsync(string projectCode, int? excludeProjectId = null)
    {
        var query = _context.Projects.Where(p => p.ProjectCode == projectCode);

        if (excludeProjectId.HasValue)
        {
            query = query.Where(p => p.ProjectId != excludeProjectId.Value);
        }

        return !await query.AnyAsync();
    }

    public async Task<int> GetTotalProjectsCountAsync()
    {
        return await _context.Projects.CountAsync();
    }

    public async Task<int> GetActiveProjectsCountAsync()
    {
        return await _context.Projects.CountAsync(p => p.IsActive);
    }
}