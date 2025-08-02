using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimeSheetManagement.Models;

[Table("ExpenseAudits")]
public class ExpenseAudit
{
    [Key]
    public int AuditId { get; set; }

    [Required]
    public int ExpenseId { get; set; }
    [ForeignKey("ExpenseId")]
    public virtual required Expense Expense { get; set; }

    [Required]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual required User User { get; set; }

    [Required]
    [StringLength(50)]
    public required string Action { get; set; } // Created, Modified, Submitted, Approved, Rejected

    [StringLength(500)]
    public string? Comment { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string? OldValues { get; set; }

    [StringLength(500)]
    public string? NewValues { get; set; }
}