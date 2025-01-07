using APIBanXeDap.Repository.KichThuoc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly IKichThuocRepository KichThuocRepository;

        public SizesController(IKichThuocRepository KichThuocRepository)
        {
            this.KichThuocRepository = KichThuocRepository;
        }
        [HttpGet]
        public IActionResult GetAllSize()
        {
            var ListSizes = KichThuocRepository.GetAll();
            return Ok(ListSizes);
        }
    }
}
