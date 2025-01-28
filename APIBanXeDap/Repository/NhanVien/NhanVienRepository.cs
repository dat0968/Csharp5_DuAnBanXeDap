using APIBanXeDap.Models;
using APIBanXeDap.Models.ViewModels;
using APIBanXeDap.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace APIBanXeDap.Repository
{

    public class NhanVienRepository : INhanVienRepository
    {
        private readonly Csharp5Context _context;

        public NhanVienRepository(Csharp5Context context)
        {
            _context = context;
        }

        public List<Nhanvien> GetAll(string? keyword, string? sort, string? status, string? gender)
        {
            var query = _context.Nhanviens.Where(nv => nv.IsDelete == false);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(nv => nv.HoTen.Contains(keyword) || nv.TenTaiKhoan.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(nv => nv.TinhTrang == status);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(nv => nv.GioiTinh == gender);
            }

            if (sort == "asc")
            {
                query = query.OrderBy(nv => nv.HoTen);
            }
            else if (sort == "desc")
            {
                query = query.OrderByDescending(nv => nv.HoTen);
            }

            return query.ToList();
        }



        public Nhanvien Add(Nhanvien nhanVien)
        {
            _context.Nhanviens.Add(nhanVien);
            _context.SaveChanges();
            return nhanVien;
        }



        public Nhanvien Update(int id, NhanVienVM nhanVienVM)
        {
            var nhanVien = _context.Nhanviens.FirstOrDefault(nv => nv.MaNv == id);
            if (nhanVien != null)
            {
                nhanVien.HoTen = nhanVienVM.HoTen;
                nhanVien.GioiTinh = nhanVienVM.GioiTinh;
                nhanVien.NgaySinh = nhanVienVM.NgaySinh;
                nhanVien.DiaChi = nhanVienVM.DiaChi;
                nhanVien.Cccd = nhanVienVM.Cccd;
                nhanVien.Sdt = nhanVienVM.Sdt;
                nhanVien.Email = nhanVienVM.Email;
                nhanVien.TenTaiKhoan = nhanVienVM.TenTaiKhoan;
                nhanVien.MatKhau = nhanVienVM.MatKhau;
                nhanVien.TinhTrang = nhanVienVM.TinhTrang;
                nhanVien.VaiTro = nhanVienVM.VaiTro;
                nhanVien.Luong = nhanVienVM.Luong;
                nhanVien.NgayVaoLam = nhanVienVM.NgayVaoLam;

                if (nhanVienVM.Anh != null)
                {
                    nhanVien.Hinh = SaveImage(nhanVienVM.Anh);
                }

                _context.SaveChanges();
            }
            return nhanVien;
        }

        public Nhanvien GetNhanVienById(int id)
        {
            return _context.Nhanviens.FirstOrDefault(nv => nv.MaNv == id && nv.IsDelete == false);
        }

        public void ToggleStatus(int id, string status)
        {
            var nhanVien = _context.Nhanviens.FirstOrDefault(nv => nv.MaNv == id);
            if (nhanVien == null)
            {
                throw new Exception("Không tìm thấy nhân viên.");
            }

            nhanVien.TinhTrang = status;
            _context.SaveChanges();
        }

        public void ToggleIsDelete(int id)
        {
            var nhanVien = _context.Nhanviens.FirstOrDefault(nv => nv.MaNv == id);
            if (nhanVien == null)
            {
                throw new Exception("Không tìm thấy nhân viên.");
            }

            nhanVien.IsDelete = true;
            nhanVien.Email = null;
            nhanVien.TenTaiKhoan = null;
            nhanVien.MatKhau = null;

            _context.SaveChanges();
        }


        public List<Nhanvien> GetPaged(int pageNumber, int pageSize, string? keyword, string? sort, string? status, string? gender)
        {
            var query = _context.Nhanviens.Where(nv => nv.IsDelete == false);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(nv => nv.HoTen.Contains(keyword) || nv.TenTaiKhoan.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(nv => nv.TinhTrang == status);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(nv => nv.GioiTinh == gender);
            }

            if (sort == "asc")
            {
                query = query.OrderBy(nv => nv.HoTen);
            }
            else if (sort == "desc")
            {
                query = query.OrderByDescending(nv => nv.HoTen);
            }

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetTotalCount(string? keyword)
        {
            var query = _context.Nhanviens.Where(nv => nv.IsDelete == false);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(nv => nv.HoTen.Contains(keyword) || nv.TenTaiKhoan.Contains(keyword));
            }

            return query.Count();
        }

        private string SaveImage(IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/AnhNhanVien");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var filePath = Path.Combine(directoryPath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return $"/Hinh/AnhNhanVien/{file.FileName}";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi lưu ảnh: {ex.Message}");
                }
            }
            return null;
        }

        public bool IsCccdExists(string cccd)
        {
            return _context.Nhanviens.Any(x => x.Cccd == cccd);
        }

        public bool IsSdtExists(string sdt)
        {
            return _context.Nhanviens.Any(x => x.Sdt == sdt);
        }

        public bool IsEmailExists(string email)
        {
            return _context.Nhanviens.Any(x => x.Email == email);
        }

        public bool IsTenTaiKhoanExists(string tenTaiKhoan)
        {
            return _context.Nhanviens.Any(x => x.TenTaiKhoan == tenTaiKhoan);
        }
    }
}
