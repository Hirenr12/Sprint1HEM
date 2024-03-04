using System;
using System.Collections.Generic;

namespace Sprint1HEM.Models;

public partial class OrderList
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? ItemId { get; set; }

    public decimal Price { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Item? Item { get; set; }
}
