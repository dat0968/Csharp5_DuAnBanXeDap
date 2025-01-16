using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace APIBanXeDap.Repository.TrangChu
{

    public class TrangChuRepository : ITrangChuRepository
    {
        private readonly Csharp5Context db;

        public TrangChuRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public List<ProductVM> GetSanPhamBanChay()
        {
            var list = new List<ProductVM>();
            //var sanPhamBanChayChiTiet = db.Chitiethoadons
            //    .GroupBy(cthd => cthd.MaSp)
            //    .Select(g => new
            //    {
            //        MaSanPham = g.Key,
            //        TongSoLuongBan = g.Sum(cthd => cthd.SoLuong),
            //    })
            //    .OrderByDescending(x => x.TongSoLuongBan)
            //    .Take(10)
            //    .Join(db.Sanphams,
            //          spBanChay => spBanChay.MaSanPham,
            //          sp => sp.MaSp,
            //          (spBanChay, sp) => new { spBanChay, sp })
            //    .Join(db.Chitietsanphams,
            //          spWithBan => spWithBan.sp.MaSp,
            //          ctsp => ctsp.MaSp,
            //          (spWithBan, ctsp) => new
            //          {
            //              MaSP = spWithBan.sp.MaSp,
            //              TenSp = spWithBan.sp.TenSp,
            //              NgaySanXuat = spWithBan.sp.NgaySanXuat,
            //              spWithBan.sp.MoTa,
            //              spWithBan.sp.Hinh,
            //              NhaCungCap = spWithBan.sp.MaNhaCcNavigation.TenNhaCc,
            //              DanhMuc = spWithBan.sp.MaDanhMucNavigation.TenDanhMuc,
            //              ThuongHieu = spWithBan.sp.MaThuongHieuNavigation.TenThuongHieu,
            //              TongSoLuongBan = spWithBan.spBanChay.TongSoLuongBan,
            //              SoLuongTon = ctsp.SoLuongTon,
            //              MaMau = ctsp.MaMau,
            //              MaKichThuoc = ctsp.MaKichThuoc
            //          })
            //    .ToList();

            //foreach (var item in sanPhamBanChayChiTiet)
            //{
            //    // Lấy danh sách chi tiết sản phẩm cho mỗi sản phẩm bán chạy, lọc theo MaMau và MaKichThuoc của từng sản phẩm
            //    var chiTietSanPhamList = db.Mausacs
            //         .Join(db.Chitietsanphams, // Nối bảng Mausacs và Chitietsanphams
            //                ms => ms.MaMau, // Điều kiện nối theo MaMau
            //                ctsp => ctsp.MaMau,
            //                (ms, ctsp) => new { ms, ctsp }) // Chọn kết quả từ Mausacs và Chitietsanphams
            //         .Join(db.Kichthuocs, // Nối tiếp với bảng KichThuocs
            //                combined => combined.ctsp.MaKichThuoc, // Điều kiện nối theo MaKichThuoc
            //                kt => kt.MaKichThuoc,
            //                (combined, kt) => new DetailsProductVM // Tạo đối tượng DetailsProductVM từ kết quả nối
            //                {
            //                    MaMau = combined.ctsp.MaMau,
            //                    TenMau = combined.ms.TenMau,
            //                    MaKichThuoc = combined.ctsp.MaKichThuoc,
            //                    TenKichThuoc = kt.TenKichThuoc, // Lấy tên kích thước từ bảng KichThuocs
            //                    SoLuongTon = combined.ctsp.SoLuongTon
            //                })
            //         // Lọc theo MaMau và MaKichThuoc của từng sản phẩm bán chạy
            //         .Where(ctsp => ctsp.MaMau == item.MaMau && ctsp.MaKichThuoc == item.MaKichThuoc)
            //        .ToList();

            //    // Thêm vào danh sách sản phẩm bán chạy
            //    list.Add(new ProductVM
            //    {
            //        MaSP = item.MaSP,
            //        TenSp = item.TenSp,
            //        ThuongHieu = item.ThuongHieu,
            //        Hinh = item.Hinh,
            //        MoTa = item.MoTa,
            //        NgaySanXuat = item.NgaySanXuat,
            //        NhaCungCap = item.NhaCungCap,
            //        DanhMuc = item.DanhMuc,
            //        SoLuong = item.SoLuongTon,
            //        Chitietsanphams = chiTietSanPhamList // Gắn chi tiết sản phẩm vào sản phẩm bán chạy
            //    });
            //}
            var sanphambanchay = (from cthd in db.Chitiethoadons
                                  join sp in db.Sanphams on cthd.MaSp equals sp.MaSp
                                  join ctsp in db.Chitietsanphams on sp.MaSp equals ctsp.MaSp
                                  group cthd by sp.MaSp into grouped
                                  select new
                                  {
                                      MaSP = grouped.Key,
                                      TenSP = grouped.Select(p => p.MaSpNavigation.TenSp).FirstOrDefault(),
                                      ThuongHieu = grouped.Select(p => p.MaSpNavigation.MaThuongHieuNavigation.TenThuongHieu).FirstOrDefault(),
                                      TongSoLuongMua = grouped.Sum(p => p.SoLuong),
                                      Hinh = grouped.Select(p => p.MaSpNavigation.Hinh).FirstOrDefault(),
                                      MoTa = grouped.Select(p => p.MaSpNavigation.MoTa).FirstOrDefault(),
                                      NgaySanXuat = grouped.Select(p => p.MaSpNavigation.NgaySanXuat).FirstOrDefault(),
                                      NhaCungCap = grouped.Select(p => p.MaSpNavigation.MaNhaCcNavigation.TenNhaCc).FirstOrDefault(),
                                      DanhMuc = grouped.Select(p => p.MaSpNavigation.MaDanhMucNavigation.TenDanhMuc).FirstOrDefault(),
                                      SoLuong = grouped.Select(p => p.MaSpNavigation.Chitietsanphams.Sum(p => p.SoLuongTon)).FirstOrDefault(),
                                  }).OrderByDescending(p => p.TongSoLuongMua).Take(10).ToList();
            foreach(var item in sanphambanchay)
            {
                var chitietsanphams = db.Chitietsanphams.Include(p => p.MaMauNavigation)
                            .Where(ct => ct.MaSp == item.MaSP) 
                            .Select(ct => new DetailsProductVM
                            {
                                MaSP = item.MaSP,
                                TenSP = item.TenSP,
                                SoLuongTon = ct.SoLuongTon,
                                DonGia = ct.DonGia,
                                MaMau = ct.MaMau,
                                TenMau = ct.MaMauNavigation.TenMau,
                                MaKichThuoc = ct.MaKichThuoc,
                                TenKichThuoc = ct.MaKichThuocNavigation.TenKichThuoc,
                            })
                            .ToList();
                list.Add(new ProductVM
                {
                    MaSP = item.MaSP,
                    TenSp = item.TenSP,
                    ThuongHieu = item.ThuongHieu,
                    Hinh = item.Hinh,
                    MoTa = item.MoTa,
                    NgaySanXuat = item.NgaySanXuat,
                    NhaCungCap = item.NhaCungCap,
                    DanhMuc = item.DanhMuc,
                    SoLuong = item.SoLuong,
                    Chitietsanphams = chitietsanphams
                }); 
            }
            return list;
        }

        //public List<Sanpham> GetSanphams()
        //{
        //    return db.Sanphams.ToList();
        //}
    }
}
