using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Services.Interfaces;

public interface ITimeSheetService
{
    Task<TimeSheet?> GetTimeSheetByIdAsync(int timeSheetId);
    Task<IEnumerable<TimeSheet>> GetTimeSheetsByUserIdAsync(int userId);
    Task<IEnumerable<TimeSheet>> GetTimeSheetsByProjectIdAsync(int projectId);
    Task<IEnumerable<TimeSheet>> GetTimeSheetsByStatusAsync(TimeSheetStatus status);
    Task<IEnumerable<TimeSheet>> GetAllTimeSheetsAsync();
    Task<IEnumerable<TimeSheet>> GetTimeSheetsWithPaginationAsync(int page, int pageSize, string? searchTerm = null, int? userId = null);
    Task<IEnumerable<TimeSheet>> GetPendingTimeSheetsForApprovalAsync(int managerId);

    Task<TimeSheet> CreateTimeSheetAsync(TimeSheet timeSheet);
    Task<TimeSheet> UpdateTimeSheetAsync(TimeSheet timeSheet);
    Task<bool> DeleteTimeSheetAsync(int timeSheetId);

    Task<bool> SubmitTimeSheetAsync(int timeSheetId, int userId);
    Task<bool> ApproveTimeSheetAsync(int timeSheetId, int approvedBy, string? comment = null);
    Task<bool> RejectTimeSheetAsync(int timeSheetId, int rejectedBy, string comment);

    Task<bool> IsWeekAlreadyUsedAsync(DateTime weekStart, int userId, int? excludeTimeSheetId = null);
    Task<bool> IsTimeSheetEditableAsync(int timeSheetId, int userId);
    Task<bool> CanUserApproveTimeSheetAsync(int timeSheetId, int userId);

    Task<int> GetTimeSheetCountByUserAsync(int userId);
    Task<int> GetTimeSheetCountByStatusAsync(TimeSheetStatus status);
    Task<int> GetPendingApprovalsCountAsync(int managerId);

    Task AddTimeSheetAuditAsync(TimeSheetAudit audit);
    Task<IEnumerable<TimeSheetAudit>> GetTimeSheetAuditHistoryAsync(int timeSheetId);
}