using APIBanXeDap.Repository.NhaCungCap;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository SupplierRepository;

        public SuppliersController(ISupplierRepository SupplierRepository)
        {
            this.SupplierRepository = SupplierRepository;
        }
        [HttpGet]
        public IActionResult GetAllSupplier()
        {
            var ListSuppliers = SupplierRepository.getAllSupplier();
            return Ok(ListSuppliers);
        }
    }
}
