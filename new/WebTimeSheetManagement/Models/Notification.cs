using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTimeSheetManagement.Models;

[Table("Notifications")]
public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    [Required]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual required User User { get; set; }

    [Required]
    [StringLength(200)]
    public required string Title { get; set; }

    [Required]
    [StringLength(1000)]
    public required string Message { get; set; }

    [StringLength(50)]
    public string? Type { get; set; } // Info, Warning, Error, Success

    public bool IsRead { get; set; } = false;

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? ReadOn { get; set; }

    [StringLength(200)]
    public string? ActionUrl { get; set; }
}