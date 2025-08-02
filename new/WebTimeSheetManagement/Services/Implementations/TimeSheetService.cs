using Microsoft.EntityFrameworkCore;
using WebTimeSheetManagement.Data;
using WebTimeSheetManagement.Models;
using WebTimeSheetManagement.Services.Interfaces;

namespace WebTimeSheetManagement.Services.Implementations;

public class TimeSheetService : ITimeSheetService
{
    private readonly ApplicationDbContext _context;

    public TimeSheetService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TimeSheet?> GetTimeSheetByIdAsync(int timeSheetId)
    {
        return await _context.TimeSheets
            .Include(t => t.User)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.TimeSheetId == timeSheetId);
    }

    public async Task<IEnumerable<TimeSheet>> GetTimeSheetsByUserIdAsync(int userId)
    {
        return await _context.TimeSheets
            .Include(t => t.User)
            .Include(t => t.Project)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.FromDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeSheet>> GetTimeSheetsByProjectIdAsync(int projectId)
    {
        return await _context.TimeSheets
            .Include(t => t.User)
            .Include(t => t.Project)
            .Where(t => t.ProjectId == projectId)
            .OrderByDescending(t => t.FromDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeSheet>> GetTimeSheetsByStatusAsync(TimeSheetStatus status)
    {
        return await _context.TimeSheets
            .Include(t => t.User)
            .Include(t => t.Project)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeSheet>> GetAllTimeSheetsAsync()
    {
        return await _context.TimeSheets
            .Include(t => t.User)
            .Include(t => t.Project)
            .OrderByDescending(t => t.FromDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeSheet>> GetTimeSheetsWithPaginationAsync(int page, int pageSize, string? searchTerm = null, int? userId = null)
    {
        var query = _context.TimeSheets
            .Include(t => t.User)
            .Include(t => t.Project)
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(t => t.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(t => t.User.Name.Contains(searchTerm) ||
                                   t.Project.ProjectName.Contains(searchTerm) ||
                                   t.Comments!.Contains(searchTerm));
        }

        return await query
            .OrderByDescending(t => t.FromDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeSheet>> GetPendingTimeSheetsForApprovalAsync(int managerId)
    {
        return await _context.TimeSheets
            .Include(t => t.User)
            .Include(t => t.Project)
            .Where(t => t.Status == TimeSheetStatus.Submitted)
            .OrderBy(t => t.SubmittedOn)
            .ToListAsync();
    }

    public async Task<TimeSheet> CreateTimeSheetAsync(TimeSheet timeSheet)
    {
        timeSheet.CreatedOn = DateTime.UtcNow;
        timeSheet.Status = TimeSheetStatus.Draft;

        _context.TimeSheets.Add(timeSheet);
        await _context.SaveChangesAsync();

        // Add audit log
        await AddTimeSheetAuditAsync(new TimeSheetAudit
        {
            TimeSheetId = timeSheet.TimeSheetId,
            UserId = timeSheet.UserId,
            Action = "Created",
            Comment = "TimeSheet created",
            CreatedOn = DateTime.UtcNow,
            TimeSheet = timeSheet,
            User = timeSheet.User
        });

        return timeSheet;
    }

    public async Task<TimeSheet> UpdateTimeSheetAsync(TimeSheet timeSheet)
    {
        var existingTimeSheet = await _context.TimeSheets.AsNoTracking()
            .FirstOrDefaultAsync(t => t.TimeSheetId == timeSheet.TimeSheetId);

        if (existingTimeSheet != null)
        {
            // Add audit log for modification
            await AddTimeSheetAuditAsync(new TimeSheetAudit
            {
                TimeSheetId = timeSheet.TimeSheetId,
                UserId = timeSheet.UserId,
                Action = "Modified",
                Comment = "TimeSheet modified",
                CreatedOn = DateTime.UtcNow,
                TimeSheet = timeSheet,
                User = timeSheet.User
            });
        }

        _context.TimeSheets.Update(timeSheet);
        await _context.SaveChangesAsync();
        return timeSheet;
    }

    public async Task<bool> DeleteTimeSheetAsync(int timeSheetId)
    {
        var timeSheet = await _context.TimeSheets.FindAsync(timeSheetId);
        if (timeSheet == null) return false;

        // Only allow deletion if not submitted
        if (timeSheet.Status != TimeSheetStatus.Draft)
            return false;

        _context.TimeSheets.Remove(timeSheet);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SubmitTimeSheetAsync(int timeSheetId, int userId)
    {
        var timeSheet = await _context.TimeSheets.FindAsync(timeSheetId);
        if (timeSheet == null || timeSheet.UserId != userId) return false;

        if (timeSheet.Status != TimeSheetStatus.Draft)
            return false;

        timeSheet.Status = TimeSheetStatus.Submitted;
        timeSheet.SubmittedOn = DateTime.UtcNow;

        await AddTimeSheetAuditAsync(new TimeSheetAudit
        {
            TimeSheetId = timeSheetId,
            UserId = userId,
            Action = "Submitted",
            Comment = "TimeSheet submitted for approval",
            CreatedOn = DateTime.UtcNow,
            TimeSheet = timeSheet,
            User = timeSheet.User
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveTimeSheetAsync(int timeSheetId, int approvedBy, string? comment = null)
    {
        var timeSheet = await _context.TimeSheets.FindAsync(timeSheetId);
        if (timeSheet == null) return false;

        if (timeSheet.Status != TimeSheetStatus.Submitted)
            return false;

        timeSheet.Status = TimeSheetStatus.Approved;
        timeSheet.ApprovedOn = DateTime.UtcNow;
        timeSheet.ApprovedBy = approvedBy;
        timeSheet.ApprovalComment = comment;

        await AddTimeSheetAuditAsync(new TimeSheetAudit
        {
            TimeSheetId = timeSheetId,
            UserId = approvedBy,
            Action = "Approved",
            Comment = comment ?? "TimeSheet approved",
            CreatedOn = DateTime.UtcNow,
            TimeSheet = timeSheet,
            User = timeSheet.User
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectTimeSheetAsync(int timeSheetId, int rejectedBy, string comment)
    {
        var timeSheet = await _context.TimeSheets.FindAsync(timeSheetId);
        if (timeSheet == null) return false;

        if (timeSheet.Status != TimeSheetStatus.Submitted)
            return false;

        timeSheet.Status = TimeSheetStatus.Rejected;
        timeSheet.ApprovedBy = rejectedBy;
        timeSheet.ApprovalComment = comment;

        await AddTimeSheetAuditAsync(new TimeSheetAudit
        {
            TimeSheetId = timeSheetId,
            UserId = rejectedBy,
            Action = "Rejected",
            Comment = comment,
            CreatedOn = DateTime.UtcNow,
            TimeSheet = timeSheet,
            User = timeSheet.User
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsWeekAlreadyUsedAsync(DateTime weekStart, int userId, int? excludeTimeSheetId = null)
    {
        var query = _context.TimeSheets.Where(t =>
            t.UserId == userId &&
            t.FromDate == weekStart.Date);

        if (excludeTimeSheetId.HasValue)
        {
            query = query.Where(t => t.TimeSheetId != excludeTimeSheetId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> IsTimeSheetEditableAsync(int timeSheetId, int userId)
    {
        var timeSheet = await _context.TimeSheets.FindAsync(timeSheetId);
        if (timeSheet == null || timeSheet.UserId != userId) return false;

        return timeSheet.Status == TimeSheetStatus.Draft || timeSheet.Status == TimeSheetStatus.Rejected;
    }

    public async Task<bool> CanUserApproveTimeSheetAsync(int timeSheetId, int userId)
    {
        var timeSheet = await _context.TimeSheets
            .Include(t => t.User)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(t => t.TimeSheetId == timeSheetId);

        if (timeSheet == null) return false;

        var approver = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (approver?.Role == null) return false;

        // Only managers and admins can approve
        var approverRoles = new[] { "Manager", "Admin", "SuperAdmin" };
        return approverRoles.Contains(approver.Role.RoleName);
    }

    public async Task<int> GetTimeSheetCountByUserAsync(int userId)
    {
        return await _context.TimeSheets.CountAsync(t => t.UserId == userId);
    }

    public async Task<int> GetTimeSheetCountByStatusAsync(TimeSheetStatus status)
    {
        return await _context.TimeSheets.CountAsync(t => t.Status == status);
    }

    public async Task<int> GetPendingApprovalsCountAsync(int managerId)
    {
        return await _context.TimeSheets.CountAsync(t => t.Status == TimeSheetStatus.Submitted);
    }

    public async Task AddTimeSheetAuditAsync(TimeSheetAudit audit)
    {
        _context.TimeSheetAudits.Add(audit);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TimeSheetAudit>> GetTimeSheetAuditHistoryAsync(int timeSheetId)
    {
        return await _context.TimeSheetAudits
            .Include(a => a.User)
            .Where(a => a.TimeSheetId == timeSheetId)
            .OrderByDescending(a => a.CreatedOn)
            .ToListAsync();
    }
}