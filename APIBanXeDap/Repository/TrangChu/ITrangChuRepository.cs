using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.TrangChu
{
    public interface ITrangChuRepository
    {
        public List<ProductVM> GetSanphamLienQuan(string tenDM);
        public List<ProductVM> GetSanPhamBanChay();
    }   
}
