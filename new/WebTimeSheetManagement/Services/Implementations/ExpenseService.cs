using Microsoft.EntityFrameworkCore;
using WebTimeSheetManagement.Data;
using WebTimeSheetManagement.Models;
using WebTimeSheetManagement.Services.Interfaces;

namespace WebTimeSheetManagement.Services.Implementations;

public class ExpenseService : IExpenseService
{
    private readonly ApplicationDbContext _context;

    public ExpenseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Expense?> GetExpenseByIdAsync(int expenseId)
    {
        return await _context.Expenses
            .Include(e => e.User)
            .Include(e => e.Project)
            .Include(e => e.Documents)
            .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);
    }

    public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId)
    {
        return await _context.Expenses
            .Include(e => e.User)
            .Include(e => e.Project)
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.FromDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesByProjectIdAsync(int projectId)
    {
        return await _context.Expenses
            .Include(e => e.User)
            .Include(e => e.Project)
            .Where(e => e.ProjectId == projectId)
            .OrderByDescending(e => e.FromDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesByStatusAsync(ExpenseStatus status)
    {
        return await _context.Expenses
            .Include(e => e.User)
            .Include(e => e.Project)
            .Where(e => e.Status == status)
            .OrderByDescending(e => e.CreatedOn)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
        return await _context.Expenses
            .Include(e => e.User)
            .Include(e => e.Project)
            .OrderByDescending(e => e.FromDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesWithPaginationAsync(int page, int pageSize, string? searchTerm = null, int? userId = null)
    {
        var query = _context.Expenses
            .Include(e => e.User)
            .Include(e => e.Project)
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(e => e.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(e => e.User.Name.Contains(searchTerm) ||
                                   e.Project.ProjectName.Contains(searchTerm) ||
                                   e.PurposeOrReason.Contains(searchTerm) ||
                                   e.VoucherId!.Contains(searchTerm));
        }

        return await query
            .OrderByDescending(e => e.FromDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetPendingExpensesForApprovalAsync(int managerId)
    {
        return await _context.Expenses
            .Include(e => e.User)
            .Include(e => e.Project)
            .Where(e => e.Status == ExpenseStatus.Submitted)
            .OrderBy(e => e.SubmittedOn)
            .ToListAsync();
    }

    public async Task<Expense> CreateExpenseAsync(Expense expense)
    {
        expense.CreatedOn = DateTime.UtcNow;
        expense.Status = ExpenseStatus.Draft;

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        // Add audit log
        await AddExpenseAuditAsync(new ExpenseAudit
        {
            ExpenseId = expense.ExpenseId,
            UserId = expense.UserId,
            Action = "Created",
            Comment = "Expense created",
            CreatedOn = DateTime.UtcNow,
            Expense = expense,
            User = expense.User
        });

        return expense;
    }

    public async Task<Expense> UpdateExpenseAsync(Expense expense)
    {
        var existingExpense = await _context.Expenses.AsNoTracking()
            .FirstOrDefaultAsync(e => e.ExpenseId == expense.ExpenseId);

        if (existingExpense != null)
        {
            // Add audit log for modification
            await AddExpenseAuditAsync(new ExpenseAudit
            {
                ExpenseId = expense.ExpenseId,
                UserId = expense.UserId,
                Action = "Modified",
                Comment = "Expense modified",
                CreatedOn = DateTime.UtcNow,
                Expense = expense,
                User = expense.User
            });
        }

        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<bool> DeleteExpenseAsync(int expenseId)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        if (expense == null) return false;

        // Only allow deletion if not submitted
        if (expense.Status != ExpenseStatus.Draft)
            return false;

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SubmitExpenseAsync(int expenseId, int userId)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        if (expense == null || expense.UserId != userId) return false;

        if (expense.Status != ExpenseStatus.Draft)
            return false;

        expense.Status = ExpenseStatus.Submitted;
        expense.SubmittedOn = DateTime.UtcNow;

        await AddExpenseAuditAsync(new ExpenseAudit
        {
            ExpenseId = expenseId,
            UserId = userId,
            Action = "Submitted",
            Comment = "Expense submitted for approval",
            CreatedOn = DateTime.UtcNow,
            Expense = expense,
            User = expense.User
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ApproveExpenseAsync(int expenseId, int approvedBy, string? comment = null)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        if (expense == null) return false;

        if (expense.Status != ExpenseStatus.Submitted)
            return false;

        expense.Status = ExpenseStatus.Approved;
        expense.ApprovedOn = DateTime.UtcNow;
        expense.ApprovedBy = approvedBy;
        expense.ApprovalComment = comment;

        await AddExpenseAuditAsync(new ExpenseAudit
        {
            ExpenseId = expenseId,
            UserId = approvedBy,
            Action = "Approved",
            Comment = comment ?? "Expense approved",
            CreatedOn = DateTime.UtcNow,
            Expense = expense,
            User = expense.User
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectExpenseAsync(int expenseId, int rejectedBy, string comment)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        if (expense == null) return false;

        if (expense.Status != ExpenseStatus.Submitted)
            return false;

        expense.Status = ExpenseStatus.Rejected;
        expense.ApprovedBy = rejectedBy;
        expense.ApprovalComment = comment;

        await AddExpenseAuditAsync(new ExpenseAudit
        {
            ExpenseId = expenseId,
            UserId = rejectedBy,
            Action = "Rejected",
            Comment = comment,
            CreatedOn = DateTime.UtcNow,
            Expense = expense,
            User = expense.User
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsDateRangeAlreadyUsedAsync(DateTime fromDate, DateTime toDate, int userId, int? excludeExpenseId = null)
    {
        var query = _context.Expenses.Where(e =>
            e.UserId == userId &&
            ((e.FromDate <= fromDate && e.ToDate >= fromDate) ||
             (e.FromDate <= toDate && e.ToDate >= toDate) ||
             (e.FromDate >= fromDate && e.ToDate <= toDate)));

        if (excludeExpenseId.HasValue)
        {
            query = query.Where(e => e.ExpenseId != excludeExpenseId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> IsExpenseEditableAsync(int expenseId, int userId)
    {
        var expense = await _context.Expenses.FindAsync(expenseId);
        if (expense == null || expense.UserId != userId) return false;

        return expense.Status == ExpenseStatus.Draft || expense.Status == ExpenseStatus.Rejected;
    }

    public async Task<bool> CanUserApproveExpenseAsync(int expenseId, int userId)
    {
        var expense = await _context.Expenses
            .Include(e => e.User)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);

        if (expense == null) return false;

        var approver = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (approver?.Role == null) return false;

        // Only managers and admins can approve
        var approverRoles = new[] { "Manager", "Admin", "SuperAdmin" };
        return approverRoles.Contains(approver.Role.RoleName);
    }

    public async Task<int> GetExpenseCountByUserAsync(int userId)
    {
        return await _context.Expenses.CountAsync(e => e.UserId == userId);
    }

    public async Task<int> GetExpenseCountByStatusAsync(ExpenseStatus status)
    {
        return await _context.Expenses.CountAsync(e => e.Status == status);
    }

    public async Task<int> GetPendingApprovalsCountAsync(int managerId)
    {
        return await _context.Expenses.CountAsync(e => e.Status == ExpenseStatus.Submitted);
    }

    public async Task<decimal> GetTotalExpenseAmountByUserAsync(int userId)
    {
        return await _context.Expenses
            .Where(e => e.UserId == userId && e.Status == ExpenseStatus.Approved)
            .SumAsync(e => e.TotalAmount);
    }

    public async Task<decimal> GetTotalExpenseAmountByProjectAsync(int projectId)
    {
        return await _context.Expenses
            .Where(e => e.ProjectId == projectId && e.Status == ExpenseStatus.Approved)
            .SumAsync(e => e.TotalAmount);
    }

    public async Task AddExpenseAuditAsync(ExpenseAudit audit)
    {
        _context.ExpenseAudits.Add(audit);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ExpenseAudit>> GetExpenseAuditHistoryAsync(int expenseId)
    {
        return await _context.ExpenseAudits
            .Include(a => a.User)
            .Where(a => a.ExpenseId == expenseId)
            .OrderByDescending(a => a.CreatedOn)
            .ToListAsync();
    }

    public async Task<ExpenseDocument> AddDocumentAsync(ExpenseDocument document)
    {
        document.UploadedOn = DateTime.UtcNow;
        _context.ExpenseDocuments.Add(document);
        await _context.SaveChangesAsync();
        return document;
    }

    public async Task<IEnumerable<ExpenseDocument>> GetExpenseDocumentsAsync(int expenseId)
    {
        return await _context.ExpenseDocuments
            .Where(d => d.ExpenseId == expenseId)
            .OrderByDescending(d => d.UploadedOn)
            .ToListAsync();
    }

    public async Task<bool> DeleteDocumentAsync(int documentId)
    {
        var document = await _context.ExpenseDocuments.FindAsync(documentId);
        if (document == null) return false;

        _context.ExpenseDocuments.Remove(document);
        await _context.SaveChangesAsync();
        return true;
    }
}