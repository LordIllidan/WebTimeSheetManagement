using Microsoft.EntityFrameworkCore;
using WebTimeSheetManagement.Data;
using WebTimeSheetManagement.Models;
using WebTimeSheetManagement.Services.Interfaces;

namespace WebTimeSheetManagement.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetNotificationByIdAsync(int notificationId)
    {
        return await _context.Notifications
            .Include(n => n.User)
            .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedOn)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedOn)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetNotificationsWithPaginationAsync(int userId, int page, int pageSize)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Notification> CreateNotificationAsync(Notification notification)
    {
        notification.CreatedOn = DateTime.UtcNow;
        notification.IsRead = false;

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<bool> MarkAsReadAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification == null) return false;

        notification.IsRead = true;
        notification.ReadOn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkAllAsReadAsync(int userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadOn = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteNotificationAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification == null) return false;

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetUnreadNotificationsCountAsync(int userId)
    {
        return await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    public async Task SendTimeSheetSubmittedNotificationAsync(int timeSheetId, int managerId)
    {
        var timeSheet = await _context.TimeSheets
            .Include(t => t.User)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.TimeSheetId == timeSheetId);

        if (timeSheet == null) return;

        var notification = new Notification
        {
            UserId = managerId,
            Title = "New TimeSheet Submitted",
            Message = $"{timeSheet.User.Name} has submitted a timesheet for {timeSheet.Project.ProjectName} (Week: {timeSheet.FromDate:MM/dd/yyyy})",
            Type = "Info",
            ActionUrl = $"/timesheets/approve/{timeSheetId}",
            User = await _context.Users.FindAsync(managerId) ?? new User { UserId = managerId, Name = "", Username = "", Email = "", Password = "", Gender = "", MobileNumber = "" }
        };

        await CreateNotificationAsync(notification);
    }

    public async Task SendTimeSheetApprovedNotificationAsync(int timeSheetId, int userId)
    {
        var timeSheet = await _context.TimeSheets
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.TimeSheetId == timeSheetId);

        if (timeSheet == null) return;

        var notification = new Notification
        {
            UserId = userId,
            Title = "TimeSheet Approved",
            Message = $"Your timesheet for {timeSheet.Project.ProjectName} (Week: {timeSheet.FromDate:MM/dd/yyyy}) has been approved.",
            Type = "Success",
            ActionUrl = $"/timesheets/{timeSheetId}",
            User = await _context.Users.FindAsync(userId) ?? new User { UserId = userId, Name = "", Username = "", Email = "", Password = "", Gender = "", MobileNumber = "" }
        };

        await CreateNotificationAsync(notification);
    }

    public async Task SendTimeSheetRejectedNotificationAsync(int timeSheetId, int userId, string reason)
    {
        var timeSheet = await _context.TimeSheets
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.TimeSheetId == timeSheetId);

        if (timeSheet == null) return;

        var notification = new Notification
        {
            UserId = userId,
            Title = "TimeSheet Rejected",
            Message = $"Your timesheet for {timeSheet.Project.ProjectName} (Week: {timeSheet.FromDate:MM/dd/yyyy}) has been rejected. Reason: {reason}",
            Type = "Error",
            ActionUrl = $"/timesheets/{timeSheetId}",
            User = await _context.Users.FindAsync(userId) ?? new User { UserId = userId, Name = "", Username = "", Email = "", Password = "", Gender = "", MobileNumber = "" }
        };

        await CreateNotificationAsync(notification);
    }

    public async Task SendExpenseSubmittedNotificationAsync(int expenseId, int managerId)
    {
        var expense = await _context.Expenses
            .Include(e => e.User)
            .Include(e => e.Project)
            .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);

        if (expense == null) return;

        var notification = new Notification
        {
            UserId = managerId,
            Title = "New Expense Submitted",
            Message = $"{expense.User.Name} has submitted an expense for {expense.Project.ProjectName} (Amount: ${expense.TotalAmount:F2})",
            Type = "Info",
            ActionUrl = $"/expenses/approve/{expenseId}",
            User = await _context.Users.FindAsync(managerId) ?? new User { UserId = managerId, Name = "", Username = "", Email = "", Password = "", Gender = "", MobileNumber = "" }
        };

        await CreateNotificationAsync(notification);
    }

    public async Task SendExpenseApprovedNotificationAsync(int expenseId, int userId)
    {
        var expense = await _context.Expenses
            .Include(e => e.Project)
            .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);

        if (expense == null) return;

        var notification = new Notification
        {
            UserId = userId,
            Title = "Expense Approved",
            Message = $"Your expense for {expense.Project.ProjectName} (Amount: ${expense.TotalAmount:F2}) has been approved.",
            Type = "Success",
            ActionUrl = $"/expenses/{expenseId}",
            User = await _context.Users.FindAsync(userId) ?? new User { UserId = userId, Name = "", Username = "", Email = "", Password = "", Gender = "", MobileNumber = "" }
        };

        await CreateNotificationAsync(notification);
    }

    public async Task SendExpenseRejectedNotificationAsync(int expenseId, int userId, string reason)
    {
        var expense = await _context.Expenses
            .Include(e => e.Project)
            .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);

        if (expense == null) return;

        var notification = new Notification
        {
            UserId = userId,
            Title = "Expense Rejected",
            Message = $"Your expense for {expense.Project.ProjectName} (Amount: ${expense.TotalAmount:F2}) has been rejected. Reason: {reason}",
            Type = "Error",
            ActionUrl = $"/expenses/{expenseId}",
            User = await _context.Users.FindAsync(userId) ?? new User { UserId = userId, Name = "", Username = "", Email = "", Password = "", Gender = "", MobileNumber = "" }
        };

        await CreateNotificationAsync(notification);
    }
}