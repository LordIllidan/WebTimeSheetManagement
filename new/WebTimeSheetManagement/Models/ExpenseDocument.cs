using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimeSheetManagement.Models;

[Table("ExpenseDocuments")]
public class ExpenseDocument
{
    [Key]
    public int DocumentId { get; set; }

    [Required]
    public int ExpenseId { get; set; }
    [ForeignKey("ExpenseId")]
    public virtual required Expense Expense { get; set; }

    [Required]
    [StringLength(255)]
    public required string FileName { get; set; }

    [Required]
    [StringLength(500)]
    public required string FilePath { get; set; }

    [StringLength(100)]
    public string? ContentType { get; set; }

    public long FileSize { get; set; }

    public DateTime UploadedOn { get; set; } = DateTime.UtcNow;

    [StringLength(200)]
    public string? Description { get; set; }
}