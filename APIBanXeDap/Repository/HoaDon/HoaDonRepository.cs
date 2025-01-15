using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace APIBanXeDap.Repository.HoaDon
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly Csharp5Context _db;
        private IQueryable<Hoadon> dbSet;

        public HoaDonRepository(Csharp5Context db)
        {
            _db = db;
            this.dbSet = _db.Set<Hoadon>();
        }

        public async Task<IEnumerable<Hoadon>> GetAllAsync(Expression<Func<Hoadon, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Hoadon> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<Hoadon> GetAsync(Expression<Func<Hoadon, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Hoadon> query;
            if (tracked)
            {
                query = dbSet;

            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task ChangStatusOrder(int idOrder, int idStaff, string statusOrder)
        {
            var originHoadon = await _db.Hoadons.FirstOrDefaultAsync(x => x.MaHoaDon == idOrder);
            originHoadon.TinhTrang = statusOrder;
            originHoadon.MaNv = idStaff;

            if (statusOrder == "Đã xác nhận")
            {
                originHoadon.ThoiGianGiao = DateOnly.FromDateTime(DateTime.Today.Date);
            }

            _db.Update(originHoadon);
            await _db.SaveChangesAsync();
        }
        public async Task<InvoiceVM> GetInvoiceDataAsync(int maHoaDon)
        {
            // Truy vấn hóa đơn chính
            var invoiceData = await _db.Hoadons
                .Include(i => i.MaKhNavigation)
                .Include(i => i.MaNvNavigation)
                .FirstOrDefaultAsync(i => i.MaHoaDon == maHoaDon);

            if (invoiceData == null)
            {
                throw new Exception("Hóa đơn không tồn tại.");
            }

            // Truy vấn chi tiết hóa đơn
            var invoiceItems = await _db.Chitiethoadons
                .Where(ct => ct.MaHoaDon == maHoaDon)
                .Include(ct => ct.MaSpNavigation) // Include sản phẩm
                .ToListAsync();

            var invoiceViewModel = new InvoiceVM
            {
                MaHoaDon = invoiceData.MaHoaDon,
                DiaChiNhanHang = invoiceData.DiaChiNhanHang,
                TinhTrang = invoiceData.TinhTrang,
                NgayTao = invoiceData.NgayTao.ToDateTime(TimeOnly.MinValue),
                Httt = invoiceData.Httt,
                CustomerName = invoiceData.MaKhNavigation.HoTen,
                CustomerPhone = invoiceData.MaKhNavigation.Sdt,
                CustomerAddress = invoiceData.MaKhNavigation.DiaChi,
                Items = invoiceItems.Select(ct => new InvoiceVM.InvoiceItemViewModel
                {
                    ProductName = ct.MaSpNavigation.TenSp,
                    Quantity = ct.SoLuong,
                    UnitPrice = ct.Gia,
                    Total = ct.ThanhTien
                }).ToList(),
                TotalAmount = invoiceItems.Sum(ct => ct.ThanhTien)
            };

            return invoiceViewModel;
        }

        public async Task<IEnumerable<InvoiceVM>> GetAllInvoiceDataAsync(int maHoaDon)
        {
            // Truy vấn hóa đơn chính
            var invoicesData = _db.Hoadons
                .Include(i => i.MaKhNavigation)
                .Include(i => i.MaNvNavigation);

            if (invoicesData == null)
            {
                throw new Exception("Hóa đơn không tồn tại.");
            }

            // Truy vấn chi tiết hóa đơn
            var invoiceItems = await _db.Chitiethoadons
                .Where(ct => ct.MaHoaDon == maHoaDon)
                .Include(ct => ct.MaSpNavigation) // Include sản phẩm
                .ToListAsync();

            var invoicesViewModel = invoicesData.Select(invoiceData => new InvoiceVM
            {
                MaHoaDon = invoiceData.MaHoaDon,
                DiaChiNhanHang = invoiceData.DiaChiNhanHang,
                TinhTrang = invoiceData.TinhTrang,
                NgayTao = invoiceData.NgayTao.ToDateTime(TimeOnly.MinValue),
                Httt = invoiceData.Httt,
                CustomerName = invoiceData.MaKhNavigation.HoTen,
                CustomerPhone = invoiceData.MaKhNavigation.Sdt,
                CustomerAddress = invoiceData.MaKhNavigation.DiaChi,
                Items = invoiceItems.Select(ct => new InvoiceVM.InvoiceItemViewModel
                {
                    ProductName = ct.MaSpNavigation.TenSp,
                    Quantity = ct.SoLuong,
                    UnitPrice = ct.Gia,
                    Total = ct.ThanhTien
                }).ToList(),
                TotalAmount = invoiceItems.Sum(ct => ct.ThanhTien)
            });

            return invoicesViewModel;
        }


        public async Task<string?> GetOrderStatusById(int maHoaDon)
        {
            Hoadon? hoadon = await _db.Hoadons.FirstOrDefaultAsync(x => x.MaHoaDon == maHoaDon);
            if (hoadon == null) return null;
            return hoadon.TinhTrang;
        }
    }
}
