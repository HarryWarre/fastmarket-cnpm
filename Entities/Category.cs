using System;
using System.Collections.Generic;

namespace fastwebsite.Entities;

public partial class Category
{
    public int CateId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
