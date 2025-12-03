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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Requests)
            .WithOne(r => r.Student)
            .HasForeignKey(r => r.StudentId);
    }
}
