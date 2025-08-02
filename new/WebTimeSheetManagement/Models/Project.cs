using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimeSheetManagement.Models;

[Table("Projects")]
public class Project
{
    [Key]
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "Enter project code")]
    [StringLength(20)]
    public required string ProjectCode { get; set; }

    [Required(ErrorMessage = "Enter nature of industry")]
    [StringLength(100)]
    public required string NatureOfIndustry { get; set; }

    [Required(ErrorMessage = "Enter project name")]
    [StringLength(200)]
    public required string ProjectName { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? ClientName { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Budget { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<TimeSheet> TimeSheets { get; set; } = new List<TimeSheet>();
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}