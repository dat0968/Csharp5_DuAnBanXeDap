using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class Hoadon
{
    public int MaHoaDon { get; set; }

    public string DiaChiNhanHang { get; set; } = null!;

    public DateOnly NgayTao { get; set; }

    public string Httt { get; set; } = null!;

    public string TinhTrang { get; set; } = null!;

    public int? MaNv { get; set; }

    public int MaKh { get; set; }

    public string? MoTa { get; set; }

    public string? Hoten { get; set; }

    public string? Sdt { get; set; }

    public DateOnly ThoiGianGiao { get; set; }
    public float GiamGiaMaCoupon { get; set; } = 0;
    public float PhiVanChuyen { get; set; } = 0;
    public float TienGoc { get; set; }
    public float TongTien { get; set; }

    public virtual Khachhang MaKhNavigation { get; set; } = null!;

    public virtual Nhanvien? MaNvNavigation { get; set; }
}
