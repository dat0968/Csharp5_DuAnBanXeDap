using APIBanXeDap.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBanXeDap.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly Csharp5Context _db;
        public DbInitializer(Csharp5Context db)
        {
            _db = db;
        }
        public void Initializer()
        {
            //Mã dùng kiểm tra Migrations chưa có trong CSDL hoặc chưa thấy CSDL

            //if (_db.Database.GetPendingMigrations().Any())
            //{
            //    _db.Database.Migrate();
            //}

            if (!_db.Nhanviens.Any(x => x.Email == "email@gmail.com"))
            {
                CreateTempStaff();
            }
        }
        public void CreateTempStaff()
        {
            Nhanvien nhanvien = new Nhanvien
            {
                HoTen = "Nguyen Van A",
                GioiTinh = "Nam",
                NgaySinh = new DateOnly(1990, 1, 1),
                DiaChi = "123 Duong ABC, Phuong XYZ, Thanh pho HCM",
                Cccd = "123456789012",
                Sdt = "0901234567",
                Email = "email@gmail.com",
                NgayVaoLam = new DateOnly(2021, 1, 1),
                Luong = 10000000,
                VaiTro = "Nhan vien ban hang",
                TenTaiKhoan = "nguyenvana",
                MatKhau = "password123",
                TinhTrang = "Đang hoạt động",
                IsDelete = false
            };
            _db.Add(nhanvien);
            _db.SaveChanges();
        }
    }
}
