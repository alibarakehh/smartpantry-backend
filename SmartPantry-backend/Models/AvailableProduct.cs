using System;
using System.Collections.Generic;

namespace SmartPantry_backend.Models;

public partial class AvailableProduct
{
    public int AvailableProductId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime PurchasingTime { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? OriginalUnit { get; set; }

    public virtual Product? Product { get; set; }


}
