using System.ComponentModel.DataAnnotations;

namespace MVCBanXeDap.ViewModels
{
    public class NhanVienVM
    {
        public int? MaNv { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string? HoTen { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? Sdt { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "CCCD không được để trống")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "CCCD phải từ 9 đến 12 ký tự số")]
        public string? Cccd { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        [StringLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự")]
        public string? TenTaiKhoan { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải ít nhất 6 ký tự")]
        public string? MatKhau { get; set; }

        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }

        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string? DiaChi { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [StringLength(50, ErrorMessage = "Vai trò không được vượt quá 50 ký tự")]
        public string? VaiTro { get; set; }

        [Required(ErrorMessage = "Lương không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Lương phải là số không âm")]
        public int Luong { get; set; }
        public DateTime? NgayVaoLam { get; set; }
        public string? TinhTrang { get; set; }
        public string? Hinh { get; set; }
        public IFormFile? Anh { get; set; }
        public bool? IsDelete { get; set; }
    }
}
