using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.VanChuyen
{
    public interface IShippingRepository
    {
        public List<ShippingVM> GetAll(string? keywords, string? priceFilter, string? SortByPrice);
        public float? GetShippingFee(string pho, string quan, string phuong);
        public ShippingVM Create(ShippingVM ship);
        public void Delete(int id);
        public void Edit(ShippingVM ship);
    }
}
