
// ProductRecipeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPantry_backend.Models;

namespace SmartPantry_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRecipeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductRecipeController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductRecipe>>> GetAll()
        {
            return await _context.ProductRecipes
                .Include(pr => pr.Product)
                .Include(pr => pr.Recipe)
                .ToListAsync();
        }

        [HttpGet("{productId}/{recipeId}")]
        public async Task<ActionResult<ProductRecipe>> Get(int productId, int recipeId)
        {
            var item = await _context.ProductRecipes
                .Include(pr => pr.Product)
                .Include(pr => pr.Recipe)
                .FirstOrDefaultAsync(pr => pr.ProductId == productId && pr.RecipeId == recipeId);

            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ProductRecipe>> Create(ProductRecipe item)
        {
            _context.ProductRecipes.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { productId = item.ProductId, recipeId = item.RecipeId }, item);
        }

        [HttpDelete("{productId}/{recipeId}")]
        public async Task<IActionResult> Delete(int productId, int recipeId)
        {
            var item = await _context.ProductRecipes.FindAsync(productId, recipeId);
            if (item == null) return NotFound();
            _context.ProductRecipes.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}