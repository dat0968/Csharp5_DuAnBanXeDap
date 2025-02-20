using APIBanXeDap.Repository.VanChuyen;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingsController : ControllerBase
    {
        private readonly IShippingRepository ShippingRepository;

        public ShippingsController(IShippingRepository ShippingRepository)
        {
            this.ShippingRepository = ShippingRepository;
        }
        [Authorize(Roles = "Admin, Nhân viên")]
        [HttpGet("GetAll")]
        public IActionResult Get(string? keywords, string? priceFilter, string? SortByPrice, int page = 1)
        {
            int pagesize = 10;
            try
            {
                var list = ShippingRepository.GetAll(keywords, priceFilter, SortByPrice);
                page = page < 1 ? 1 : page;
                var pageShipping = list.Skip((page - 1) * pagesize).Take(pagesize).ToList();
                int totalCount = list.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pagesize);
                return Ok(new
                {
                    Success = true,
                    Data = pageShipping,
                    Page = page,
                    TotalPages = totalPages,
                    TotalItems = totalCount,
                });
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}",
                });
            }          
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateShipping")]
        public IActionResult Create(ShippingVM model)
        {
            try
            {
                var result = ShippingRepository.Create(model);
                if(result != null)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = "Thêm thông tin vận chuyển mới thành công"
                    });
                }
                return Ok(new
                {
                    Success = false,
                    Message = "Thông tin vận chuyển này đã tồn tại. Không thể thêm thông tin trùng khớp!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Có lỗi xảy ra {ex.Message}"
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("EditShipping")]
        public IActionResult Edit([FromBody] ShippingVM model)
        {
            try
            {
                ShippingRepository.Edit(model);
                return Ok(new
                {
                    Success = true,
                    Message = "Sửa thông tin vận chuyển thành công"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Có lỗi xảy ra {ex.Message}"
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteShipping/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                ShippingRepository.Delete(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Xóa thông tin vận chuyển thành công"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Có lỗi xảy ra {ex.Message}"
                });
            }
        }
        [HttpGet("GetShippingFee")]
        public IActionResult? GetShippingFee(string pho, string quan, string phuong)
        {
            try
            {
                var getShippingfee = ShippingRepository.GetShippingFee(pho, quan, phuong);
                return Ok(new
                {
                    Success = true,
                    Data = getShippingfee,
                    Message = "Truy xuất phí vận chuyển thành công"
                });
            }catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Lỗi {ex.Message}"
                });
            }
        }
    }
}
