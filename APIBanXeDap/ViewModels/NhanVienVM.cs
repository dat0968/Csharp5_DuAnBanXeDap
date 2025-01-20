using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace APIBanXeDap.ViewModels
{
    public class NhanVienVM
    {
        public int MaNv { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? Cccd { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public DateOnly NgayVaoLam { get; set; }
        public int Luong { get; set; }
        public string VaiTro { get; set; }
        public string? TenTaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string? TinhTrang { get; set; }
        public bool? IsDelete { get; set; }
        public string? Hinh { get; set; }
        public IFormFile? Anh { get; set; }
    }
}
