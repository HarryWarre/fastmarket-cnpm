using System;
using System.Collections.Generic;

namespace fastwebsite.Entities;

public partial class Banner
{
    public int BannerId { get; set; }

    public string? Img { get; set; }

    public string? Link { get; set; }
}
