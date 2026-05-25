using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
  private readonly AppDbContext _db;

  public TasksController(AppDbContext db)
  {
    _db = db;
  }

  // GET api/tasks  — list all tasks
  [HttpGet]
  public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
  {
    return await _db.Tasks.ToListAsync();
  }

  // GET api/tasks/5  — get one task
  [HttpGet("{id}")]
  public async Task<ActionResult<TaskItem>> GetById(int id)
  {
    var task = await _db.Tasks.FindAsync(id);
    if (task == null) return NotFound();
    return task;
  }

  // POST api/tasks  — create task
  [HttpPost]
  public async Task<ActionResult<TaskItem>> Create(TaskItem task)
  {
    task.CreatedAt = DateTime.UtcNow;
    _db.Tasks.Add(task);
    await _db.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById),
        new { id = task.Id }, task);
  }

  // PUT api/tasks/5  — update task
  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, TaskItem updated)
  {
    if (id != updated.Id) return BadRequest();

    _db.Entry(updated).State = EntityState.Modified;

    try
    {
      await _db.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!await _db.Tasks.AnyAsync(t => t.Id == id))
        return NotFound();
      throw;
    }

    return NoContent();
  }

  // PATCH api/tasks/5/complete  — mark as done
  [HttpPatch("{id}/complete")]
  public async Task<IActionResult> MarkComplete(int id)
  {
    var task = await _db.Tasks.FindAsync(id);
    if (task == null) return NotFound();
    task.Status = TaskItemStatus.Done;
    task.UpdatedAt = DateTime.UtcNow;
    await _db.SaveChangesAsync();
    return NoContent();
  }

  // DELETE api/tasks/5  — delete task
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var task = await _db.Tasks.FindAsync(id);
    if (task == null) return NotFound();
    _db.Tasks.Remove(task);
    await _db.SaveChangesAsync();
    return NoContent();
  }
}