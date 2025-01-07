using APIBanXeDap.EditModels;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.ChiTietSanPham
{
    public interface IProductDetailsRepository
    {
        public List<DetailsProductVM> GetAllDetailsProduct();
        public DetailsProductEM CreateDetailsProduct(DetailsProductEM detailsproduct);
        public void EditDetailsProduct(DetailsProductEM detailsproduct);
        public void DeleteDetailsProduct(int MaSP, int MaMau, int MaKichThuoc);
    }
}
