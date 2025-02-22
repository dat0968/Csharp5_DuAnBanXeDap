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
                                  }).OrderByDescending(p => p.TongSoLuongMua).Take(4).ToList();
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

        public List<ProductVM> GetSanphamLienQuan(string tenDM)
        {
            var Product = db.Sanphams.AsNoTracking()
                 .Include(sp => sp.Chitietsanphams)
                     .ThenInclude(ct => ct.MaMauNavigation)
                 .Include(sp => sp.Chitietsanphams)
                     .ThenInclude(ct => ct.MaKichThuocNavigation)
                 .Include(sp => sp.MaThuongHieuNavigation)
                 .Include(sp => sp.MaNhaCcNavigation)
                 .Include(sp => sp.MaDanhMucNavigation)
                 .Take(4)
                 .Where(sp => sp.MaDanhMucNavigation.TenDanhMuc == tenDM).ToList();
            var productVMList = Product.Select(sp => new ProductVM
            {
                MaSP = sp.MaSp,
                TenSp = sp.TenSp,
                ThuongHieu = sp.MaThuongHieuNavigation.TenThuongHieu,
                Hinh = sp.Hinh,
                MoTa = sp.MoTa,
                NgaySanXuat = sp.NgaySanXuat,
                NhaCungCap = sp.MaNhaCcNavigation.TenNhaCc,
                DanhMuc = sp.MaDanhMucNavigation.TenDanhMuc,
                Chitietsanphams = sp.Chitietsanphams.Select(ct => new DetailsProductVM
                {
                    TenMau = ct.MaMauNavigation.TenMau,
                    TenKichThuoc = ct.MaKichThuocNavigation.TenKichThuoc,
                    SoLuongTon = ct.SoLuongTon,
                    DonGia = ct.DonGia,
                }).ToList(),
            }).ToList();

            return productVMList;
        }
        public List<ProductVM> GetAllProduct(string? keywords, int? MaDanhMuc, int? MaThuongHieu, string? sort)
        {
            var list = db.Sanphams.AsNoTracking().Where(p => p.IsDelete == false)
                .Include(sp => sp.MaThuongHieuNavigation)
                .Include(sp => sp.MaNhaCcNavigation)
                .Include(sp => sp.MaDanhMucNavigation)
                .Include(sp => sp.Chitietsanphams)
                .ThenInclude(ct => ct.MaMauNavigation)
                .Include(sp => sp.Chitietsanphams)
                .ThenInclude(ct => ct.MaKichThuocNavigation)
                .Include(sp => sp.Hinhanhs)
                .AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                list = list.Where(p => p.MaSp.ToString().Contains(keywords) || p.TenSp.Contains(keywords));
            }
            if (MaDanhMuc.HasValue)
            {
                list = list.Where(p => p.MaDanhMuc == MaDanhMuc);
            }
            if (MaThuongHieu.HasValue)
            {
                list = list.Where(p => p.MaThuongHieu == MaThuongHieu);
            }
            switch (sort)
            {
                case "asc":
                    list = list.OrderBy(p => p.Chitietsanphams.Min(p => p.DonGia));
                    break;
                case "desc":
                    list = list.OrderByDescending(p => p.Chitietsanphams.Min(p => p.DonGia));
                    break;
                default:
                    list = list.OrderByDescending(p => p.TenSp);
                    break;
            }
            var ConvertToProductVM = new List<ProductVM>();
            foreach (var item in list)
            {
                ConvertToProductVM.Add(new ProductVM
                {
                    MaSP = item.MaSp,
                    TenSp = item.TenSp,
                    ThuongHieu = item.MaThuongHieuNavigation.TenThuongHieu,
                    Hinh = item.Hinh,
                    MoTa = item.MoTa,
                    NgaySanXuat = item.NgaySanXuat,
                    NhaCungCap = item.MaNhaCcNavigation.TenNhaCc,
                    DanhMuc = item.MaDanhMucNavigation.TenDanhMuc,
                    SoLuong = item.Chitietsanphams?.Sum(p => p?.SoLuongTon ?? 0) ?? 0,
                    KhoangGia = item.Chitietsanphams != null && item.Chitietsanphams.Any()
                    ? $"Từ {item.Chitietsanphams.Where(p => p?.DonGia != null).Min(p => p.DonGia)} VNĐ đến {item.Chitietsanphams.Where(p => p?.DonGia != null).Max(p => p.DonGia)} VNĐ"
                    : "Không có thông tin giá",
                    Chitietsanphams = item.Chitietsanphams != null
                    ? item.Chitietsanphams.Select(ct => new DetailsProductVM
                    {
                        MaSP = ct.MaSp,
                        MaMau = ct.MaMau,
                        TenSP = ct.MaSpNavigation.TenSp,
                        TenMau = ct.MaMauNavigation?.TenMau,
                        MaKichThuoc = ct.MaKichThuoc,
                        TenKichThuoc = ct.MaKichThuocNavigation?.TenKichThuoc,
                        SoLuongTon = ct.SoLuongTon,
                        DonGia = ct.DonGia,
                    }).ToList()
                    : new List<DetailsProductVM>(),
                    Hinhanhs = item.Hinhanhs?.Select(img => new Hinhanh
                    {
                        MaHinhAnh = img.MaHinhAnh,
                        HinhAnh1 = img.HinhAnh1,
                    }).ToList() ?? new List<Hinhanh>()
                });
            }
            return ConvertToProductVM;
        }
        public ProductVM GetProductById(int id)
        {
            var Product = db.Sanphams.AsNoTracking()
                .Include(sp => sp.Chitietsanphams)
                    .ThenInclude(ct => ct.MaMauNavigation)
                .Include(sp => sp.Chitietsanphams)
                    .ThenInclude(ct => ct.MaKichThuocNavigation)
                .Include(sp => sp.MaThuongHieuNavigation)
                .Include(sp => sp.MaNhaCcNavigation)
                .Include(sp => sp.MaDanhMucNavigation)
                .FirstOrDefault(sp => sp.MaSp == id);
            var ProductVM = new ProductVM
            {
                MaSP = Product.MaSp,
                TenSp = Product.TenSp,
                ThuongHieu = Product.MaThuongHieuNavigation.TenThuongHieu,
                Hinh = Product.Hinh,
                MoTa = Product.MoTa,
                NgaySanXuat = Product.NgaySanXuat,
                NhaCungCap = Product.MaNhaCcNavigation.TenNhaCc,
                DanhMuc = Product.MaDanhMucNavigation.TenDanhMuc,
                Chitietsanphams = Product.Chitietsanphams.Select(ct => new DetailsProductVM
                {
                    MaMau = ct.MaMau,
                    TenMau = ct.MaMauNavigation.TenMau,
                    MaKichThuoc = ct.MaKichThuoc,
                    TenKichThuoc = ct.MaKichThuocNavigation.TenKichThuoc,
                    SoLuongTon = ct.SoLuongTon,
                    DonGia = ct.DonGia,
                }).ToList(),
            };
            return ProductVM;
        }
        public List<Thuonghieu> GetAllBrand(string? keywords, string? sort)
        {
            var list = db.Thuonghieus.AsNoTracking().Where(p => p.IsDelete == false).AsQueryable();
            if (!string.IsNullOrEmpty(keywords))
            {
                list = list.Where(p => p.MaThuongHieu.ToString().Contains(keywords) || p.TenThuongHieu.Contains(keywords));
            }
            switch (sort)
            {
                case "asc":
                    // Sắp xếp theo giá tên thương hiệu A-Z
                    list = list.OrderBy(p => p.TenThuongHieu);
                    break;
                case "desc":
                    // Sắp xếp theo giá tên thương hiệu Z-A
                    list = list.OrderByDescending(p => p.TenThuongHieu);
                    break;
                default:
                    list = list.OrderByDescending(p => p.TenThuongHieu);
                    break;
            }
            var lists = list.ToList();
            return lists;
        }
        public List<Danhmuc> GetAllCategory()
        {
            return db.Danhmucs.ToList();
        }
    }
}
