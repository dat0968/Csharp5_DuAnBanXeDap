using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.Repository.TrangChu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIBanXeDap.ViewModels;
using System.ComponentModel.DataAnnotations;
using APIBanXeDap.Models;
namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ITrangChuRepository trangChuRepository;
        public HomeController( ITrangChuRepository trangChuRepository, IProductRepository productRepository)
        {
            this.trangChuRepository = trangChuRepository;
            this.productRepository = productRepository;
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
            var detail = productRepository.GetProductById(id);
            return Ok(detail);
        }
    }
}
