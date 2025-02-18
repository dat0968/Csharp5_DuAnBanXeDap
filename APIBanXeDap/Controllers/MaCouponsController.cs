using APIBanXeDap.Repository.MaCoupon;
using APIBanXeDap.ViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaCouponsController : ControllerBase
    {
        private readonly IMaCouponRepository MaCouponRepository;

        public MaCouponsController(IMaCouponRepository MaCouponRepository)
        {
            this.MaCouponRepository = MaCouponRepository;
        }
        [Authorize(Roles = "Admin, Nhân viên")]
        [HttpGet("GetAllCouponCodeByPage")]
        public IActionResult GetAllByPage(string? keywords, string? status, string? sort, int page = 1)
        {
            try
            {
                int pagesize = 5;
                var listCounponCode = MaCouponRepository.GetAll(keywords, status, sort);
                var totalItems = listCounponCode.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                var pagedCouponCode = listCounponCode.Skip((page - 1) * pagesize).Take(pagesize);
                return Ok(new
                {
                    Success = true,
                    Data = pagedCouponCode,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Page = page,
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
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var listCounponCode = MaCouponRepository.GetAll(null, null, null);
                return Ok(new
                {
                    Success = true,
                    Data = listCounponCode,
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
        [Authorize(Roles = "Admin, Nhân viên")]
        [HttpPost("Create")]
        public IActionResult Create(MaCouponVM model)
        {
            try
            {
                var newCouponCode = MaCouponRepository.Create(model);
                return Ok(new
                {
                    Success = true,
                    Message = "Thêm mã coupon mới thành công"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"error {ex.Message}"
                });
            }
        }
        [Authorize(Roles = "Admin, Nhân viên")]
        [HttpPut("Update")]
        public IActionResult Update(MaCouponVM model)
        {
            try
            {
                MaCouponRepository.Update(model);
                return Ok(new
                {
                    Success = true,
                    Message = "Sửa thông tin mã coupon thành công"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"error {ex.Message}"
                });
            }
        }
        [Authorize(Roles = "Admin, Nhân viên")]
        [HttpPut("Cancel")]
        public IActionResult Cancel(string id)
        {
            try
            {
                MaCouponRepository.Cancel(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Hủy thông tin mã coupon thành công"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"error {ex.Message}"
                });
            }
        }
        
    }
}
