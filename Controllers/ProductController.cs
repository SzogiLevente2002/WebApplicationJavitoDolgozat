using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplicationJavitoDolgozat.Entities;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private int categoryId;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Új termék létrehozása
    [HttpPost]
    public async Task<ActionResult<Product>> Create(PostProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            ManufacturedDate = productDto.ManufacturedDate,
            
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // Termékek lekérdezése
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _context.Products
            .Where(b => b.CategoryId == categoryId)
            .ToListAsync();
        return Ok(products);
    }

    // Termék lekérdezése ID alapján
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (product == null)
        {
            return NotFound(new { error = "Product not found." });
        }

        return Ok(product);
    }

    // Termék frissítése
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PutProductDto productDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null )
        {
            return NotFound(new { error = "Product not found." });
        }

        product.Name = productDto.Name;
        product.Price = productDto.Price;
        product.ManufacturedDate = productDto.ManufacturedDate;
        

        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Termék törlése
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null )
        {
            return NotFound(new { error = "Product not found." });
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
