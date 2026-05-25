using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
  private readonly AppDbContext _db;

  public CategoriesController(AppDbContext db)
  {
    _db = db;
  }

  // GET api/categories
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Category>>> GetAll()
  {
    return await _db.Categories
        .Include(c => c.Tasks)
        .ToListAsync();
  }

  // GET api/categories/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Category>> GetById(int id)
  {
    var category = await _db.Categories
        .Include(c => c.Tasks)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (category == null) return NotFound();
    return category;
  }

  // POST api/categories
  [HttpPost]
  public async Task<ActionResult<Category>> Create(Category category)
  {
    category.CreatedAt = DateTime.UtcNow;
    _db.Categories.Add(category);
    await _db.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById),
        new { id = category.Id }, category);
  }

  // PUT api/categories/5
  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, Category updated)
  {
    if (id != updated.Id) return BadRequest();
    _db.Entry(updated).State = EntityState.Modified;
    await _db.SaveChangesAsync();
    return NoContent();
  }

  // DELETE api/categories/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var category = await _db.Categories.FindAsync(id);
    if (category == null) return NotFound();
    _db.Categories.Remove(category);
    await _db.SaveChangesAsync();
    return NoContent();
  }
}