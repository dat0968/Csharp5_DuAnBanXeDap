using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class Chitiethoadon
{
    public int MaHoaDon { get; set; }

    public int MaSp { get; set; }

    public int MaMau { get; set; }

    public int MaKichThuoc { get; set; }
    public float GiamGiaMaCoupon { get; set; } = 0;
    public float PhiVanChuyen { get; set; } = 0;

    public int SoLuong { get; set; }

    public decimal Gia { get; set; }

    public decimal ThanhTien { get; set; }

    public virtual Hoadon MaHoaDonNavigation { get; set; } = null!;

    public virtual SizeVM MaKichThuocNavigation { get; set; } = null!;

    public virtual Mausac MaMauNavigation { get; set; } = null!;

    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
