﻿using APIBanXeDap.Repository.MaCoupon;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("GetAllCouponCode")]
        public IActionResult GetAll(string? keywords, bool? status, string? sort, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 5;
            try
            {
                var listCounponCode = MaCouponRepository.GetAll(keywords, status, sort);
                var pagedCouponCode = listCounponCode.Skip((page - 1) * pagesize).Take(pagesize);
                var totalItems = listCounponCode.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                return Ok(new
                {
                    Success = true,
                    Data = pagedCouponCode,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Page = page,
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
        [HttpDelete("Delete")]
        public IActionResult Delete(string id)
        {
            try
            {
                MaCouponRepository.Delete(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Xóa thông tin mã coupon thành công"
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
