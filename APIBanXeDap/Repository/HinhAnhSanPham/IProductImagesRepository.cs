using APIBanXeDap.EditModels;
using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.HinhAnhSanPham
{
    public interface IProductImagesRepository
    {
        public List<Hinhanh> GetAllProductImage();
        public ProductImgEM CreateProductImage(ProductImgEM productImage);
        public void DeleteProductImage(int productImageID);
    }
}
