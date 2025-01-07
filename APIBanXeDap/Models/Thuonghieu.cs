using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class Thuonghieu
{
    public int MaThuongHieu { get; set; }

    public string TenThuongHieu { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
