using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository ProductRepository;

        public ProductsController(IProductRepository ProductRepository)
        {
            this.ProductRepository = ProductRepository;
        }
        [HttpGet("GetAllProduct")]
        public IActionResult GetAllProduct()
        {
            var list = ProductRepository.GetAllProduct();
            return Ok(list);
        }
        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById([FromRoute]int id)
        {
            var detail = ProductRepository.GetProductById(id);
            return Ok(detail);
        }
        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct(ProductEM product)
        {
            try
            {
                var CreateProduct = ProductRepository.CreateProduct(product);
                return Ok(new
                {
                    Success = true,
                    Messagge = "Thêm sản phẩm mới thành công",
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
        [HttpPut("EditProduct")]
        public IActionResult EditProduct(ProductEM product)
        {
            try
            {
                ProductRepository.UpdateProduct(product);
                return Ok(new
                {
                    Success = true,
                    Messagge = "Sửa thông tin sản phẩm thành công",
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}"
                });
            }
        }
        [HttpPut("DeleteProduct/{id}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            try
            {
                ProductRepository.DeleteProduct(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Xóa thông tin sản phẩm thành công",
                });
            }
            catch(Exception ex)
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
