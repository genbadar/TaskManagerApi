namespace TaskManagerApi.Models;

public class Category
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? Color { get; set; }  // e.g. "#FF5733" for UI color coding
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  // Navigation property — one category has many tasks
  public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}