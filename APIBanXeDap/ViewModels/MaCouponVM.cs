using System.ComponentModel.DataAnnotations;

namespace APIBanXeDap.ViewModels
{
    public class MaCouponVM
    {
        public string Code { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 10);
        public decimal? SoTienGiam { get; set; }
        public float? PhanTramGiam { get; set; }
        public DateTime NgayHetHan { get; set; }
        public bool TrangThai { get; set; } = true;
        public decimal? MinimumOrderAmount { get; set; }
    }
}
