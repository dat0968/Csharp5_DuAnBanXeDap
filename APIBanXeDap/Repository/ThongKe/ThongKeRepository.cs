using APIBanXeDap.Models;
using APIBanXeDap.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Linq;
using System.Linq.Expressions;

namespace APIBanXeDap.Repository.ThongKe
{
    public class ThongKeRepository : IThongKeRepository
    {
        private readonly Csharp5Context _db;
        private IQueryable<Nhanvien> dbSet;

        public ThongKeRepository(Csharp5Context db)
        {
            _db = db;
            this.dbSet = _db.Set<Nhanvien>();
        }
        public async Task<IEnumerable<NhanVienVM>> GetEmployeeOrderStatsAsync(Expression<Func<Nhanvien, bool>>? filter = null)
        {
            IQueryable<Nhanvien> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Lấy dữ liệu từ cơ sở dữ liệu
            var nhanviens = await query.ToListAsync();

            // Chuyển đổi danh sách nhân viên thành danh sách NhanVienVM
            var nhanVienVMs = nhanviens.Select(nv => new NhanVienVM
            {
                MaNv = nv.MaNv,
                HoTen = nv.HoTen,
                GioiTinh = nv.GioiTinh,
                NgaySinh = nv.NgaySinh,
                DiaChi = nv.DiaChi,
                Sdt = nv.Sdt,
                Email = nv.Email,
                NgayVaoLam = nv.NgayVaoLam,
                Luong = nv.Luong,
                VaiTro = nv.VaiTro,
                TinhTrang = nv.TinhTrang,

            }).ToList();

            return nhanVienVMs;
        }

        public async Task<IEnumerable<(int, int)>> GetStatUser()
        {
            List<(int, int)> values = new List<(int, int)>();

            var nhanViens = await _db.Nhanviens.Select(x => x.IsDelete).ToListAsync();
            var khachHangs = await _db.Khachhangs.Select(x => x.IsDelete).ToListAsync();


            //Console.WriteLine(String.Join(", ",nhanViens));
            //Console.WriteLine(String.Join(", ", khachHangs));


            int inactiveNV = nhanViens.Count(nv => nv.HasValue && nv.Value);
            int activeNV = nhanViens.Count() - inactiveNV;
            values.Add((activeNV, inactiveNV));

            int inactiveKH = khachHangs.Count(kh => kh.HasValue && kh.Value);
            int activeKh = khachHangs.Count() - inactiveKH;
            values.Add((activeKh, inactiveKH));
            
            return values;
        }
    }
}
