using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options) { }

  public DbSet<User> Users { get; set; }
  public DbSet<Project> Projects { get; set; }
  public DbSet<TaskItem> Tasks { get; set; }
  public DbSet<Category> Categories { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // User — unique email
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

    // Role stored as string not int
    modelBuilder.Entity<User>()
        .Property(u => u.Role)
        .HasConversion<string>();

    // TaskStatus and TaskPriority stored as string
    modelBuilder.Entity<TaskItem>()
        .Property(t => t.Status)
        .HasConversion<string>();

    modelBuilder.Entity<TaskItem>()
        .Property(t => t.Priority)
        .HasConversion<string>();

    // TaskItem — CreatedBy relationship
    modelBuilder.Entity<TaskItem>()
        .HasOne(t => t.CreatedBy)
        .WithMany(u => u.CreatedTasks)
        .HasForeignKey(t => t.CreatedByUserId)
        .OnDelete(DeleteBehavior.Restrict);

    // TaskItem — AssignedTo relationship
    modelBuilder.Entity<TaskItem>()
        .HasOne(t => t.AssignedTo)
        .WithMany(u => u.AssignedTasks)
        .HasForeignKey(t => t.AssignedToUserId)
        .OnDelete(DeleteBehavior.SetNull);

  }
}
