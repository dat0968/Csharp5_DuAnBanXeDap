using System.ComponentModel.DataAnnotations;

namespace MVCBanXeDap.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(40, ErrorMessage = "Họ và tên chỉ nhận tối đa 40 kí tự")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        [MaxLength(15, ErrorMessage = "Tên tài khoản chỉ nhận tối da 15 kí tự")]
        [MinLength(7, ErrorMessage = "Tên tài khoản chỉ nhận tối thiểu 7 kí tự")]
        public string TenTaiKhoan { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MaxLength(20, ErrorMessage = "Mật khẩu chỉ nhận tối da 20 kí tự")]
        [MinLength(10, ErrorMessage = "Mật khẩu chỉ nhận tối thiểu 10 kí tự")]
        public string MatKhau { get; set; }
    }
}
