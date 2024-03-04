using System;
using System.Collections.Generic;

namespace Sprint1HEM.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();
}
