using System;
using System.Collections.Generic;

namespace fastwebsite.Entities;

public partial class SalesStaff
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte AdminState { get; set; }
}
