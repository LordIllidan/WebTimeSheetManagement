using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Services.Interfaces;

public interface IProjectService
{
    Task<Project?> GetProjectByIdAsync(int projectId);
    Task<Project?> GetProjectByCodeAsync(string projectCode);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task<IEnumerable<Project>> GetActiveProjectsAsync();
    Task<IEnumerable<Project>> GetProjectsWithPaginationAsync(int page, int pageSize, string? searchTerm = null);
    Task<Project> CreateProjectAsync(Project project);
    Task<Project> UpdateProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(int projectId);
    Task<bool> DeactivateProjectAsync(int projectId);
    Task<bool> ActivateProjectAsync(int projectId);
    Task<bool> IsProjectCodeUniqueAsync(string projectCode, int? excludeProjectId = null);
    Task<int> GetTotalProjectsCountAsync();
    Task<int> GetActiveProjectsCountAsync();
}