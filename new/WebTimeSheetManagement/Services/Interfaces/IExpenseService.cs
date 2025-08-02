using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Services.Interfaces;

public interface IExpenseService
{
    Task<Expense?> GetExpenseByIdAsync(int expenseId);
    Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId);
    Task<IEnumerable<Expense>> GetExpensesByProjectIdAsync(int projectId);
    Task<IEnumerable<Expense>> GetExpensesByStatusAsync(ExpenseStatus status);
    Task<IEnumerable<Expense>> GetAllExpensesAsync();
    Task<IEnumerable<Expense>> GetExpensesWithPaginationAsync(int page, int pageSize, string? searchTerm = null, int? userId = null);
    Task<IEnumerable<Expense>> GetPendingExpensesForApprovalAsync(int managerId);

    Task<Expense> CreateExpenseAsync(Expense expense);
    Task<Expense> UpdateExpenseAsync(Expense expense);
    Task<bool> DeleteExpenseAsync(int expenseId);

    Task<bool> SubmitExpenseAsync(int expenseId, int userId);
    Task<bool> ApproveExpenseAsync(int expenseId, int approvedBy, string? comment = null);
    Task<bool> RejectExpenseAsync(int expenseId, int rejectedBy, string comment);

    Task<bool> IsDateRangeAlreadyUsedAsync(DateTime fromDate, DateTime toDate, int userId, int? excludeExpenseId = null);
    Task<bool> IsExpenseEditableAsync(int expenseId, int userId);
    Task<bool> CanUserApproveExpenseAsync(int expenseId, int userId);

    Task<int> GetExpenseCountByUserAsync(int userId);
    Task<int> GetExpenseCountByStatusAsync(ExpenseStatus status);
    Task<int> GetPendingApprovalsCountAsync(int managerId);
    Task<decimal> GetTotalExpenseAmountByUserAsync(int userId);
    Task<decimal> GetTotalExpenseAmountByProjectAsync(int projectId);

    Task AddExpenseAuditAsync(ExpenseAudit audit);
    Task<IEnumerable<ExpenseAudit>> GetExpenseAuditHistoryAsync(int expenseId);

    Task<ExpenseDocument> AddDocumentAsync(ExpenseDocument document);
    Task<IEnumerable<ExpenseDocument>> GetExpenseDocumentsAsync(int expenseId);
    Task<bool> DeleteDocumentAsync(int documentId);
}