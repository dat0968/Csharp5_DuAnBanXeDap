using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Repository.HinhAnhSanPham
{
    public class ProductImagesRepository : IProductImagesRepository
    {
        private readonly Csharp5Context db;
        public ProductImagesRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public ProductImgEM CreateProductImage(ProductImgEM productImage)
        {
            if(productImage.MaSp == 0)
            {
                productImage.MaSp = db.Sanphams.OrderBy(p => p.MaSp).LastOrDefault().MaSp;
            }
            var NewImg = new Hinhanh
            {
                MaSp = productImage.MaSp,
                HinhAnh1 = productImage.HinhAnh1,
            };
            db.Hinhanhs.Add(NewImg);
            db.SaveChanges();
            return productImage;
        }

        public void DeleteProductImage(int productImageID)
        {
            var FindImg = db.Hinhanhs.FirstOrDefault(p => p.MaHinhAnh == productImageID);
            if(FindImg != null)
            {
                db.Hinhanhs.Remove(FindImg);
                db.SaveChanges();              
            }
        }

        public List<Hinhanh> GetAllProductImage()
        {
            return db.Hinhanhs.ToList();
        }
    }
}
