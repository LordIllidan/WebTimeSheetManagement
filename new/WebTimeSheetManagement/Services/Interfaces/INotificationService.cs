using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Services.Interfaces;

public interface INotificationService
{
    Task<Notification?> GetNotificationByIdAsync(int notificationId);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
    Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId);
    Task<IEnumerable<Notification>> GetNotificationsWithPaginationAsync(int userId, int page, int pageSize);
    Task<Notification> CreateNotificationAsync(Notification notification);
    Task<bool> MarkAsReadAsync(int notificationId);
    Task<bool> MarkAllAsReadAsync(int userId);
    Task<bool> DeleteNotificationAsync(int notificationId);
    Task<int> GetUnreadNotificationsCountAsync(int userId);

    // Predefined notification methods
    Task SendTimeSheetSubmittedNotificationAsync(int timeSheetId, int managerId);
    Task SendTimeSheetApprovedNotificationAsync(int timeSheetId, int userId);
    Task SendTimeSheetRejectedNotificationAsync(int timeSheetId, int userId, string reason);
    Task SendExpenseSubmittedNotificationAsync(int expenseId, int managerId);
    Task SendExpenseApprovedNotificationAsync(int expenseId, int userId);
    Task SendExpenseRejectedNotificationAsync(int expenseId, int userId, string reason);
}