namespace TaskManagerApi.Models;

public class TaskItem
{
  public int Id { get; set; }

  public string Title { get; set; } = string.Empty;

  public string? Description { get; set; }

  public bool IsCompleted { get; set; } = false;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime? DueDate { get; set; }

  // Foreign key
  public int? CategoryId { get; set; }

  // Navigation property
  public Category? Category { get; set; }
}
