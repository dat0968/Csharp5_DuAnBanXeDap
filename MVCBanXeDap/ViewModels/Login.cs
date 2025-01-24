using System.ComponentModel.DataAnnotations;

namespace MVCBanXeDap.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "Email hoặc tên tài khoản không được để trống")]
        public string Email_TenTaiKhoan { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; }
    }
}
