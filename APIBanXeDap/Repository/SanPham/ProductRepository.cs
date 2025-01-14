using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APIBanXeDap.Repository.SanPham
{
    public class ProductRepository : IProductRepository
    {
        private readonly Csharp5Context db;

        public ProductRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public ProductEM CreateProduct(ProductEM product)
        {
            var NewProduct = new Sanpham
            {
                TenSp = product.TenSp,
                MaThuongHieu = product.MaThuongHieu,
                Hinh = product.Hinh,
                MoTa = product.MoTa,
                NgaySanXuat = product.NgaySanXuat,
                MaNhaCc = product.MaNhaCC,
                MaDanhMuc = product.MaDanhMuc,
                IsDelete = false,
            };
            db.Sanphams.Add(NewProduct);
            db.SaveChanges();
            return product;
        }

        public void DeleteProduct(int id)
        {
            var findProduct = db.Sanphams.FirstOrDefault(p => p.MaSp == id);
            findProduct.IsDelete = true;
            db.Update(findProduct);
            db.SaveChanges();
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
                .Include(sp => sp.MaThuongHieuNavigation)
                .Include(sp => sp.MaNhaCcNavigation)
                .Include(sp => sp.MaDanhMucNavigation)
                .Include(sp => sp.Chitietsanphams)
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
                    TenMau = ct.MaMauNavigation.TenMau,
                    TenKichThuoc = ct.MaKichThuocNavigation.TenKichThuoc,
                    SoLuongTon = ct.SoLuongTon,
                    DonGia = ct.DonGia,
                }).ToList(),
            };
            return ProductVM;
        }

        public void UpdateProduct(ProductEM product)
        {
            var findProduct = db.Sanphams.SingleOrDefault(sp => sp.MaSp == product.MaSP);
            if(string.IsNullOrEmpty(product.Hinh))
            {
                product.Hinh = findProduct.Hinh;
            }
            if (findProduct != null)
            {
                findProduct.MaDanhMuc = product.MaDanhMuc;
                findProduct.TenSp = product.TenSp;
                findProduct.MaThuongHieu = product.MaThuongHieu;
                findProduct.MoTa = product.MoTa;
                findProduct.NgaySanXuat = product.NgaySanXuat;
                findProduct.MaNhaCc = product.MaNhaCC;
                findProduct.Hinh = product.Hinh;
                db.SaveChanges();
            }
        }
    }
}
