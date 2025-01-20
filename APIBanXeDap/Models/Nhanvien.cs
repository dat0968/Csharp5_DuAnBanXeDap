using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBanXeDap.Models;

public partial class Nhanvien
{
    public int MaNv { get; set; }

    public string HoTen { get; set; } = null!;

    public string GioiTinh { get; set; } = null!;

    public DateOnly? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string? Cccd { get; set; }

    public string Sdt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly NgayVaoLam { get; set; }

    public int Luong { get; set; }

    public string VaiTro { get; set; } = null!;

    public string? TenTaiKhoan { get; set; }

    public string MatKhau { get; set; } = null!;

    public string? TinhTrang { get; set; }

    public bool? IsDelete { get; set; }
    public string? Hinh { get; set; }
    [NotMapped]
    public IFormFile? Anh { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
}
