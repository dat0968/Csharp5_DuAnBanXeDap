using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBanXeDap.Models
{
    public class Binhluan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaBinhLuan { get; set; }
        [Required]
        [MaxLength(500)]
        public string NoiDung { get; set; } = string.Empty;
        [Required]
        public DateTime NgayTao { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("SanPham")]
        public int MaSP { get; set; }
        [Required]
        [ForeignKey("KhachHang")]
        public int MaKH { get; set; }
        public double Rating { get; set; }
        public bool IsDelete { get; set; } = false;
        public virtual Sanpham? SanPham { get; set; }
        public virtual Khachhang? KhachHang { get; set; }
        public virtual ICollection<Traloibinhluan>? TraLoiBinhLuans { get; set; } = new List<Traloibinhluan>();
    }
}
