using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class Mausac
{
    public int MaMau { get; set; }

    public string TenMau { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual ICollection<Chitietsanpham> Chitietsanphams { get; set; } = new List<Chitietsanpham>();
}
