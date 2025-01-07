using APIBanXeDap.Repository.MauSac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IMauSacRepository MauSacRepository;

        public ColorsController(IMauSacRepository MauSacRepository)
        {
            this.MauSacRepository = MauSacRepository;
        }
        [HttpGet]
        public IActionResult GetAllColor()
        {
            var ListColor = MauSacRepository.GetAll();
            return Ok(ListColor);
        }
    }
}
