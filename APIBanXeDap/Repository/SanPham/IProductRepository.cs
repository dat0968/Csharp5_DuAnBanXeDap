using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.SanPham
{
    public interface IProductRepository
    {
        public List<ProductVM> GetAllProduct();
        public ProductVM GetProductById(int id);
        public ProductEM CreateProduct(ProductEM product);
        public void UpdateProduct(ProductEM product);
        public void DeleteProduct(int id);

    }
}
