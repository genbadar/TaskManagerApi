namespace TaskManagerApi.Models;

public enum TaskItemStatus
{
  Todo,
  InProgress,
  InReview,
  Done,
  Cancelled
}

public enum TaskPriority
{
  Low,
  Medium,
  High,
  Critical
}

public class TaskItem
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string? Description { get; set; }
  public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;
  public TaskPriority Priority { get; set; } = TaskPriority.Medium;
  public DateTime? DueDate { get; set; }
  public DateTime? StartDate { get; set; }
  public bool IsArchived { get; set; } = false;

  // Foreign key
  public int? CategoryId { get; set; }
  public int ProjectId { get; set; }
  public int CreatedByUserId { get; set; }
  public int? AssignedToUserId { get; set; }

  // Navigation properties
  public Category? Category { get; set; }
  public Project Project { get; set; } = null!;
  public User CreatedBy { get; set; } = null!;
  public User? AssignedTo { get; set; }


  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
