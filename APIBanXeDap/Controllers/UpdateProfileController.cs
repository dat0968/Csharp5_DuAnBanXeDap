using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.Repository.UpdateProfile;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateProfileController : ControllerBase
    {
        private readonly IUpdateProfileRepository ProductRepository;
        public UpdateProfileController(IUpdateProfileRepository productRepository)
        {
            this.ProductRepository = productRepository;
        }

        [HttpPut("UpdateProfile/{id}")]
        public IActionResult Update(int id, [FromForm] KhachHangVM khachHangVM)
        {
            ProductRepository.UpdateProflie(id, khachHangVM);
            return Ok();
        }
    }
}
