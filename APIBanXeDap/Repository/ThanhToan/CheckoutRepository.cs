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
            db.Database.BeginTransaction();
            try
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

                    var Chitietsanpham = db.Chitietsanphams.FirstOrDefault(p => p.MaSp == detailOrder.MaSp && p.MaMau == detailOrder.MaMau && p.MaKichThuoc == detailOrder.MaKichThuoc);
                    if(Chitietsanpham != null)
                    {
                        Chitietsanpham.SoLuongTon = Chitietsanpham.SoLuongTon - 1;
                        db.Database.CommitTransaction();
                    }
                    else
                    {
                        throw new Exception($"Không tìm thấy sản phẩm với MaSp: {detailOrder.MaSp}, MaMau: {detailOrder.MaMau}, MaKichThuoc: {detailOrder.MaKichThuoc}");
                    }
                }

                
            }catch (Exception ex)
            {
                db.Database.RollbackTransaction();
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
