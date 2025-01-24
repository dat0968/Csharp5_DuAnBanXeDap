using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APIBanXeDap.Repository.ChiTietSanPham
{
    public class ProductDetailsRepository : IProductDetailsRepository
    {
        private readonly Csharp5Context db;

        public ProductDetailsRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public DetailsProductEM CreateDetailsProduct(DetailsProductEM detailsproduct)
        {
            if (detailsproduct.MaSP == 0)
            {
                detailsproduct.MaSP = db.Sanphams.OrderBy(p => p.MaSp).LastOrDefault().MaSp;
            }
            var NewDetailsProducts = new Chitietsanpham
            {
                MaSp = detailsproduct.MaSP,
                MaKichThuoc = detailsproduct.MaKichThuoc,
                MaMau = detailsproduct.MaMau,
                SoLuongTon = detailsproduct.SoLuongTon,
                DonGia = detailsproduct.DonGia,
            };
            db.Chitietsanphams.Add(NewDetailsProducts);
            db.SaveChanges();
            return detailsproduct;
        }

        public void DeleteDetailsProduct(int MaSP, int MaMau, int MaKichThuoc)
        {
            var findDetailsProduct = db.Chitietsanphams.FirstOrDefault(details => details.MaSp == MaSP &&
            details.MaKichThuoc == MaKichThuoc && details.MaMau == MaMau);
            db.Chitietsanphams.Remove(findDetailsProduct);
            db.SaveChanges();
        }

        public void EditDetailsProduct(DetailsProductEM detailsproduct)
        {
            var findDetailsProduct = db.Chitietsanphams.FirstOrDefault(details => details.MaSp == detailsproduct.MaSP && 
            details.MaKichThuoc == detailsproduct.MaKichThuoc && details.MaMau == detailsproduct.MaMau);
            findDetailsProduct.SoLuongTon = detailsproduct.SoLuongTon;
            findDetailsProduct.DonGia = detailsproduct.DonGia;
            db.Chitietsanphams.Update(findDetailsProduct);
            db.SaveChanges();
        }

        public List<DetailsProductVM> GetAllDetailsProduct()
        {
            var ListDetailsProduct = db.Chitietsanphams
                .Include(p => p.MaSpNavigation)
                .Include(p => p.MaKichThuocNavigation)
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaKichThuocNavigation).ToList();
            var ListDetailsProductVM = new List<DetailsProductVM>();
            foreach(var item in ListDetailsProduct)
            {
                ListDetailsProductVM.Add(new DetailsProductVM
                {
                    TenSP = item.MaSpNavigation.TenSp,
                    TenMau = item.MaMauNavigation.TenMau,
                    TenKichThuoc = item.MaKichThuocNavigation.TenKichThuoc,
                    SoLuongTon = item.SoLuongTon,
                    DonGia = item.DonGia
                });
            };
            return ListDetailsProductVM;
        }
    }
}
