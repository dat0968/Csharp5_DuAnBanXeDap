using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APIBanXeDap.Repository.ThanhToan
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly Csharp5Context db;

        public CheckoutRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public void CreateDetailOrder(List<ChiTietHoaDonVM> model)
        {
            foreach (var chitiethoadon in model)
            {
                var detailOrder = new Chitiethoadon
                {
                    MaHoaDon = chitiethoadon.MaHoaDon,
                    MaSp = chitiethoadon.MaSp,
                    MaMau = chitiethoadon.MaMau,
                    MaKichThuoc = chitiethoadon.MaKichThuoc,
                    SoLuong = chitiethoadon.SoLuong,
                    Gia = chitiethoadon.Gia,
                    ThanhTien = chitiethoadon.ThanhTien,
                };
                db.Chitiethoadons.Add(detailOrder);
                db.SaveChanges();               
            }
        }
        public void UpdateQuantityProduct(List<ChiTietHoaDonVM> model)
        {
            foreach(var chitiet in model)
            {
                var Chitietsanpham = db.Chitietsanphams.FirstOrDefault(p => p.MaSp == chitiet.MaSp && p.MaMau == chitiet.MaMau && p.MaKichThuoc == chitiet.MaKichThuoc);
                if (Chitietsanpham != null)
                {
                    Chitietsanpham.SoLuongTon = Chitietsanpham.SoLuongTon - 1;
                    db.Chitietsanphams.Update(Chitietsanpham);
                    db.SaveChanges();
                }
            }            
        }
        public Hoadon CreateOrder(HoadonVM model)
        {
            var Order = new Hoadon
            {
                DiaChiNhanHang = model.DiaChiNhanHang,
                NgayTao = model.NgayTao,
                Httt = model.Httt,
                TinhTrang = model.TinhTrang,
                MaNv = model.MaNv,
                MaKh = model.MaKh,
                MoTa = model.MoTa,
                Hoten = model.Hoten,
                Sdt = model.Sdt,
                ThoiGianGiao = model.ThoiGianGiao,
                PhiVanChuyen = model.PhiVanChuyen,
                GiamGiaMaCoupon = model.GiamGiaMaCoupon,
                TienGoc = model.TienGoc,
                TongTien = model.TongTien,
            };
            db.Hoadons.Add(Order);
            db.SaveChanges();
            return Order;
        }
    }
}
