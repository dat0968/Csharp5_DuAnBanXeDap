using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.Repository.TrangChu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIBanXeDap.ViewModels;
using System.ComponentModel.DataAnnotations;
using APIBanXeDap.Models;
using System.Data.Common;
using System.Security.Claims;
using APIBanXeDap.Repository.DanhMuc;
using APIBanXeDap.Repository.ThuongHieu;
namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        
        private readonly ITrangChuRepository trangChuRepository;
        public HomeController( ITrangChuRepository trangChuRepository, IProductRepository productRepository)
        {
            this.trangChuRepository = trangChuRepository;
            
        }
        [NonAction]
        [HttpGet("CheckRole")]
        public IActionResult CheckRole()
        {
            var role = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role);
            if(role?.Value == "Admin" || role?.Value == "Nhân viên")
            {
                return StatusCode(401);
            }
            return StatusCode(200);
        }
        [HttpGet("SanPhamBanChay")]
        public async Task<IActionResult> GetSanPhamBanChay()
        {
            var status = CheckRole();
            if(status is StatusCodeResult statuscode && statuscode.StatusCode == 401)
            {
                return StatusCode(401);
            }
            var spbc = trangChuRepository.GetSanPhamBanChay();
            return Ok(spbc);
        }
        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var status = CheckRole();
            if (status is StatusCodeResult statuscode && statuscode.StatusCode == 401)
            {
                return StatusCode(401);
            }
            var detail = trangChuRepository.GetProductById(id);
            return Ok(detail);
        }
        [HttpGet("GetSanPhamLienQuan/{dm}")]
        public IActionResult GetSanPhamLienQuan([FromRoute] string dm)
        {
            var status = CheckRole();
            if (status is StatusCodeResult statuscode && statuscode.StatusCode == 401)
            {
                return StatusCode(401);
            }
            var detail = trangChuRepository.GetSanphamLienQuan(dm);
            return Ok(detail);
        }
        [HttpGet("GetAllProduct")]
        public IActionResult GetAllProduct(string? keywords, int? MaDanhMuc, int? MaThuongHieu, string? sort, double? giaMin, double? giaMax, int page = 1)
        {
            var status = CheckRole();
            if (status is StatusCodeResult statuscode && statuscode.StatusCode == 401)
            {
                return StatusCode(401);
            }
            page = page < 1 ? 1 : page;
            int pagesize = 8;
            var list = trangChuRepository.GetAllProduct(keywords, MaDanhMuc, MaThuongHieu, sort, giaMin, giaMax);
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
        
    }
}
