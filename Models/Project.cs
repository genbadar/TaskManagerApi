namespace TaskManagerApi.Models;

public class Project
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public string? Color { get; set; }
  public bool IsArchived { get; set; } = false;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  // Navigation properties
  public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
