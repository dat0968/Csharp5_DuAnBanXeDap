using APIBanXeDap.EditModels;
using APIBanXeDap.Repository.ThuongHieu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Razor;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Authorization;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository BrandRepository;

        public BrandsController(IBrandRepository BrandRepository)
        {
            this.BrandRepository = BrandRepository;
        }
        [HttpGet("GetAllBrand")]
        public IActionResult GetAllBrand(string? keywords, string? sort, int page)
        {
            if(page >= 1)
            {
                int pagesize = 10;
                var ListBrands = BrandRepository.GetAllBrand(keywords, sort);
                var pagedBrand = ListBrands.Skip((page - 1) * pagesize).Take(pagesize).ToList();    
                var totalItems = ListBrands.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                return Ok(new
                {
                    Data = pagedBrand,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Page = page,
                });
            }
            else
            {
                var ListBrands = BrandRepository.GetAllBrand(keywords, sort);
                return Ok(ListBrands);
            }
        }
        [HttpGet("GetBrandById/{id}")]
        public IActionResult GetBrandById([FromRoute]int id)
        {
            try
            {
                var detail = BrandRepository.GetBrandById(id);
                if (detail == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Không tìm thấy thương hiệu với ID = {id}"
                    });
                }
                return Ok(detail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}"
                });
            }
        }
        [HttpPost("CreateBrand")]
        public IActionResult CreateBrand(BrandEM brand)
        {
            try
            {
                if (brand == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Dữ liệu thương hiệu không hợp lệ"
                    });
                }

                var createBrand = BrandRepository.CreateBrand(brand);
                return Ok(new
                {
                    Success = true,
                    Message = "Thêm thương hiệu mới thành công",
                    Data = createBrand
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}"
                });
            }
        }
        [HttpPut("EditBrand")]
        public IActionResult EditBrand(BrandEM brand)
        {
            try
            {
                if (brand == null || brand.MaThuongHieu <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Dữ liệu thương hiệu không hợp lệ"
                    });
                }

                BrandRepository.UpdateBrand(brand);
                return Ok(new
                {
                    Success = true,
                    Message = "Sửa thông tin thương hiệu thành công"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}"
                });
            }
        }
        [HttpPut("DeleteBrand/{id}")]
        public IActionResult DeleteBrand([FromRoute] int id)
        {
            try
            {
                BrandRepository.DeleteBrand(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Xóa thông tin thương hiệu này thành công",
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
    }
}
