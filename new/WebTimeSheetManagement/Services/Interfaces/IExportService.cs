using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Services.Interfaces;

public interface IExportService
{
    /// <summary>
    /// Export timesheets to Excel format
    /// </summary>
    Task<byte[]> ExportTimeSheetsToExcelAsync(IEnumerable<TimeSheet> timeSheets, string fileName = "TimeSheets");

    /// <summary>
    /// Export expenses to Excel format
    /// </summary>
    Task<byte[]> ExportExpensesToExcelAsync(IEnumerable<Expense> expenses, string fileName = "Expenses");

    /// <summary>
    /// Export users to Excel format
    /// </summary>
    Task<byte[]> ExportUsersToExcelAsync(IEnumerable<User> users, string fileName = "Users");

    /// <summary>
    /// Export projects to Excel format
    /// </summary>
    Task<byte[]> ExportProjectsToExcelAsync(IEnumerable<Project> projects, string fileName = "Projects");

    /// <summary>
    /// Export timesheet to PDF format
    /// </summary>
    Task<byte[]> ExportTimeSheetToPdfAsync(TimeSheet timeSheet);

    /// <summary>
    /// Export expense report to PDF format
    /// </summary>
    Task<byte[]> ExportExpenseToPdfAsync(Expense expense);

    /// <summary>
    /// Export master timesheet report to Excel
    /// </summary>
    Task<byte[]> ExportMasterTimeSheetToExcelAsync(DateTime fromDate, DateTime toDate, int? projectId = null, int? userId = null);

    /// <summary>
    /// Export master expense report to Excel
    /// </summary>
    Task<byte[]> ExportMasterExpenseToExcelAsync(DateTime fromDate, DateTime toDate, int? projectId = null, int? userId = null);

    /// <summary>
    /// Export project summary report to Excel
    /// </summary>
    Task<byte[]> ExportProjectSummaryToExcelAsync(int projectId, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// Export user activity report to Excel
    /// </summary>
    Task<byte[]> ExportUserActivityToExcelAsync(int userId, DateTime? fromDate = null, DateTime? toDate = null);
}