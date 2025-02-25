using APIBanXeDap.Models;
using APIBanXeDap.Models.ViewModels;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.UpdateNhanVien
{
    public class UpdateNhanVienRepository : IUpdateNhanVienRepository
    {
        private readonly Csharp5Context db;

        public UpdateNhanVienRepository(Csharp5Context db)
        {
            this.db = db;
        }

        public Nhanvien GetNhanVienById(int id)
        {
            
            var nhanvien = db.Nhanviens.FirstOrDefault(kh => kh.MaNv == id && kh.IsDelete == false);

            if (nhanvien != null)
            {
                nhanvien.HoTen = nhanvien.HoTen?.Trim();
                nhanvien.DiaChi = nhanvien.DiaChi?.Trim();
                nhanvien.Sdt = nhanvien.Sdt?.Trim();
                nhanvien.Cccd = nhanvien.Cccd?.Trim();
                nhanvien.TenTaiKhoan = nhanvien.TenTaiKhoan?.Trim();
                nhanvien.MatKhau = nhanvien.MatKhau?.Trim();
                nhanvien.Email = nhanvien.Email?.Trim();
   
            }

            return nhanvien;
        
        }

        public void UpdateProflie(int id, NhanVienVM profile)
        {
            var nhanvien = db.Nhanviens.Find(id);
            if (nhanvien == null)
            {
                return; // Không tìm thấy khách hàng
            }

            nhanvien.HoTen = profile.HoTen;
            nhanvien.Cccd = profile.Cccd;
            nhanvien.Sdt = profile.Sdt;
            nhanvien.Email = profile.Email;  
            nhanvien.MatKhau = profile.MatKhau;
            nhanvien.GioiTinh = profile.GioiTinh;
            nhanvien.NgaySinh = profile.NgaySinh;
            nhanvien.DiaChi = profile.DiaChi;

            if (profile.Anh != null)
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Hinh/AnhNhanVien");
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

                nhanvien.Hinh = fileName; // Chỉ lưu tên file vào database
            }

            db.Nhanviens.Update(nhanvien);
            db.SaveChanges();
        }
    }
}
