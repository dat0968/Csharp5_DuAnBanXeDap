using APIBanXeDap.Models;
using APIBanXeDap.EditModels;
using APIBanXeDap.ViewModels;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Repository.ThuongHieu
{
    public class BrandRepository : IBrandRepository
    {
        private readonly Csharp5Context db;

        public BrandRepository(Csharp5Context db)
        {
            this.db = db;
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
        public BrandEM CreateBrand(BrandEM brand)
        {
            var NewBrand = new Thuonghieu
            {
                TenThuongHieu = brand.TenThuongHieu,
                IsDelete = false,
            };
            db.Thuonghieus.Add(NewBrand);
            db.SaveChanges();
            return brand;
        }
        public void DeleteBrand(int id)
        {
            var findBrand = db.Thuonghieus.FirstOrDefault(p => p.MaThuongHieu == id);
            findBrand.IsDelete = true;
            db.Thuonghieus.Update(findBrand);
            db.SaveChanges();
        }
        public void UpdateBrand(BrandEM brand)
        {
            var findProduct = db.Thuonghieus.SingleOrDefault(sp => sp.MaThuongHieu == brand.MaThuongHieu);
            if (findProduct != null)
            {
                findProduct.TenThuongHieu = brand.TenThuongHieu;
                db.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Không tìm thấy thương hiệu với ID = {brand.MaThuongHieu}");
            }
        }
        public BrandEM GetBrandById(int id)
        {
            var brand = db.Thuonghieus.AsNoTracking()
                .FirstOrDefault(sp => sp.MaThuongHieu == id);
            if(brand == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy thương hiệu với ID = {id}");
            }
            var brandEM = new BrandEM
            {
                MaThuongHieu = brand.MaThuongHieu,
                TenThuongHieu = brand.TenThuongHieu,
            };
            return brandEM;
        }
    }
}

