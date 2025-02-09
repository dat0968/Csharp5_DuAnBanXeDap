using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.ThanhToan
{
    public interface ICheckoutRepository
    {
        public void CreateOrder(HoadonVM model);
        public void CreateDetailOrder(List<ChiTietHoaDonVM> model);
    }
}
