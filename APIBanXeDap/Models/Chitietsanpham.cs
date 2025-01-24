using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class Chitietsanpham
{
    public int MaSp { get; set; }

    public int MaMau { get; set; }

    public int MaKichThuoc { get; set; }

    public int SoLuongTon { get; set; }

    public double DonGia { get; set; }

    public virtual SizeVM MaKichThuocNavigation { get; set; } = null!;

    public virtual Mausac MaMauNavigation { get; set; } = null!;

    public virtual Sanpham MaSpNavigation { get; set; } = null!;
}
