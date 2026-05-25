using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly AppDbContext _db;

  public UsersController(AppDbContext db)
  {
    _db = db;
  }

  // GET api/users
  [HttpGet]
  public async Task<ActionResult<IEnumerable<User>>> GetAll()
  {
    return await _db.Users
        .Select(u => new User
        {
          Id = u.Id,
          FirstName = u.FirstName,
          LastName = u.LastName,
          Email = u.Email,
          Role = u.Role,
          AvatarUrl = u.AvatarUrl,
          TimeZone = u.TimeZone,
          IsActive = u.IsActive,
          EmailVerified = u.EmailVerified,
          LastLoginAt = u.LastLoginAt,
          CreatedAt = u.CreatedAt,
          UpdatedAt = u.UpdatedAt
              // PasswordHash intentionally excluded
            })
        .ToListAsync();
  }

  // GET api/users/5
  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetById(int id)
  {
    var user = await _db.Users
        .Include(u => u.AssignedTasks)
        .FirstOrDefaultAsync(u => u.Id == id);

    if (user == null) return NotFound();

    user.PasswordHash = string.Empty; // never expose hash
    return user;
  }

  // GET api/users/5/tasks
  [HttpGet("{id}/tasks")]
  public async Task<ActionResult<IEnumerable<TaskItem>>> GetUserTasks(int id)
  {
    var exists = await _db.Users.AnyAsync(u => u.Id == id);
    if (!exists) return NotFound();

    return await _db.Tasks
        .Where(t => t.AssignedToUserId == id)
        .Include(t => t.Project)
        .Include(t => t.Category)
        .ToListAsync();
  }

  // POST api/users
  [HttpPost]
  public async Task<ActionResult<User>> Create(User user)
  {
    // In real app: hash the password before saving
    // user.PasswordHash = BCrypt.HashPassword(user.PasswordHash);

    var emailExists = await _db.Users
        .AnyAsync(u => u.Email == user.Email);

    if (emailExists)
      return Conflict(new { message = "Email already in use." });

    user.CreatedAt = DateTime.UtcNow;
    user.UpdatedAt = DateTime.UtcNow;

    _db.Users.Add(user);
    await _db.SaveChangesAsync();

    user.PasswordHash = string.Empty; // don't return hash
    return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
  }

  // PUT api/users/5
  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, User updated)
  {
    if (id != updated.Id) return BadRequest();

    var user = await _db.Users.FindAsync(id);
    if (user == null) return NotFound();

    user.FirstName = updated.FirstName;
    user.LastName = updated.LastName;
    user.AvatarUrl = updated.AvatarUrl;
    user.TimeZone = updated.TimeZone;
    user.Role = updated.Role;
    user.IsActive = updated.IsActive;
    user.UpdatedAt = DateTime.UtcNow;
    // Email and PasswordHash not updated here — separate endpoints

    await _db.SaveChangesAsync();
    return NoContent();
  }

  // PATCH api/users/5/deactivate
  [HttpPatch("{id}/deactivate")]
  public async Task<IActionResult> Deactivate(int id)
  {
    var user = await _db.Users.FindAsync(id);
    if (user == null) return NotFound();

    user.IsActive = false;
    user.UpdatedAt = DateTime.UtcNow;
    await _db.SaveChangesAsync();
    return NoContent();
  }

  // PATCH api/users/5/role
  [HttpPatch("{id}/role")]
  public async Task<IActionResult> UpdateRole(int id, [FromBody] Role role)
  {
    var user = await _db.Users.FindAsync(id);
    if (user == null) return NotFound();

    user.Role = role;
    user.UpdatedAt = DateTime.UtcNow;
    await _db.SaveChangesAsync();
    return NoContent();
  }

  // DELETE api/users/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var user = await _db.Users.FindAsync(id);
    if (user == null) return NotFound();

    _db.Users.Remove(user);
    await _db.SaveChangesAsync();
    return NoContent();
  }
}
