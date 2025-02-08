using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompareProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public CompareProductsController(IProductRepository ProductRepository)
        {
            _productRepo = ProductRepository;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> AddProductToCopare(int id)
        {
            CompareProductVM? productVM = await _productRepo.GetCompareProductVmByIdAsync(id);
            if (productVM == null)
            {
                return NotFound();
            }
            return Ok(productVM);
        }
    }
}
