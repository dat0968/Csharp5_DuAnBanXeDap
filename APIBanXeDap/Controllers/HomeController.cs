using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.Repository.TrangChu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIBanXeDap.ViewModels;
using System.ComponentModel.DataAnnotations;
using APIBanXeDap.Models;
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
        //[HttpGet("SanPhams")]
        //public async Task<IActionResult> GetSanPham()
        //{
        //    var sanPhamList = trangChuRepository.GetSanphams();
        //    return Ok(sanPhamList);
        //}
        [HttpGet("SanPhamBanChay")]
        public async Task<IActionResult> GetSanPhamBanChay()
        {
            var spbc = trangChuRepository.GetSanPhamBanChay();
            return Ok(spbc);
        }
        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var detail = trangChuRepository.GetProductById(id);
            return Ok(detail);
        }
        [HttpGet("GetSanPhamLienQuan/{dm}")]
        public IActionResult GetSanPhamLienQuan([FromRoute] string dm)
        {
            var detail = trangChuRepository.GetSanphamLienQuan(dm);
            return Ok(detail);
        }
        [HttpGet("GetAllProduct")]
        public IActionResult GetAllProduct(string? keywords, int? MaDanhMuc, int? MaThuongHieu, string? sort, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 8;
            var list = trangChuRepository.GetAllProduct(keywords, MaDanhMuc, MaThuongHieu, sort);
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
        [HttpGet("GetAllCategory")]
        public IActionResult GetAllCategory()
        {
            var ListCategories = trangChuRepository.GetAllCategory();
            return Ok(ListCategories);
        }
        [HttpGet("GetAllBrand")]
        public IActionResult GetAllBrand(string? keywords, string? sort, int page)
        {
            if (page >= 1)
            {
                int pagesize = 10;
                var ListBrands = trangChuRepository.GetAllBrand(keywords, sort);
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
                var ListBrands = trangChuRepository.GetAllBrand(keywords, sort);
                return Ok(ListBrands);
            }
        }
    }
}
