using System.ComponentModel.DataAnnotations;

namespace APIBanXeDap.Models
{
    public class Yeuthich
    {
        [Key]
        public int Ma { get; set; }
        public int MaDoiTuong { get; set; }
        public int MaNguoiDung { get; set; }
        public string DoiTuongYeuThich { get; set; } = "SanPham"; // SanPham / BinhLuan
        public virtual Sanpham? Sanpham { get; set; }

        //public virtual BinhLuan? BinhLuan { get; set; };
        public virtual Khachhang? Khachhang { get; set; }
    }
}
