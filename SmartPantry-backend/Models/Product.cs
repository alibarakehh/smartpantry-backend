using System;
using System.Collections.Generic;

namespace SmartPantry_backend.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public string? Category { get; set; }

    public string? ImageUrl { get; set; }

    public int ExpiryDurationSummer { get; set; }

    public int ExpiryDurationWinter { get; set; }

    public virtual ICollection<AvailableProduct> AvailableProducts { get; set; } = new List<AvailableProduct>();

    public virtual ICollection<ProductRecipe> ProductRecipes { get; set; } = new List<ProductRecipe>();
}
