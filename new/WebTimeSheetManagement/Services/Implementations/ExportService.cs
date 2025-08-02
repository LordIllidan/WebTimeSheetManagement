using WebTimeSheetManagement.Services.Interfaces;
using WebTimeSheetManagement.Models;
using ClosedXML.Excel;
using System.Text;

namespace WebTimeSheetManagement.Services.Implementations;

public class ExportService : IExportService
{
    private readonly ITimeSheetService _timeSheetService;
    private readonly IExpenseService _expenseService;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;

    public ExportService(
        ITimeSheetService timeSheetService,
        IExpenseService expenseService,
        IUserService userService,
        IProjectService projectService)
    {
        _timeSheetService = timeSheetService;
        _expenseService = expenseService;
        _userService = userService;
        _projectService = projectService;
    }

    public async Task<byte[]> ExportTimeSheetsToExcelAsync(IEnumerable<TimeSheet> timeSheets, string fileName = "TimeSheets")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("TimeSheets");

        // Headers
        worksheet.Cell(1, 1).Value = "Employee Name";
        worksheet.Cell(1, 2).Value = "Username";
        worksheet.Cell(1, 3).Value = "Project Name";
        worksheet.Cell(1, 4).Value = "Project Code";
        worksheet.Cell(1, 5).Value = "From Date";
        worksheet.Cell(1, 6).Value = "To Date";
        worksheet.Cell(1, 7).Value = "Total Hours";
        worksheet.Cell(1, 8).Value = "Monday";
        worksheet.Cell(1, 9).Value = "Tuesday";
        worksheet.Cell(1, 10).Value = "Wednesday";
        worksheet.Cell(1, 11).Value = "Thursday";
        worksheet.Cell(1, 12).Value = "Friday";
        worksheet.Cell(1, 13).Value = "Saturday";
        worksheet.Cell(1, 14).Value = "Sunday";
        worksheet.Cell(1, 15).Value = "Status";
        worksheet.Cell(1, 16).Value = "Comments";
        worksheet.Cell(1, 17).Value = "Created On";

        // Style headers
        var headerRow = worksheet.Range(1, 1, 1, 17);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightBlue;
        headerRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

