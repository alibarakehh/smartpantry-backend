
// AvailableProductsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPantry_backend.Models;

namespace SmartPantry_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailableProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AvailableProductsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvailableProduct>>> GetAll()
        {
            return await _context.AvailableProducts
                .Include(a => a.Product)
                .ToListAsync();
        }

        [HttpGet("expiring-soon")]
        public async Task<ActionResult<IEnumerable<AvailableProduct>>> GetExpiringSoon()
        {
            var threshold = DateOnly.FromDateTime(DateTime.Today.AddDays(3));
            return await _context.AvailableProducts
                .Where(p => p.ExpiryDate.HasValue && p.ExpiryDate.Value <= threshold)
                .Include(p => p.Product)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AvailableProduct>> Get(int id)
        {
            var ap = await _context.AvailableProducts
                .Include(a => a.Product)
                .FirstOrDefaultAsync(a => a.AvailableProductId == id);

            return ap == null ? NotFound() : Ok(ap);
        }

        [HttpPost]
        public async Task<ActionResult<AvailableProduct>> Create(AvailableProduct ap)
        {
            _context.AvailableProducts.Add(ap);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = ap.AvailableProductId }, ap);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AvailableProduct ap)
        {
            if (id != ap.AvailableProductId) return BadRequest();
            _context.Entry(ap).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ap = await _context.AvailableProducts.FindAsync(id);
            if (ap == null) return NotFound();
            _context.AvailableProducts.Remove(ap);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}