using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.TrangChu
{
    public interface ITrangChuRepository
    {
        public List<ProductVM> GetSanphamLienQuan(string tenDM);
        public List<ProductVM> GetSanPhamBanChay();
        public List<ProductVM> GetAllProduct(string? keywords, int? MaDanhMuc, int? MaThuongHieu, string? sort, double? giaMin, double? giaMax);
        public ProductVM GetProductById(int id);
        public List<Thuonghieu> GetAllBrand(string? keywords, string? sort);
        public List<Danhmuc> GetAllCategory();
    }   
}