        // Data
        int row = 2;
        foreach (var timeSheet in timeSheets)
        {
            worksheet.Cell(row, 1).Value = timeSheet.User.Name;
            worksheet.Cell(row, 2).Value = timeSheet.User.Username;
            worksheet.Cell(row, 3).Value = timeSheet.Project.ProjectName;
            worksheet.Cell(row, 4).Value = timeSheet.Project.ProjectCode;
            worksheet.Cell(row, 5).Value = timeSheet.FromDate;
            worksheet.Cell(row, 6).Value = timeSheet.ToDate;
            worksheet.Cell(row, 7).Value = timeSheet.TotalHours;
            worksheet.Cell(row, 8).Value = timeSheet.MondayHours;
            worksheet.Cell(row, 9).Value = timeSheet.TuesdayHours;
            worksheet.Cell(row, 10).Value = timeSheet.WednesdayHours;
            worksheet.Cell(row, 11).Value = timeSheet.ThursdayHours;
            worksheet.Cell(row, 12).Value = timeSheet.FridayHours;
            worksheet.Cell(row, 13).Value = timeSheet.SaturdayHours;
            worksheet.Cell(row, 14).Value = timeSheet.SundayHours;
            worksheet.Cell(row, 15).Value = timeSheet.Status.ToString();
            worksheet.Cell(row, 16).Value = timeSheet.Comments ?? "";
            worksheet.Cell(row, 17).Value = timeSheet.CreatedOn;
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        // Add totals row
        if (timeSheets.Any())
        {
            worksheet.Cell(row, 6).Value = "TOTAL:";
            worksheet.Cell(row, 6).Style.Font.Bold = true;
            worksheet.Cell(row, 7).Value = timeSheets.Sum(ts => ts.TotalHours);
            worksheet.Cell(row, 7).Style.Font.Bold = true;
            worksheet.Cell(row, 8).Value = timeSheets.Sum(ts => ts.MondayHours);
            worksheet.Cell(row, 9).Value = timeSheets.Sum(ts => ts.TuesdayHours);
            worksheet.Cell(row, 10).Value = timeSheets.Sum(ts => ts.WednesdayHours);
            worksheet.Cell(row, 11).Value = timeSheets.Sum(ts => ts.ThursdayHours);
            worksheet.Cell(row, 12).Value = timeSheets.Sum(ts => ts.FridayHours);
            worksheet.Cell(row, 13).Value = timeSheets.Sum(ts => ts.SaturdayHours);
            worksheet.Cell(row, 14).Value = timeSheets.Sum(ts => ts.SundayHours);
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> ExportExpensesToExcelAsync(IEnumerable<Expense> expenses, string fileName = "Expenses")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Expenses");

        // Headers
        worksheet.Cell(1, 1).Value = "Employee Name";
        worksheet.Cell(1, 2).Value = "Username";
        worksheet.Cell(1, 3).Value = "Project Name";
        worksheet.Cell(1, 4).Value = "Project Code";
        worksheet.Cell(1, 5).Value = "From Date";
        worksheet.Cell(1, 6).Value = "To Date";
        worksheet.Cell(1, 7).Value = "Hotel Bills";
        worksheet.Cell(1, 8).Value = "Travel Bills";
        worksheet.Cell(1, 9).Value = "Meals Bills";
        worksheet.Cell(1, 10).Value = "Other Bills";
        worksheet.Cell(1, 11).Value = "Total Amount";
        worksheet.Cell(1, 12).Value = "Purpose/Reason";
        worksheet.Cell(1, 13).Value = "Voucher ID";
        worksheet.Cell(1, 14).Value = "Status";
        worksheet.Cell(1, 15).Value = "Created On";

        // Style headers
        var headerRow = worksheet.Range(1, 1, 1, 15);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightGreen;
        headerRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

        // Data
        int row = 2;
        foreach (var expense in expenses)
        {
            worksheet.Cell(row, 1).Value = expense.User.Name;
            worksheet.Cell(row, 2).Value = expense.User.Username;
            worksheet.Cell(row, 3).Value = expense.Project.ProjectName;
            worksheet.Cell(row, 4).Value = expense.Project.ProjectCode;
            worksheet.Cell(row, 5).Value = expense.FromDate;
            worksheet.Cell(row, 6).Value = expense.ToDate;
            worksheet.Cell(row, 7).Value = expense.HotelBills;
            worksheet.Cell(row, 8).Value = expense.TravelBills;
            worksheet.Cell(row, 9).Value = expense.MealsBills;
            worksheet.Cell(row, 10).Value = expense.OtherBills;
            worksheet.Cell(row, 11).Value = expense.TotalAmount;
            worksheet.Cell(row, 12).Value = expense.PurposeOrReason ?? "";
            worksheet.Cell(row, 13).Value = expense.VoucherId ?? "";
            worksheet.Cell(row, 14).Value = expense.Status.ToString();
            worksheet.Cell(row, 15).Value = expense.CreatedOn;
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        // Add totals row
        if (expenses.Any())
        {
            worksheet.Cell(row, 6).Value = "TOTAL:";
            worksheet.Cell(row, 6).Style.Font.Bold = true;
            worksheet.Cell(row, 7).Value = expenses.Sum(e => e.HotelBills);
            worksheet.Cell(row, 8).Value = expenses.Sum(e => e.TravelBills);
            worksheet.Cell(row, 9).Value = expenses.Sum(e => e.MealsBills);
            worksheet.Cell(row, 10).Value = expenses.Sum(e => e.OtherBills);
            worksheet.Cell(row, 11).Value = expenses.Sum(e => e.TotalAmount);
            worksheet.Cell(row, 11).Style.Font.Bold = true;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> ExportUsersToExcelAsync(IEnumerable<User> users, string fileName = "Users")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Users");

        // Headers
        worksheet.Cell(1, 1).Value = "User ID";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Username";
        worksheet.Cell(1, 4).Value = "Email";
        worksheet.Cell(1, 5).Value = "Mobile Number";
        worksheet.Cell(1, 6).Value = "Gender";
        worksheet.Cell(1, 7).Value = "Designation";
        worksheet.Cell(1, 8).Value = "Department";
        worksheet.Cell(1, 9).Value = "Status";
        worksheet.Cell(1, 10).Value = "Created On";
        worksheet.Cell(1, 11).Value = "Last Login";

        // Style headers
        var headerRow = worksheet.Range(1, 1, 1, 11);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightYellow;
        headerRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

        // Data
        int row = 2;
        foreach (var user in users)
        {
            worksheet.Cell(row, 1).Value = user.UserId;
            worksheet.Cell(row, 2).Value = user.Name;
            worksheet.Cell(row, 3).Value = user.Username;
            worksheet.Cell(row, 4).Value = user.Email;
            worksheet.Cell(row, 5).Value = user.MobileNumber ?? "";
            worksheet.Cell(row, 6).Value = user.Gender ?? "";
            worksheet.Cell(row, 7).Value = user.Designation ?? "";
            worksheet.Cell(row, 8).Value = user.Department ?? "";
            worksheet.Cell(row, 9).Value = user.IsActive ? "Active" : "Inactive";
            worksheet.Cell(row, 10).Value = user.CreatedOn;
            worksheet.Cell(row, 11).Value = user.LastLoginDate?.ToString() ?? "Never";
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> ExportProjectsToExcelAsync(IEnumerable<Project> projects, string fileName = "Projects")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Projects");

        // Headers
        worksheet.Cell(1, 1).Value = "Project ID";
        worksheet.Cell(1, 2).Value = "Project Code";
        worksheet.Cell(1, 3).Value = "Project Name";
        worksheet.Cell(1, 4).Value = "Nature of Industry";
        worksheet.Cell(1, 5).Value = "Description";
        worksheet.Cell(1, 6).Value = "Client Name";
        worksheet.Cell(1, 7).Value = "Budget";
        worksheet.Cell(1, 8).Value = "Start Date";
        worksheet.Cell(1, 9).Value = "End Date";
        worksheet.Cell(1, 10).Value = "Duration (Days)";
        worksheet.Cell(1, 11).Value = "Status";
        worksheet.Cell(1, 12).Value = "Created On";

        // Style headers
        var headerRow = worksheet.Range(1, 1, 1, 12);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.LightCoral;
        headerRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

        // Data
        int row = 2;
        foreach (var project in projects)
        {
            worksheet.Cell(row, 1).Value = project.ProjectId;
            worksheet.Cell(row, 2).Value = project.ProjectCode;
            worksheet.Cell(row, 3).Value = project.ProjectName;
            worksheet.Cell(row, 4).Value = project.NatureOfIndustry;
            worksheet.Cell(row, 5).Value = project.Description ?? "";
            worksheet.Cell(row, 6).Value = project.ClientName ?? "";
            worksheet.Cell(row, 7).Value = project.Budget?.ToString("C") ?? "";
            worksheet.Cell(row, 8).Value = project.StartDate?.ToString("MM/dd/yyyy") ?? "";
            worksheet.Cell(row, 9).Value = project.EndDate?.ToString("MM/dd/yyyy") ?? "";

            // Calculate duration
            if (project.StartDate.HasValue)
            {
                var endDate = project.EndDate ?? DateTime.Now;
                var duration = (endDate - project.StartDate.Value).Days;
                worksheet.Cell(row, 10).Value = duration;
            }

            worksheet.Cell(row, 11).Value = project.IsActive ? "Active" : "Inactive";
            worksheet.Cell(row, 12).Value = project.CreatedOn;
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> ExportTimeSheetToPdfAsync(TimeSheet timeSheet)
    {
        // TODO: Implement PDF generation using a PDF library like iTextSharp or PdfSharp
        // For now, return a placeholder
        var html = GenerateTimeSheetHtml(timeSheet);
        return Encoding.UTF8.GetBytes(html);
    }

    public async Task<byte[]> ExportExpenseToPdfAsync(Expense expense)
    {
        // TODO: Implement PDF generation using a PDF library like iTextSharp or PdfSharp
        // For now, return a placeholder
        var html = GenerateExpenseHtml(expense);
        return Encoding.UTF8.GetBytes(html);
    }

    public async Task<byte[]> ExportMasterTimeSheetToExcelAsync(DateTime fromDate, DateTime toDate, int? projectId = null, int? userId = null)
    {
        // Get all timesheets in date range
        var allTimeSheets = await _timeSheetService.GetAllTimeSheetsAsync();
        var filteredTimeSheets = allTimeSheets.Where(ts =>
            ts.FromDate >= fromDate && ts.ToDate <= toDate);

        if (projectId.HasValue)
        {
            filteredTimeSheets = filteredTimeSheets.Where(ts => ts.Project.ProjectId == projectId.Value);
        }

        if (userId.HasValue)
        {
            filteredTimeSheets = filteredTimeSheets.Where(ts => ts.User.UserId == userId.Value);
        }

        using var workbook = new XLWorkbook();

        // Summary Sheet
        var summarySheet = workbook.Worksheets.Add("Summary");
        summarySheet.Cell(1, 1).Value = "TimeSheet Master Report";
        summarySheet.Cell(1, 1).Style.Font.Bold = true;
        summarySheet.Cell(1, 1).Style.Font.FontSize = 16;

        summarySheet.Cell(3, 1).Value = "Report Period:";
        summarySheet.Cell(3, 2).Value = $"{fromDate:MM/dd/yyyy} - {toDate:MM/dd/yyyy}";

        summarySheet.Cell(4, 1).Value = "Total TimeSheets:";
        summarySheet.Cell(4, 2).Value = filteredTimeSheets.Count();

        summarySheet.Cell(5, 1).Value = "Total Hours:";
        summarySheet.Cell(5, 2).Value = filteredTimeSheets.Sum(ts => ts.TotalHours);

        // Detailed Sheet
        var detailSheet = workbook.Worksheets.Add("Details");
        return await ExportTimeSheetsToExcelAsync(filteredTimeSheets, "MasterTimeSheet");
    }

    public async Task<byte[]> ExportMasterExpenseToExcelAsync(DateTime fromDate, DateTime toDate, int? projectId = null, int? userId = null)
    {
        // Get all expenses in date range
        var allExpenses = await _expenseService.GetAllExpensesAsync();
        var filteredExpenses = allExpenses.Where(e =>
            e.FromDate >= fromDate && e.ToDate <= toDate);

        if (projectId.HasValue)
        {
            filteredExpenses = filteredExpenses.Where(e => e.Project.ProjectId == projectId.Value);
        }

        if (userId.HasValue)
        {
            filteredExpenses = filteredExpenses.Where(e => e.User.UserId == userId.Value);
        }

        return await ExportExpensesToExcelAsync(filteredExpenses, "MasterExpense");
    }

    public async Task<byte[]> ExportProjectSummaryToExcelAsync(int projectId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null)
            throw new ArgumentException("Project not found");

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Project Summary");

        // Project Info
        worksheet.Cell(1, 1).Value = "Project Summary Report";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 16;

        worksheet.Cell(3, 1).Value = "Project Name:";
        worksheet.Cell(3, 2).Value = project.ProjectName;
        worksheet.Cell(4, 1).Value = "Project Code:";
        worksheet.Cell(4, 2).Value = project.ProjectCode;
        worksheet.Cell(5, 1).Value = "Client:";
        worksheet.Cell(5, 2).Value = project.ClientName ?? "N/A";

        // TODO: Add timesheet and expense data for this project

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> ExportUserActivityToExcelAsync(int userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found");

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("User Activity");

        // User Info
        worksheet.Cell(1, 1).Value = "User Activity Report";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 16;

        worksheet.Cell(3, 1).Value = "Employee Name:";
        worksheet.Cell(3, 2).Value = user.Name;
        worksheet.Cell(4, 1).Value = "Username:";
        worksheet.Cell(4, 2).Value = user.Username;
        worksheet.Cell(5, 1).Value = "Department:";
        worksheet.Cell(5, 2).Value = user.Department ?? "N/A";

        // TODO: Add timesheet and expense data for this user

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private string GenerateTimeSheetHtml(TimeSheet timeSheet)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <title>TimeSheet Report</title>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        .header {{ text-align: center; margin-bottom: 20px; }}
        .info {{ margin-bottom: 15px; }}
        table {{ width: 100%; border-collapse: collapse; }}
        th, td {{ border: 1px solid #ccc; padding: 8px; text-align: left; }}
        th {{ background-color: #f4f4f4; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>TimeSheet Report</h1>
    </div>
    <div class='info'>
        <strong>Employee:</strong> {timeSheet.User.Name}<br/>
        <strong>Project:</strong> {timeSheet.Project.ProjectName}<br/>
        <strong>Period:</strong> {timeSheet.FromDate:MM/dd/yyyy} - {timeSheet.ToDate:MM/dd/yyyy}<br/>
        <strong>Total Hours:</strong> {timeSheet.TotalHours}
    </div>
    <table>
        <tr>
            <th>Monday</th>
            <th>Tuesday</th>
            <th>Wednesday</th>
            <th>Thursday</th>
            <th>Friday</th>
            <th>Saturday</th>
            <th>Sunday</th>
        </tr>
        <tr>
            <td>{timeSheet.MondayHours}</td>
            <td>{timeSheet.TuesdayHours}</td>
            <td>{timeSheet.WednesdayHours}</td>
            <td>{timeSheet.ThursdayHours}</td>
            <td>{timeSheet.FridayHours}</td>
            <td>{timeSheet.SaturdayHours}</td>
            <td>{timeSheet.SundayHours}</td>
        </tr>
    </table>
</body>
</html>";
    }

    private string GenerateExpenseHtml(Expense expense)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <title>Expense Report</title>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        .header {{ text-align: center; margin-bottom: 20px; }}
        .info {{ margin-bottom: 15px; }}
        table {{ width: 100%; border-collapse: collapse; }}
        th, td {{ border: 1px solid #ccc; padding: 8px; text-align: left; }}
        th {{ background-color: #f4f4f4; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Expense Report</h1>
    </div>
    <div class='info'>
        <strong>Employee:</strong> {expense.User.Name}<br/>
        <strong>Project:</strong> {expense.Project.ProjectName}<br/>
        <strong>Period:</strong> {expense.FromDate:MM/dd/yyyy} - {expense.ToDate:MM/dd/yyyy}<br/>
        <strong>Total Amount:</strong> ${expense.TotalAmount:F2}
    </div>
    <table>
        <tr>
            <th>Category</th>
            <th>Amount</th>
        </tr>
        <tr>
            <td>Hotel Bills</td>
            <td>${expense.HotelBills:F2}</td>
        </tr>
        <tr>
            <td>Travel Bills</td>
            <td>${expense.TravelBills:F2}</td>
        </tr>
        <tr>
            <td>Meals Bills</td>
            <td>${expense.MealsBills:F2}</td>
        </tr>
        <tr>
            <td>Other Bills</td>
            <td>${expense.OtherBills:F2}</td>
        </tr>
        <tr style='font-weight: bold;'>
            <td>Total</td>
            <td>${expense.TotalAmount:F2}</td>
        </tr>
    </table>
</body>
</html>";
    }
}