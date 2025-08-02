using Microsoft.EntityFrameworkCore;
using WebTimeSheetManagement.Models;

namespace WebTimeSheetManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TimeSheet> TimeSheets { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<TimeSheetAudit> TimeSheetAudits { get; set; }
    public DbSet<ExpenseAudit> ExpenseAudits { get; set; }
    public DbSet<ExpenseDocument> ExpenseDocuments { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);
            entity.HasIndex(e => e.RoleName).IsUnique();
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
        });

        // Configure Project entity
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId);
            entity.HasIndex(e => e.ProjectCode).IsUnique();
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
        });

        // Configure TimeSheet entity
        modelBuilder.Entity<TimeSheet>(entity =>
        {
            entity.HasKey(e => e.TimeSheetId);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.User)
                .WithMany(u => u.TimeSheets)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Project)
                .WithMany(p => p.TimeSheets)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Expense entity
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Project)
                .WithMany(p => p.Expenses)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure TimeSheetAudit entity
        modelBuilder.Entity<TimeSheetAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.TimeSheet)
                .WithMany(t => t.Audits)
                .HasForeignKey(e => e.TimeSheetId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ExpenseAudit entity
        modelBuilder.Entity<ExpenseAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Expense)
                .WithMany(ex => ex.Audits)
                .HasForeignKey(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ExpenseDocument entity
        modelBuilder.Entity<ExpenseDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId);
            entity.Property(e => e.UploadedOn).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Expense)
                .WithMany(ex => ex.Documents)
                .HasForeignKey(e => e.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Notification entity
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed default roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "SuperAdmin", Description = "Super Administrator with full access" },
            new Role { RoleId = 2, RoleName = "Admin", Description = "Administrator with management access" },
            new Role { RoleId = 3, RoleName = "Manager", Description = "Manager with approval rights" },
            new Role { RoleId = 4, RoleName = "User", Description = "Regular user" }
        );
    }
}