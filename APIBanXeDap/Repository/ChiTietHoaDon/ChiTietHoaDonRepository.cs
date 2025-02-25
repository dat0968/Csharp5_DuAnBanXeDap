using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace APIBanXeDap.Repository.ChiTietHoaDon
{
    public class ChiTietHoaDonRepository : IChiTietHoaDonRepository
    {
        private readonly Csharp5Context _db;
        private IQueryable<Chitiethoadon> dbSet;

        public ChiTietHoaDonRepository(Csharp5Context db)
        {
            _db = db;
            this.dbSet = _db.Set<Chitiethoadon>();
        }
        public async Task<List<ChiTietHoaDonVM>> GetAllDetailInvoiceAsync(Expression<Func<Chitiethoadon, bool>>? filter = null)
        {
            IQueryable<Chitiethoadon> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Lấy dữ liệu từ cơ sở dữ liệu
            var chitiethoadonList = await query.ToListAsync();
            var sanPhamList = await _db.Sanphams.ToListAsync();
            var mauList = await _db.Mausacs.ToListAsync();
            var kichthuocList = await _db.Kichthuocs.ToListAsync();

            // Chuyển đổi sang ChiTietHoaDonVM
            var result = chitiethoadonList.Select(chitiet =>
            {
                Sanpham? sp = sanPhamList.FirstOrDefault(x => x.MaSp == chitiet.MaSp);
                Mausac? mausac = mauList.FirstOrDefault(x => x.MaMau == chitiet.MaMau);
                SizeVM? sizeVM = kichthuocList.FirstOrDefault(x => x.MaKichThuoc == chitiet.MaKichThuoc);
                return new ChiTietHoaDonVM
                {
                    MaHoaDon = chitiet.MaHoaDon,
                    MaSp = chitiet.MaSp,
                    TenSp = sp?.TenSp ?? "Không xác định",
                    MaMau = chitiet.MaMau,
                    TenMau = mausac?.TenMau ?? "Không xác định", 
                    MaKichThuoc = chitiet.MaKichThuoc,
                    TenKichThuoc = sizeVM?.TenKichThuoc ?? "Không xác định", 
                    SoLuong = chitiet.SoLuong,
                    Gia = chitiet.Gia,
                    ThanhTien = chitiet.ThanhTien,
                    MoTa = sp?.MoTa ?? "Không xác định",
                    Hinh = sp?.Hinh ?? "",                
                };
            }).ToList();
            return result;
        }


        public async Task<ChiTietHoaDonVM> GetDetailInvoiceByIdAsync(Expression<Func<Chitiethoadon, bool>> filter)
        {
            IQueryable<Chitiethoadon> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Lấy dữ liệu từ cơ sở dữ liệu
            var chitiethoadon = await query.FirstOrDefaultAsync();

            // Chuyển đổi sang ChiTietHoaDonVM
            var result = new ChiTietHoaDonVM
            {
                MaHoaDon = chitiethoadon.MaHoaDon,
                MaSp = chitiethoadon.MaSp,
                TenSp = chitiethoadon.MaSpNavigation.TenSp,
                MaMau = chitiethoadon.MaMau,
                TenMau = chitiethoadon.MaMauNavigation.TenMau,
                MaKichThuoc = chitiethoadon.MaKichThuoc,
                TenKichThuoc = chitiethoadon.MaKichThuocNavigation.TenKichThuoc,
                SoLuong = chitiethoadon.SoLuong,
                Gia = chitiethoadon.Gia,
                ThanhTien = chitiethoadon.ThanhTien,
                MoTa = chitiethoadon.MaSpNavigation.MoTa,
                Hinh = chitiethoadon.MaSpNavigation.Hinh
            };

            return result;
        }
    }
}
