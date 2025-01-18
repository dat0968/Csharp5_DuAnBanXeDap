using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
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
            if (_db.Hoadons.Count() < 100)
            {
                CreateOrderForCharts();
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

        public void CreateOrderForCharts()
        {
            try
            {
                // Lấy danh sách khách hàng và nhân viên
                var maKhachHangs = _db.Khachhangs.Select(x => x.MaKh).ToList();
                var maNhanViens = _db.Nhanviens.Select(x => x.MaNv).ToList();

                // Lấy danh sách sản phẩm
                var sanPhams = _db.Chitietsanphams.ToList();

                // Random generator
                Random rand = new Random();

                // Danh sách lưu hóa đơn và chi tiết hóa đơn để thêm vào DbContext
                var hoaDons = new List<Hoadon>();
                var chiTietHoaDons = new List<Chitiethoadon>();

                for (int i = 0; i < 500; i++) // Tạo 500 hóa đơn
                {
                    // Lấy ngẫu nhiên khách hàng và nhân viên
                    int maKhachHang = maKhachHangs[rand.Next(maKhachHangs.Count)];
                    int? maNhanVien = maNhanViens.Count > 0 ? maNhanViens[rand.Next(maNhanViens.Count)] : (int?)null;

                    // Tạo hóa đơn
                    var hoaDon = new Hoadon
                    {
                        MaKh = maKhachHang,
                        MaNv = maNhanVien,
                        DiaChiNhanHang = $"Địa chỉ {i + 1}",
                        NgayTao = DateOnly.FromDateTime(DateTime.Now.AddDays(-rand.Next(1, 365))),
                        ThoiGianGiao = DateOnly.FromDateTime(DateTime.Now.AddDays(rand.Next(7, 365 * 5))),
                        Httt = rand.Next(0, 2) == 0 ? "COD" : "VNPAY", // Hình thức thanh toán
                        TinhTrang = "Chờ xác nhận",
                        Hoten = $"Khách hàng {i + 1}",
                        Sdt = "0123456789",
                        MoTa = "Tạo tự động hóa đơn cho mục đích test"
                    };

                    // Thêm hóa đơn vào danh sách
                    hoaDons.Add(hoaDon);


                }

                // Thêm toàn bộ danh sách hóa đơn và chi tiết hóa đơn vào DbContext
                _db.Hoadons.AddRange(hoaDons);

                // Lưu tất cả thay đổi cùng một lúc (1)
                _db.SaveChanges();

                foreach (var hoaDon in hoaDons)
                {
                    // Tạo chi tiết hóa đơn cho hóa đơn này
                    int soLuongSanPham = rand.Next(1, 5); // Mỗi hóa đơn có 1-5 sản phẩm
                    for (int j = 0; j < soLuongSanPham; j++)
                    {
                        var sanPham = sanPhams[rand.Next(sanPhams.Count)];

                        // Kiểm tra số lượng tồn kho của sản phẩm
                        if (sanPham.SoLuongTon > 0)
                        {
                            int soLuongMua = rand.Next(1, Math.Min(5, sanPham.SoLuongTon + 1)); // Đặt giới hạn mua
                            var chiTiet = new Chitiethoadon
                            {
                                MaSp = sanPham.MaSp,
                                MaHoaDon = hoaDon.MaHoaDon, // Gán MaHoaDon sau khi lưu hoaDon
                                MaMau = sanPham.MaMau,
                                MaKichThuoc = sanPham.MaKichThuoc,
                                SoLuong = soLuongMua,
                                Gia = (decimal)sanPham.DonGia,
                                ThanhTien = soLuongMua * (decimal)sanPham.DonGia
                            };

                            // Giảm số lượng tồn kho và thêm chi tiết hóa đơn vào danh sách
                            sanPham.SoLuongTon -= soLuongMua;
                            chiTietHoaDons.Add(chiTiet);
                        }
                    }
                }
                _db.Chitiethoadons.AddRange(chiTietHoaDons);

                // Lưu tất cả thay đổi cùng một lúc (2)
                _db.SaveChanges();

                Console.WriteLine("Tạo hóa đơn và chi tiết hóa đơn thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi khi tạo hóa đơn: {ex.Message}");
            }
        }


    }
}
