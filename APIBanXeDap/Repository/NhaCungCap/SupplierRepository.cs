using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBanXeDap.Repository.NhaCungCap
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly Csharp5Context db;

        public SupplierRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public List<Nhacungcap> GetAllSupplier(string? keywords, string? sort)
        {
            var list = db.Nhacungcaps.AsNoTracking().Where(ncc => ncc.IsDelete == false).AsQueryable();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(keywords))
            {
                list = list.Where(n => n.MaNhaCc.ToString().Contains(keywords) || n.TenNhaCc.Contains(keywords));
            }

            // Sắp xếp theo yêu cầu
            switch (sort)
            {
                case "asc":
                    // Sắp xếp theo tên nhà cung cấp từ A-Z
                    list = list.OrderBy(n => n.TenNhaCc);
                    break;
                case "desc":
                    // Sắp xếp theo tên nhà cung cấp từ Z-A
                    list = list.OrderByDescending(n => n.TenNhaCc);
                    break;
                default:
                    // Sắp xếp mặc định (có thể thay đổi theo nhu cầu)
                    list = list.OrderByDescending(n => n.TenNhaCc);
                    break;
            }

            return list.ToList();
        }
        public SupplierEM CreateSupplier(SupplierEM supplier)
        {
            var NewSupplier = new Nhacungcap
            {
                TenNhaCc = supplier.TenNhaCc,
                DiaChi = supplier.DiaChi,
                Email = supplier.Email,
                Sdt = supplier.Sdt,
                IsDelete = false,
            };
            db.Nhacungcaps.Add(NewSupplier);
            db.SaveChanges();
            return supplier;
        }
        public void DeleteSupplier(int id)
        {
            var findSupplier = db.Nhacungcaps.FirstOrDefault(p => p.MaNhaCc == id);
            findSupplier.IsDelete = true;
            db.Nhacungcaps.Update(findSupplier);
            db.SaveChanges();
        }
        public void UpdateSupplier(SupplierEM supplier)
        {
            var findSupplier = db.Nhacungcaps.SingleOrDefault(sp => sp.MaNhaCc == supplier.MaNhaCc);
            if (findSupplier != null)
            {
                findSupplier.TenNhaCc = supplier.TenNhaCc;
                findSupplier.DiaChi = supplier.DiaChi;
                findSupplier.Email = supplier.Email;
                findSupplier.Sdt = supplier.Sdt;
                db.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy nhà cung cấp với ID = {supplier.MaNhaCc}");
            }
        }

        public SupplierEM GetSupplierById(int id)
        {
            var brand = db.Nhacungcaps.AsNoTracking()
                .FirstOrDefault(sp => sp.MaNhaCc == id);

            if (brand == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy nhà cung cấp với ID = {id}");
            }
            var brandEM = new SupplierEM
            {
                MaNhaCc = brand.MaNhaCc,
                TenNhaCc = brand.TenNhaCc,
                DiaChi = brand.DiaChi,
                Email = brand.Email,
                Sdt = brand.Sdt,
            };
            return brandEM;
        }
    }
}
