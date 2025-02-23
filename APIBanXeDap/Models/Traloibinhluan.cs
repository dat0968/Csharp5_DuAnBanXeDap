using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBanXeDap.Models
{
    public class Traloibinhluan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTraLoi { get; set; }
        [Required]
        [MaxLength(500)]
        public string NoiDung { get; set; } = string.Empty;
        [Required]
        public DateTime ThoiGian { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("BinhLuan")]
        public int MaBinhLuan { get; set; }
        public virtual Binhluan? BinhLuan { get; set; }
        [Required]
        [ForeignKey("NhanVien")]
        public int MaNV { get; set; }
        public virtual Nhanvien? NhanVien { get; set; }
    }
}
