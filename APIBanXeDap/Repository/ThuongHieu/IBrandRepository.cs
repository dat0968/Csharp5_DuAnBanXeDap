using APIBanXeDap.EditModels;
using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.ThuongHieu
{
    public interface IBrandRepository
    {
        public List<Thuonghieu> GetAllBrand(string? keywords, string? sort);
        public BrandEM GetBrandById(int id);
        public BrandEM CreateBrand(BrandEM brand);
        public void UpdateBrand(BrandEM brand);
        public void DeleteBrand(int id);
    }
}
