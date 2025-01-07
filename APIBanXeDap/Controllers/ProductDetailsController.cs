using APIBanXeDap.EditModels;
using APIBanXeDap.Models;
using APIBanXeDap.Repository.ChiTietSanPham;
using APIBanXeDap.Repository.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailsRepository ProductDetailsRepository;

        public ProductDetailsController(IProductDetailsRepository ProductDetailsRepository)
        {
            this.ProductDetailsRepository = ProductDetailsRepository;
        }
        [HttpGet("GetAllProductDetails")]
        public IActionResult GetAllProductDetails()
        {
            var ListProductDetails = ProductDetailsRepository.GetAllDetailsProduct();
            return Ok(ListProductDetails);
        }
        [HttpPost("CreateAllProductDetails")]
        public IActionResult CreateAllProductDetails(DetailsProductEM model)
        {
            try
            {
                var CreateDetailProduct = ProductDetailsRepository.CreateDetailsProduct(model);
                return Ok(new
                {
                    Success = true,
                    Message = "Thêm chi tiết sản phẩm mới thành công"
                });
            }catch (Exception ex)
            {
                return Ok( new
                {
                    Success = false,
                    Message = $"Error {ex.Message}"
                });
            }            
        }
        [HttpPut("EditDetailsProduct")]
        public IActionResult EditDetailsProduct(DetailsProductEM model)
        {
            try
            {
                ProductDetailsRepository.EditDetailsProduct(model);
                return Ok(new
                {
                    Success = true,
                    Message = "Sửa thông tin chi tiết sản phẩm thành công"
                });
            }catch(Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}"
                });
            }
        }
        [HttpDelete("DeleteDetailsProduct")]
        public async Task<IActionResult> DeleteDetailsProduct( [FromQuery] int MaSP, int MaMau, int MaKichThuoc)
        {
            try
            {
                ProductDetailsRepository.DeleteDetailsProduct(MaSP, MaMau, MaKichThuoc);
                return Ok(new
                {
                    Success = true,
                    Message = "Xóa chi tiết sản phẩm thành công"
                });
            }catch(Exception ex)
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
