using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
  private readonly AppDbContext _db;

  public ProjectsController(AppDbContext db)
  {
    _db = db;
  }

  // GET api/projects
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Project>>> GetAll()
  {
    return await _db.Projects
        .Where(p => !p.IsArchived)
        .Include(p => p.Tasks)
        .ToListAsync();
  }

  // GET api/projects/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Project>> GetById(int id)
  {
    var project = await _db.Projects
        .Include(p => p.Tasks)
            .ThenInclude(t => t.AssignedTo)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (project == null) return NotFound();
    return project;
  }

  // GET api/projects/5/tasks
  [HttpGet("{id}/tasks")]
  public async Task<ActionResult<IEnumerable<TaskItem>>> GetProjectTasks(
      int id,
      [FromQuery] TaskItemStatus? status,
      [FromQuery] TaskPriority? priority)
  {
    var exists = await _db.Projects.AnyAsync(p => p.Id == id);
    if (!exists) return NotFound();

    var query = _db.Tasks
        .Where(t => t.ProjectId == id && !t.IsArchived)
        .Include(t => t.AssignedTo)
        .Include(t => t.Category)
        .AsQueryable();

    if (status.HasValue)
      query = query.Where(t => t.Status == status.Value);

    if (priority.HasValue)
      query = query.Where(t => t.Priority == priority.Value);

    return await query.ToListAsync();
  }

  // POST api/projects
  [HttpPost]
  public async Task<ActionResult<Project>> Create(Project project)
  {
    project.CreatedAt = DateTime.UtcNow;
    project.UpdatedAt = DateTime.UtcNow;

    _db.Projects.Add(project);
    await _db.SaveChangesAsync();

    return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
  }

  // PUT api/projects/5
  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, Project updated)
  {
    if (id != updated.Id) return BadRequest();

    var project = await _db.Projects.FindAsync(id);
    if (project == null) return NotFound();

    project.Name = updated.Name;
    project.Description = updated.Description;
    project.Color = updated.Color;
    project.UpdatedAt = DateTime.UtcNow;

    await _db.SaveChangesAsync();
    return NoContent();
  }

  // PATCH api/projects/5/archive
  [HttpPatch("{id}/archive")]
  public async Task<IActionResult> Archive(int id)
  {
    var project = await _db.Projects.FindAsync(id);
    if (project == null) return NotFound();

    project.IsArchived = true;
    project.UpdatedAt = DateTime.UtcNow;
    await _db.SaveChangesAsync();
    return NoContent();
  }

  // DELETE api/projects/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var project = await _db.Projects.FindAsync(id);
    if (project == null) return NotFound();

    _db.Projects.Remove(project);
    await _db.SaveChangesAsync();
    return NoContent();
  }
}
