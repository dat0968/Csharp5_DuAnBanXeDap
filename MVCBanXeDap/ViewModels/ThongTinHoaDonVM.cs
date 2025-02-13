namespace MVCBanXeDap.ViewModels
{
    public class ThongTinHoaDonVM
    {
        public HoadonVM HoaDon { get; set; }
        public List<ChiTietHoaDonVM> ChiTietHoaDons { get; set; }
        public string? MaCoupon { get; set; }
    }
}
