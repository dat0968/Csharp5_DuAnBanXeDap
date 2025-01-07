using APIBanXeDap.Repository.ThuongHieu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository BrandRepository;

        public BrandsController(IBrandRepository BrandRepository)
        {
            this.BrandRepository = BrandRepository;
        }
        [HttpGet]
        public IActionResult GettAllBrand()
        {
            var ListBrands = BrandRepository.getAllBrand();
            return Ok(ListBrands);
        }
    }
}
