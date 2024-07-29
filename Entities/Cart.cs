using System;
using System.Collections.Generic;

namespace fastwebsite.Entities;

public partial class Cart
{
    public int CartId { get; set; }

    public int? AccountId { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Customer? Account { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
