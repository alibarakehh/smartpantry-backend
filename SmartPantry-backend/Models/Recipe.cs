using System;
using System.Collections.Generic;

namespace SmartPantry_backend.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? CookingTime { get; set; }

    public int? Servings { get; set; }

    public string? Origin { get; set; }

    public virtual ICollection<ProductRecipe> ProductRecipes { get; set; } = new List<ProductRecipe>();
}
