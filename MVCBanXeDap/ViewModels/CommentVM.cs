using System.ComponentModel.DataAnnotations;

namespace MVCBanXeDap.ViewModels
{
    public class CommentVM
    {
        public int MaBinhLuan { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [MaxLength(500, ErrorMessage = "Nội dung không được vượt quá 500 ký tự")]
        public string NoiDung { get; set; } = string.Empty;

        [Display(Name = "Ngày tạo")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Mã khách hàng không được để trống")]
        public int MaKH { get; set; }

        [Range(0, 5, ErrorMessage = "Đánh giá phải từ 0 đến 5")]
        public double Rating { get; set; }

        public bool IsDelete { get; set; } = false;

        // Thêm trường hiển thị tên sản phẩm và tên khách hàng nếu cần
        public string? TenSanPham { get; set; }
        public string? TenKhachHang { get; set; }
    }
}
