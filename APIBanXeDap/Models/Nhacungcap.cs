using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class Nhacungcap
{
    public int MaNhaCc { get; set; }

    public string TenNhaCc { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
