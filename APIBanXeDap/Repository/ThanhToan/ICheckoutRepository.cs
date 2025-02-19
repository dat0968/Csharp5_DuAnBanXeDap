using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.ThanhToan
{
    public interface ICheckoutRepository
    {
        public Hoadon CreateOrder(HoadonVM model);
        public void UpdateQuantityProduct(List<ChiTietHoaDonVM> model);
        public void CreateDetailOrder(List<ChiTietHoaDonVM> model);
    }
}
