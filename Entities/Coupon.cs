using System;
using System.Collections.Generic;

namespace fastwebsite.Entities;

public partial class Coupon
{
    public int CouponId { get; set; }

    public string Code { get; set; } = null!;

    public int? Discount { get; set; }

    public DateTime? ExpirationDate { get; set; }
}
