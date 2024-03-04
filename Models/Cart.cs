using System;
using System.Collections.Generic;

namespace Sprint1HEM.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int ItemId { get; set; }

    public int CustomerId { get; set; }

    public decimal Price { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
