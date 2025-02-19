using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace APIBanXeDap.ViewModels
{
    public class KhachHangVM
    {
        public int MaKh { get; set; }
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "CCCD không được để trống")]
        public string Cccd { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string Sdt { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        public string TenTaiKhoan { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        //    ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ hoa, chữ thường, ký tự đặc biệt và số")]
        public string MatKhau { get; set; }

        public string? GioiTinh { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? TinhTrang { get; set; }
        public string? Hinh {  get; set; }  
        public IFormFile? Anh { get; set; }
        public bool? IsDelete { get; set; }
    }
}
