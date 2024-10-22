using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplicationJavitoDolgozat.Entities;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Új kategória létrehozása
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Category>> Create(Category category)
    {
        category.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    // Kategóriák lekérdezése
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var categories = await _context.Categories
            .Where(a => a.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            .ToListAsync();
        return Ok(categories);
    }

    // Kategória lekérdezése ID alapján
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null || category.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return NotFound();
        }
        return Ok(category);
    }

    // Kategória frissítése
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, Category category)
    {
        if (id != category.Id || category.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return BadRequest();
        }

        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Kategória törlése
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null || category.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
