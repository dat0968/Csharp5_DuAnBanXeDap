using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBanXeDap.Models;

public partial class Khachhang
{
    public int MaKh { get; set; }

    public string HoTen { get; set; } = null!;

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? DiaChi { get; set; }
 
    public string? Cccd { get; set; }

    public string? Sdt { get; set; }

    public string Email { get; set; } = null!;

    public string? TenTaiKhoan { get; set; }

    public string? MatKhau { get; set; }

    public string? TinhTrang { get; set; }

    public bool? IsDelete { get; set; }
    public string? Hinh { get;set; }
    [NotMapped]
    public IFormFile? Anh { get; set; }

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
    public virtual ICollection<Yeuthich> YeuThichs { get; set; } = new List<Yeuthich>();
}
