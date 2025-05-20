// Controllers/RecommendedRecipesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPantry_backend;
using SmartPantry_backend.Models;

[Route("api/[controller]")]
[ApiController]
public class RecommendedRecipesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RecommendedRecipesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipe>>> GetRecommendedRecipes()
    {
        var recipes = await _context.Recipes
            .Include(r => r.ProductRecipes)
                .ThenInclude(pr => pr.Product)
            .ToListAsync();

        return Ok(recipes);
    }

}
