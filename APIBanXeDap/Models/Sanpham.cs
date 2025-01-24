using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class Sanpham
{
    public int MaSp { get; set; }

    public string TenSp { get; set; } = null!;

    public int MaThuongHieu { get; set; }

    public string Hinh { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public DateOnly NgaySanXuat { get; set; }

    public int MaNhaCc { get; set; }

    public int MaDanhMuc { get; set; }

    public bool? IsDelete { get; set; }

    public virtual ICollection<Chitietsanpham> Chitietsanphams { get; set; } = new List<Chitietsanpham>();

    public virtual ICollection<Hinhanh> Hinhanhs { get; set; } = new List<Hinhanh>();

    public virtual Danhmuc MaDanhMucNavigation { get; set; } = null!;

    public virtual Nhacungcap MaNhaCcNavigation { get; set; } = null!;

    public virtual Thuonghieu MaThuongHieuNavigation { get; set; } = null!;
}
