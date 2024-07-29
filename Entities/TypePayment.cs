using System;
using System.Collections.Generic;

namespace fastwebsite.Entities;

public partial class TypePayment
{
    public int TypePaymentId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
