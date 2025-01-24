using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIBanXeDap.Models;

public partial class Danhmuc
{
    public int MaDanhMuc { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
