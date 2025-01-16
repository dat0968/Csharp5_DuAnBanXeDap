using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.TrangChu
{
    public interface ITrangChuRepository
    {
        //public List<Sanpham> GetSanphams();
        public List<ProductVM> GetSanPhamBanChay();
    }   
}
