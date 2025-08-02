using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimeSheetManagement.Models;

[Table("Expenses")]
public class Expense
{
    [Key]
    public int ExpenseId { get; set; }

    [Required]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual required User User { get; set; }

    [Display(Name = "Project")]
    [Required(ErrorMessage = "Choose project")]
    public int ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public virtual required Project Project { get; set; }

    [Display(Name = "Purpose / Reason")]
    [Required(ErrorMessage = "Please enter purpose/reason")]
    [StringLength(500)]
    public required string PurposeOrReason { get; set; }

    [Display(Name = "Expense From Date")]
    [Required(ErrorMessage = "Please choose from date")]
    public DateTime FromDate { get; set; }

    [Display(Name = "Expense To Date")]
    [Required(ErrorMessage = "Please choose to date")]
    public DateTime ToDate { get; set; }

    [Display(Name = "Expense ID / Voucher ID")]
    [StringLength(50)]
    public string? VoucherId { get; set; }

    [Display(Name = "Hotel")]
    [Range(0, int.MaxValue, ErrorMessage = "Enter valid amount")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal HotelBills { get; set; }

    [Display(Name = "Travel")]
    [Range(0, int.MaxValue, ErrorMessage = "Enter valid amount")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TravelBills { get; set; }

    [Display(Name = "Meals")]
    [Range(0, int.MaxValue, ErrorMessage = "Enter valid amount")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MealsBills { get; set; }

    [Display(Name = "Others")]
    [Range(0, int.MaxValue, ErrorMessage = "Enter valid amount")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OtherBills { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount => HotelBills + TravelBills + MealsBills + OtherBills;

    public ExpenseStatus Status { get; set; } = ExpenseStatus.Submitted;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public int? ApprovedBy { get; set; }
    public string? ApprovalComment { get; set; }

    // Navigation properties
    public virtual ICollection<ExpenseDocument> Documents { get; set; } = new List<ExpenseDocument>();
    public virtual ICollection<ExpenseAudit> Audits { get; set; } = new List<ExpenseAudit>();
}

public enum ExpenseStatus
{
    Draft = 0,
    Submitted = 1,
    Approved = 2,
    Rejected = 3
}