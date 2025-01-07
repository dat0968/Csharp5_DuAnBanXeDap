using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class Hinhanh
{
    public int MaHinhAnh { get; set; }

    public string HinhAnh1 { get; set; } = null!;

    public int MaSp { get; set; }

    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
