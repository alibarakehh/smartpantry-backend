
// RecipesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPantry_backend.Models;

namespace SmartPantry_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RecipesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetAll()
        {
            return await _context.Recipes
                .Include(r => r.ProductRecipes)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> Get(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.ProductRecipes)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            return recipe == null ? NotFound() : Ok(recipe);
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> Create(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = recipe.RecipeId }, recipe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Recipe recipe)
        {
            if (id != recipe.RecipeId) return BadRequest();
            _context.Entry(recipe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return NotFound();
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}