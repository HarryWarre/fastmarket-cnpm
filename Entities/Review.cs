using System;
using System.Collections.Generic;

namespace fastwebsite.Entities;

public partial class Review
{
    public int AccountId { get; set; }

    public int ProductId { get; set; }

    public string? Content { get; set; }

    public decimal? Rates { get; set; }

    public virtual Customer Account { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
