using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace APIBanXeDap.Repository
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly Csharp5Context _context;

        public KhachHangRepository(Csharp5Context context)
        {
            _context = context;
        }
        public void Add(Khachhang khachHang)
        {
            _context.Khachhangs.Add(khachHang);
            _context.SaveChanges();
        }

        public bool IsCccdExists(string cccd)
        {
            return _context.Khachhangs.Any(x => x.Cccd == cccd);
        }

        public bool IsSdtExists(string sdt)
        {
            return _context.Khachhangs.Any(x => x.Sdt == sdt);
        }

        public bool IsEmailExists(string email)
        {
            return _context.Khachhangs.Any(x => x.Email == email);
        }

        public bool IsTenTaiKhoanExists(string tenTaiKhoan)
        {
            return _context.Khachhangs.Any(x => x.TenTaiKhoan == tenTaiKhoan);
        }

        public List<Khachhang> GetAll(string? keyword, string? sort, string? status, string? gender)
        {
            var query = _context.Khachhangs.Where(k => k.IsDelete == false); // Chỉ lấy khách hàng chưa bị xóa

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    (x.HoTen != null && x.HoTen.Contains(keyword)) ||
                    (x.TenTaiKhoan != null && x.TenTaiKhoan.Contains(keyword))
                );
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.TinhTrang == status);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(x => x.GioiTinh == gender);
            }

            if (sort == "asc")
            {
                query = query.OrderBy(x => x.HoTen);
            }
            else if (sort == "desc")
            {
                query = query.OrderByDescending(x => x.HoTen);
            }

            return query.ToList();
        }

        public int GetTotalCount(string? keyword)
        {
            var query = _context.Khachhangs.Where(k => k.IsDelete == false); // Sửa lỗi IsDelete nullable

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    (x.HoTen != null && x.HoTen.Contains(keyword)) ||
                    (x.TenTaiKhoan != null && x.TenTaiKhoan.Contains(keyword))
                );
            }

            return query.Count();
        }

        public Khachhang Add(KhachHangVM khachHangVM)
        {
            try
            {
                var khachHang = new Khachhang
                {
                    HoTen = khachHangVM.HoTen,
                    GioiTinh = khachHangVM.GioiTinh,
                    NgaySinh = khachHangVM.NgaySinh,
                    DiaChi = khachHangVM.DiaChi,
                    Cccd = khachHangVM.Cccd,
                    Sdt = khachHangVM.Sdt,
                    Email = khachHangVM.Email,
                    TenTaiKhoan = khachHangVM.TenTaiKhoan,
                    MatKhau = khachHangVM.MatKhau,
                    TinhTrang = "Đang hoạt động",
                    IsDelete = false,
                    Hinh = SaveImage(khachHangVM.Anh) 
                };

                _context.Khachhangs.Add(khachHang);
                _context.SaveChanges();

                Console.WriteLine("Khách hàng thêm thành công!");
                return khachHang;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm khách hàng: {ex.Message}");
                throw;
            }
        }



        public Khachhang Update(int id, KhachHangVM khachHangVM)
        {
            var khachHang = _context.Khachhangs.FirstOrDefault(kh => kh.MaKh == id);
            if (khachHang == null)
            {
                throw new Exception("Không tìm thấy khách hàng");
            }

            // Cập nhật các trường
            khachHang.HoTen = khachHangVM.HoTen;
            khachHang.GioiTinh = khachHangVM.GioiTinh;
            khachHang.NgaySinh = khachHangVM.NgaySinh;
            khachHang.DiaChi = khachHangVM.DiaChi;
            khachHang.Cccd = khachHangVM.Cccd;
            khachHang.Sdt = khachHangVM.Sdt;
            khachHang.Email = khachHangVM.Email;
            khachHang.TenTaiKhoan = khachHangVM.TenTaiKhoan;
            khachHang.MatKhau = khachHangVM.MatKhau;
            khachHang.TinhTrang = khachHangVM.TinhTrang;
            khachHang.IsDelete = khachHangVM.IsDelete;

            _context.SaveChanges();
            return khachHang;
        }




        public enum TinhTrangKhachHang
        {
            HoatDong,
            BiKhoa
        }

        // Sử dụng enum trong ToggleStatus
        public void ToggleStatus(int id, string status)
        {
            var khachHang = _context.Khachhangs.FirstOrDefault(x => x.MaKh == id);
            if (khachHang == null)
            {
                throw new Exception("Không tìm thấy khách hàng.");
            }

            khachHang.IsDelete = true;
            _context.SaveChanges();
        }



        private string SaveImage(IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/AnhKhachHang");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var filePath = Path.Combine(directoryPath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    Console.WriteLine($"Ảnh được lưu tại: {filePath}");
                    return $"/Hinh/AnhKhachHang/{file.FileName}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lưu ảnh: {ex.Message}");
                    throw;
                }
            }
            return null;
        }
        public void ToggleIsDelete(int id)
        {
            var khachHang = _context.Khachhangs.FirstOrDefault(x => x.MaKh == id);
            if (khachHang == null)
            {
                throw new Exception("Không tìm thấy khách hàng.");
            }

            // Đổi trạng thái IsDelete
            khachHang.IsDelete = !khachHang.IsDelete;
            _context.SaveChanges();
        }
        public Khachhang GetKhachHangById(int id)
        {
            return _context.Khachhangs.FirstOrDefault(kh => kh.MaKh == id && kh.IsDelete == false);
        }

        public List<Khachhang> GetPaged(int pageNumber, int pageSize, string? keyword, string? sort, string? status, string? gender)
        {
            var query = _context.Khachhangs.Where(k => k.IsDelete == false);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    (x.HoTen != null && x.HoTen.Contains(keyword)) ||
                    (x.TenTaiKhoan != null && x.TenTaiKhoan.Contains(keyword))
                );
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.TinhTrang == status);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(x => x.GioiTinh == gender);
            }

            if (sort == "asc")
            {
                query = query.OrderBy(x => x.HoTen);
            }
            else if (sort == "desc")
            {
                query = query.OrderByDescending(x => x.HoTen);
            }

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetTotalCount(string? keyword, string? status, string? gender)
        {
            var query = _context.Khachhangs.Where(k => k.IsDelete == false);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    (x.HoTen != null && x.HoTen.Contains(keyword)) ||
                    (x.TenTaiKhoan != null && x.TenTaiKhoan.Contains(keyword))
                );
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.TinhTrang == status);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(x => x.GioiTinh == gender);
            }

            return query.Count();
        }



    }
}
