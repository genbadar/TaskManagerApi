namespace TaskManagerApi.Models;

public enum Role
{
  Admin,
  Manager,
  Member,
  Guest
}
public class User
{
  public int Id { get; set; }
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string PasswordHash { get; set; } = string.Empty;
  public Role Role { get; set; } = Role.Member;
  public string? AvatarUrl { get; set; }
  public string? TimeZone { get; set; }
  public bool IsActive { get; set; } = true;
  public bool EmailVerified { get; set; } = false;
  public DateTime? LastLoginAt { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  // Navigation properties
  public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
  public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
}