using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

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
            foreach(var chitiethoadon in model)
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
