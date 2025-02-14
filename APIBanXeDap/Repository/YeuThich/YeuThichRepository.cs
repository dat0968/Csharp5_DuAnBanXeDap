using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace APIBanXeDap.Repository.YeuThich
{
    public class YeuThichRepository : IYeuThichRepository
    {
        private readonly Csharp5Context _db;
        private IQueryable<Yeuthich> dbSet;

        public YeuThichRepository(Csharp5Context db)
        {
            _db = db;
            this.dbSet = _db.Set<Yeuthich>();
        }
        public async Task<WishlistVM> GetAsync(Expression<Func<Yeuthich, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<Yeuthich> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            var entity = await query.FirstOrDefaultAsync(filter);

            if (entity == null) return null;

            return new WishlistVM
            {
                Ma = entity.Ma,
                MaDoiTuong = entity.MaDoiTuong,
                MaNguoiDung = entity.MaNguoiDung,
                DoiTuongYeuThich = entity.DoiTuongYeuThich,
                //NoiDungBinhLuan = entity.DoiTuongYeuThich == "BinhLuan" ? entity.NoiDungBinhLuan : null,
                TenSp = entity.DoiTuongYeuThich == "SanPham" ? entity.Sanpham?.TenSp : null,
                ThuongHieu = entity.DoiTuongYeuThich == "SanPham" ? entity.Sanpham?.MaThuongHieuNavigation.TenThuongHieu : null,
                Hinh = entity.DoiTuongYeuThich == "SanPham" ? entity.Sanpham?.Hinh : null,
                MoTa = entity.Sanpham?.MoTa ?? null,
                //NgayBinhLuan = entity.NgayBinhLuan,
                NhaCungCap = entity.DoiTuongYeuThich == "SanPham" ? entity.Sanpham?.MaNhaCcNavigation.TenNhaCc : null,
                DanhMuc = entity.DoiTuongYeuThich == "SanPham" ? entity.Sanpham?.MaDanhMucNavigation.TenDanhMuc : null,
                //SoLuong = entity.DoiTuongYeuThich == "SanPham" ? entity.Sanpham?.SoLuong ?? 0 : 0,
                //KhoangGia = entity.DoiTuongYeuThich == "SanPham" ? entity.Sanpham?.KhoangGia : null
            };
        }

        public async Task<IEnumerable<WishlistVM>> GetAllYeuThichVMAsync(Expression<Func<Yeuthich, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Yeuthich> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            // Lấy dữ liệu từ cơ sở dữ liệu
            var yeuthichList = await query.ToListAsync();

            // Chuyển đổi danh sách yeuthich sang WishlistVM
            return yeuthichList.Select(entity => new WishlistVM
            {
                Ma = entity.Ma,
                MaDoiTuong = entity.MaDoiTuong,
                MaNguoiDung = entity.MaNguoiDung,
                DoiTuongYeuThich = entity.DoiTuongYeuThich,
                TenSp = entity.DoiTuongYeuThich == "SanPham" && entity.Sanpham != null ? entity.Sanpham.TenSp : null,
                ThuongHieu = entity.DoiTuongYeuThich == "SanPham" && entity.Sanpham != null ? entity.Sanpham.MaThuongHieuNavigation?.TenThuongHieu : null,
                Hinh = entity.DoiTuongYeuThich == "SanPham" && entity.Sanpham != null ? entity.Sanpham.Hinh : null,
                MoTa = entity.Sanpham?.MoTa ?? null,
                NhaCungCap = entity.DoiTuongYeuThich == "SanPham" && entity.Sanpham != null ? entity.Sanpham.MaNhaCcNavigation?.TenNhaCc : null,
                DanhMuc = entity.DoiTuongYeuThich == "SanPham" && entity.Sanpham != null ? entity.Sanpham.MaDanhMucNavigation?.TenDanhMuc : null,
            }).ToList();
        }

        public async Task<IActionResult> ChangeWishlist(int idProduct, string typeObject, int idUser)
        {
            var wishlist = await dbSet.FirstOrDefaultAsync(x => x.MaDoiTuong == idProduct && x.DoiTuongYeuThich == typeObject && x.MaNguoiDung == idUser);

            if (wishlist == null) // Nếu sản phẩm chưa có trong danh sách yêu thích
            {
                var newWishlist = new Yeuthich
                {
                    MaDoiTuong = idProduct,
                    DoiTuongYeuThich = typeObject,
                    MaNguoiDung = idUser
                };

                await _db.YeuThichs.AddAsync(newWishlist);
                await _db.SaveChangesAsync();

                return new JsonResult(new
                {
                    success = true,
                    message = "Sản phẩm đã được thêm vào danh sách yêu thích."
                });
            }

            _db.YeuThichs.Remove(wishlist);
            await _db.SaveChangesAsync();

            return new JsonResult(new
            {
                success = true,
                message = "Đã xóa sản phẩm khỏi danh sách yêu thích."
            });
        }

        public async Task<bool> IsOneInWishlist(int idProduct, int idUser)
        {
            return await dbSet.AnyAsync(x => x.MaDoiTuong == idProduct && x.MaNguoiDung == idUser);
        }

        public async Task<IEnumerable<bool>> IsManyInWishlist(int[] idProducts, int idUser)
        {
            var results = await dbSet.Where(x => idProducts.Contains(x.MaDoiTuong) && x.MaNguoiDung == idUser)
                                      .Select(x => x.MaDoiTuong)
                                      .ToListAsync();

            return idProducts.Select(id => results.Contains(id)).ToArray();
        }
    }
}
