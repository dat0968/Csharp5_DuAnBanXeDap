using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.UpdateProfile
{
    public class UpdateProfileRepository : IUpdateProfileRepository
    {
        private readonly Csharp5Context db;

        public UpdateProfileRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public void UpdateProflie(int id, KhachHangVM profile)
        {
            var khachHang = db.Khachhangs.Find(id);
            if (khachHang == null)
            {
                return; // Không tìm thấy khách hàng
            }

            khachHang.HoTen = profile.HoTen;
            khachHang.Cccd = profile.Cccd;
            khachHang.Sdt = profile.Sdt;
            khachHang.Email = profile.Email;
            khachHang.TenTaiKhoan = profile.TenTaiKhoan;
            khachHang.MatKhau = profile.MatKhau;
            khachHang.GioiTinh = profile.GioiTinh;
            khachHang.NgaySinh = profile.NgaySinh;
            khachHang.DiaChi = profile.DiaChi;
            if (profile.Anh != null)
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/AnhKhachHang");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = $"{Guid.NewGuid()}_{profile.Anh.FileName}";
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    profile.Anh.CopyTo(stream);
                }

                khachHang.Hinh = fileName; // Chỉ lưu tên file vào database
            }

            db.Khachhangs.Update(khachHang);
            db.SaveChanges();
        }
        public Khachhang GetKhachHangById(int id)
        {
            var khachHang = db.Khachhangs.FirstOrDefault(kh => kh.MaKh == id && kh.IsDelete == false);

            if (khachHang != null)
            {
                khachHang.HoTen = khachHang.HoTen?.Trim();
                khachHang.DiaChi = khachHang.DiaChi?.Trim();
                khachHang.Sdt = khachHang.Sdt?.Trim();
                khachHang.Cccd = khachHang.Cccd?.Trim();
                khachHang.TenTaiKhoan = khachHang.TenTaiKhoan?.Trim();
                khachHang.MatKhau = khachHang.MatKhau?.Trim();
            }

            return khachHang;
        }
    }
}
