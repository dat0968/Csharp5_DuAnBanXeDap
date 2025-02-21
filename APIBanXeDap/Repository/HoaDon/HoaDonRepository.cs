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

        // Phương thức để lấy tất cả hóa đơn dưới dạng HoadonVM
        public async Task<IEnumerable<HoadonVM>> GetAllHoadonVMAsync(Expression<Func<Hoadon, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Hoadon> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            // Lấy dữ liệu từ cơ sở dữ liệu
            var hoadonList = await query.ToListAsync();

            // Chuyển đổi danh sách Hoadon sang HoadonVM
            return hoadonList.Select(hoadon => new HoadonVM
            {
                MaHoaDon = hoadon.MaHoaDon,
                DiaChiNhanHang = hoadon.DiaChiNhanHang,
                NgayTao = hoadon.NgayTao,
                Httt = hoadon.Httt,
                TinhTrang = hoadon.TinhTrang,
                MaNv = hoadon.MaNv,
                MaKh = hoadon.MaKh,
                MoTa = hoadon.MoTa,
                Hoten = hoadon.Hoten,
                Sdt = hoadon.Sdt,
                ThoiGianGiao = hoadon.ThoiGianGiao,
                LyDoHuy = hoadon.LyDoHuy
            }).ToList();
        }

        public async Task<HoadonVM> GetAsync(Expression<Func<Hoadon, bool>> filter, string? includeProperties = null, bool tracked = false)
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

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            var hoadon = await query.FirstOrDefaultAsync();
            HoadonVM hoadonVM = new HoadonVM
            {
                MaHoaDon = hoadon.MaHoaDon,
                DiaChiNhanHang = hoadon.DiaChiNhanHang,
                NgayTao = hoadon.NgayTao,
                Httt = hoadon.Httt,
                TinhTrang = hoadon.TinhTrang,
                MaNv = hoadon.MaNv,
                MaKh = hoadon.MaKh,
                MoTa = hoadon.MoTa,
                Hoten = hoadon.Hoten,
                Sdt = hoadon.Sdt,
                ThoiGianGiao = hoadon.ThoiGianGiao,
                LyDoHuy = hoadon.LyDoHuy
            };
            return hoadonVM;
        }
        public async Task<string?> ChangeStatusOrder(int idOrder, int idStaff, string statusOrder, string? reason)
        {
            var originHoadon = await _db.Hoadons.FirstOrDefaultAsync(x => x.MaHoaDon == idOrder);
            if (originHoadon == null)
            {
                // Xử lý trường hợp không tìm thấy hóa đơn (nếu cần)
                return null; // Trả về null nếu không tìm thấy hóa đơn
            }

            // Kiểm tra xem nhân viên có quyền thay đổi không
            if (!String.IsNullOrEmpty(originHoadon.MaNv.ToString()) && originHoadon.MaNv != idStaff)
            {
                return null; // Trả về null nếu nhân viên không có quyền thay đổi
            }

            originHoadon.TinhTrang = statusOrder;
            originHoadon.MaNv = idStaff;
            if (new string[] {
                    "Hoàn trả/Hoàn tiền",
                    "Đã hủy"}.Contains(statusOrder))
            {
                if (reason is not null)
                {
                    originHoadon.LyDoHuy = reason;
                } else
                {
                    originHoadon.LyDoHuy = $"Đơn hàng đã bị đổi thành tình trạng [{statusOrder}] bởi nhân viên [Mã nhân viên: {idStaff}].";
                }
            }
            // Cập nhật thời gian giao hàng nếu cần
            if (statusOrder == "Đã xác nhận")
            {
                originHoadon.ThoiGianGiao = DateOnly.FromDateTime(DateTime.Today);
            }

            _db.Update(originHoadon);
            await _db.SaveChangesAsync();

            return originHoadon.TinhTrang; // Trả về tình trạng mới của hóa đơn
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
            // Lấy thông tin nhân viên đảm nhiệm công việc
            var staffForce = await _db.Nhanviens.FirstOrDefaultAsync(nv => nv.MaNv == invoiceData.MaNv);

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
                ThoiGianGiao = invoiceData.ThoiGianGiao.ToDateTime(TimeOnly.MinValue),
                HinhThucThanhToan = invoiceData.Httt,
                TenKhachHang = invoiceData.MaKhNavigation.HoTen,
                MaNhanVien = invoiceData.MaNv ?? 0,
                TenNhanVien = staffForce?.HoTen ?? "Đơn hàng chưa có nhân viên phụ trách",
                SoDienThoaiKhachHang = invoiceData.MaKhNavigation.Sdt,
                DiaChiKhachHang = invoiceData.MaKhNavigation.DiaChi,
                LyDoHuy = invoiceData.LyDoHuy,
                Items = invoiceItems.Select(ct => new InvoiceVM.ChiTietHoaDonViewModel
                {
                    TenSanPham = ct.MaSpNavigation.TenSp,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.Gia,
                    Tong = ct.ThanhTien
                }).ToList(),
                TongTien = invoiceItems.Sum(ct => ct.ThanhTien)
            };

            return invoiceViewModel;
        }
        public async Task<IEnumerable<InvoiceVM>> GetAllInvoiceDataAsync()
        {
            // Truy vấn hóa đơn chính
            var invoicesData = await _db.Hoadons
                .Include(i => i.MaKhNavigation) // Bao gồm thông tin khách hàng
                .Include(i => i.MaNvNavigation) // Bao gồm thông tin nhân viên
                .ToListAsync(); // Lấy dữ liệu dưới dạng danh sách

            if (invoicesData == null || !invoicesData.Any())
            {
                throw new Exception("Không tìm thấy hóa đơn nào.");
            }
            // Lấy thông tin danh sách nhân viên đảm nhiệm đơn hàng
            var idsStaff = invoicesData.DistinctBy(iv => iv.MaNv).Select(iv => iv.MaNv);
            var staffsForce = await _db.Nhanviens.Where(nv => idsStaff.Contains(nv.MaNv)).ToListAsync();

            // Truy vấn chi tiết hóa đơn
            var invoiceItems = await _db.Chitiethoadons
                .Include(ct => ct.MaSpNavigation) // Bao gồm thông tin sản phẩm
                .ToListAsync();

            var invoicesViewModel = invoicesData.Select(invoiceData => new InvoiceVM
            {
                MaHoaDon = invoiceData.MaHoaDon,
                DiaChiNhanHang = invoiceData.DiaChiNhanHang,
                TinhTrang = invoiceData.TinhTrang,
                NgayTao = invoiceData.NgayTao.ToDateTime(TimeOnly.MinValue),
                ThoiGianGiao = invoiceData.ThoiGianGiao.ToDateTime(TimeOnly.MinValue),
                HinhThucThanhToan = invoiceData.Httt,
                TenKhachHang = invoiceData.MaKhNavigation.HoTen,
                MaNhanVien = invoiceData.MaNv ?? 0,
                TenNhanVien = staffsForce.FirstOrDefault(sf => sf.MaNv == invoiceData.MaNv)?.TenTaiKhoan ?? "Đơn hàng chưa có nhân viên phụ trách",
                SoDienThoaiKhachHang = invoiceData.MaKhNavigation.Sdt,
                DiaChiKhachHang = invoiceData.MaKhNavigation.DiaChi,
                Items = invoiceItems.Where(ct => ct.MaHoaDon == invoiceData.MaHoaDon).Select(ct => new InvoiceVM.ChiTietHoaDonViewModel
                {
                    TenSanPham = ct.MaSpNavigation.TenSp,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.Gia,
                    Tong = ct.ThanhTien
                }).ToList(),
                TongTien = invoiceItems
                    .Where(ct => ct.MaHoaDon == invoiceData.MaHoaDon) // Tính tổng tiền cho hóa đơn hiện tại
                    .Sum(ct => ct.ThanhTien)
            });

            return invoicesViewModel;
        }



        public async Task<string?> GetOrderStatusById(int maHoaDon)
        {
            Hoadon? hoadon = await _db.Hoadons.FirstOrDefaultAsync(x => x.MaHoaDon == maHoaDon);
            if (hoadon == null) return null;
            return hoadon.TinhTrang;
        }
/*
        public async Task<bool> SetReasonCancel(int maHoaDon, string reason)
        {
            Hoadon? hoadon = await _db.Hoadons.FirstOrDefaultAsync(x => x.MaHoaDon == maHoaDon);
            if (hoadon is null) return false;
            hoadon.LyDoHuy = reason;

            _db.Update(hoadon);

            await _db.SaveChangesAsync();

            return true;
        }*/
    }
}
