using System.ComponentModel.DataAnnotations;

namespace APIBanXeDap.Models
{
    public class MaCoupon
    {
        [Key]
        public string Code { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 10);
        public decimal? SoTienGiam { get; set; } 
        public float? PhanTramGiam { get; set; }
        [Required]        
        public DateTime NgayHetHan { get; set; } 
        public bool TrangThai { get; set; } = true;
        [Required]
        public decimal? MinimumOrderAmount { get; set; }
        [Required]
        public DateTime NgayTao { get; set; } = DateTime.Now;
        [Required]
        public bool DaSuDung = false;
    }
}
