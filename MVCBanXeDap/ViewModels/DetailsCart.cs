using NuGet.Versioning;

namespace MVCBanXeDap.ViewModels
{
    public class DetailsCart
    {
        public List<CartItem> ListCartItem { get; set; } = new List<CartItem>();
        public decimal GiaGoc => ListCartItem != null && ListCartItem.Count > 0 ? ListCartItem.Sum(p => p.ThanhTien) : 0;
        public string? MaCoupon { get; set; }
        public int? SoTienGiam { get; set; }
        public float? PhanTramGiam { get; set; }
        public decimal GiamGia => SoTienGiam != null ? (decimal)SoTienGiam : PhanTramGiam != null ? (GiaGoc * (decimal)PhanTramGiam / 100) : 0;
        public string? Phuong { get; set; }
        public string? QuanHuyen { get; set; }
        public string? Tinh { get; set; }
        public string DiaChi => Phuong + ", " + QuanHuyen + ", " + Tinh;
        public float? phiship { get; set; }
        public decimal TongTien => GiaGoc - GiamGia + (decimal)(phiship ?? 0);
    }
}
