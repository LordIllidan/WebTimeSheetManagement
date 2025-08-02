using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimeSheetManagement.Models;

[Table("TimeSheets")]
public class TimeSheet
{
    [Key]
    public int TimeSheetId { get; set; }

    [Required]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual required User User { get; set; }

    [Required]
    public int ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public virtual required Project Project { get; set; }

    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

    // Daily hours for the week (0-24 hours each day)
    [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24")]
    public int MondayHours { get; set; }

    [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24")]
    public int TuesdayHours { get; set; }

    [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24")]
    public int WednesdayHours { get; set; }

    [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24")]
    public int ThursdayHours { get; set; }

    [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24")]
    public int FridayHours { get; set; }

    [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24")]
    public int SaturdayHours { get; set; }

    [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24")]
    public int SundayHours { get; set; }

    public int TotalHours => MondayHours + TuesdayHours + WednesdayHours +
                           ThursdayHours + FridayHours + SaturdayHours + SundayHours;

    [StringLength(500)]
    public string? Comments { get; set; }

    public TimeSheetStatus Status { get; set; } = TimeSheetStatus.Draft;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedOn { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public int? ApprovedBy { get; set; }
    public string? ApprovalComment { get; set; }

    // Navigation properties
    public virtual ICollection<TimeSheetAudit> Audits { get; set; } = new List<TimeSheetAudit>();
}

public enum TimeSheetStatus
{
    Draft = 0,
    Pending = 1,
    Submitted = 1, // Alias for Pending
    Approved = 2,
    Rejected = 3
}