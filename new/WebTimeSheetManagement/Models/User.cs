using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimeSheetManagement.Models;

[Table("Users")]
public class User
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Enter Name")]
    [StringLength(100)]
    public required string Name { get; set; }

    [StringLength(100)]
    public string? MobileNumber { get; set; }

    [Required(ErrorMessage = "Email required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(100)]
    public required string Email { get; set; }

    [MinLength(6, ErrorMessage = "Minimum username must be 6 characters")]
    [Required(ErrorMessage = "Username required")]
    [StringLength(50)]
    public required string Username { get; set; }

    [MinLength(7, ErrorMessage = "Minimum password must be 7 characters")]
    [Required(ErrorMessage = "Password required")]
    [StringLength(255)]
    public required string Password { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    [StringLength(100)]
    public string? Designation { get; set; }

    [StringLength(50)]
    public string? Department { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginDate { get; set; }

    public DateTime? BirthDate { get; set; }
    public DateTime? DateOfJoining { get; set; }

    public int? RoleId { get; set; }
    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }

    [StringLength(10)]
    public string? EmployeeId { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool ForceChangePassword { get; set; } = false;

    // Navigation properties
    public virtual ICollection<TimeSheet> TimeSheets { get; set; } = new List<TimeSheet>();
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}