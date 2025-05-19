using System;
using System.Collections.Generic;

namespace SmartPantry_backend.Models;

public partial class ProductRecipe
{
    public int ProductId { get; set; }

    public int RecipeId { get; set; }

    public int QuantityRequired { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
