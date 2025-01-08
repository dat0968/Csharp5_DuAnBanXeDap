using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Razor;
using System.Drawing.Printing;

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
        public IActionResult GetAllProduct(string? keywords, int? MaDanhMuc, int? MaThuongHieu, string? sort, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var list = ProductRepository.GetAllProduct(keywords, MaDanhMuc, MaThuongHieu, sort);
            //Phân trang
            var pagedProducts = list.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            //Tổng số trang
            var totalItems = list.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
            return Ok(new
            {
                Data = pagedProducts,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Page = page,
            }); 
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
