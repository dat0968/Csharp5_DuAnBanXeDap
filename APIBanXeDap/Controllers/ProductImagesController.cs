using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.Repository.HinhAnhSanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImagesRepository ProductImages;

        public ProductImagesController(IProductImagesRepository ProductImages)
        {
            this.ProductImages = ProductImages;
        }
        [HttpGet("GetAllProductImages")]
        public IActionResult GetAllProductImages()
        {
            return Ok(ProductImages.GetAllProductImage());
        }
        [HttpPost("CreateProductImage")]
        public IActionResult CreateProductImage(ProductImgEM model)
        {
            try
            {
                
                ProductImages.CreateProductImage(model);
                return Ok(new
                {
                    Success = true,
                    Message = "Thêm hình ảnh sản phẩm thành công"
                });
            }catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}"
                });
            }
        }
        [HttpDelete("DeleteProductImage/{id}")]
        public IActionResult DeleteProductImage( [FromRoute] int id)
        {
            try
            {
                ProductImages.DeleteProductImage(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Xóa hình ảnh sản phẩm thành công"
                });
            }catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}"
                });
            }          
        }
    }
}
