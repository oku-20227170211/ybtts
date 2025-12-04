using Microsoft.EntityFrameworkCore;
using YBTTS.Core.Entities;

namespace YBTTS.Infrastructure.Data;

public class YbttsDbContext : DbContext
{
    public YbttsDbContext(DbContextOptions<YbttsDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Request> Requests { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<MaintenanceStaff> MaintenanceStaffs { get; set; } = null!;
    public DbSet<Badge> Badges { get; set; } = null!;
    public DbSet<StudentBadge> StudentBadges { get; set; } = null!;
    public DbSet<Feedback> Feedbacks { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Student-Request relationship
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Requests)
            .WithOne(r => r.Student)
            .HasForeignKey(r => r.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Request-MaintenanceStaff relationship
        modelBuilder.Entity<Request>()
            .HasOne(r => r.AssignedToStaff)
            .WithMany(m => m.AssignedRequests)
            .HasForeignKey(r => r.AssignedToStaffId)
            .OnDelete(DeleteBehavior.SetNull);

        // Student-Feedback relationship
        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Student)
            .WithMany(s => s.Feedbacks)
            .HasForeignKey(f => f.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Request-Feedback relationship
        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Request)
            .WithMany(r => r.Feedbacks)
            .HasForeignKey(f => f.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Student-Badge many-to-many relationship
        modelBuilder.Entity<StudentBadge>()
            .HasOne(sb => sb.Student)
            .WithMany(s => s.StudentBadges)
            .HasForeignKey(sb => sb.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudentBadge>()
            .HasOne(sb => sb.Badge)
            .WithMany(b => b.StudentBadges)
            .HasForeignKey(sb => sb.BadgeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Student-Notification relationship
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Student)
            .WithMany(s => s.Notifications)
            .HasForeignKey(n => n.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // MaintenanceStaff-Notification relationship
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.MaintenanceStaff)
            .WithMany(m => m.Notifications)
            .HasForeignKey(n => n.MaintenanceStaffId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed initial badges
        modelBuilder.Entity<Badge>().HasData(
            new Badge { Id = 1, Name = "İlk Adım\", Description = \"İlk arıza talebini oluşturdunuz!\", Criteria = \"İlk talep\", IconUrl = \"🎯\", RequiredPoints = 0 "},
            new Badge { Id = 2, Name = "Aktif Üye\", Description = \"5 arıza talebi oluşturdunuz\", Criteria = \"5 talep\", IconUrl = \"⭐\", RequiredPoints = 50" },
            new Badge { Id = 3, Name = "Geri Bildirim Ustası\", Description = \"10 geri bildirim verdiniz\", Criteria = \"10 feedback\", IconUrl = \"💬\", RequiredPoints = 50" },
            new Badge { Id = 4, Name = "Yıldız Öğrenci\", Description = \"Level 3'e ulaştınız\", Criteria = \"Level 3\", IconUrl = \"🌟\", RequiredPoints = 150 "},
            new Badge { Id = 5, Name = "Yurt Kahramanı\", Description = \"Level 5'e ulaştınız\", Criteria = \"Level 5\", IconUrl = \"🏆\", RequiredPoints = 500 "}
        );
    }
}