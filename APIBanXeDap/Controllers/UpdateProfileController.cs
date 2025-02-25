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
        private readonly IUpdateProfileRepository updateProfileRepository;
        public UpdateProfileController(IUpdateProfileRepository updateProfileRepository)
        {
            this.updateProfileRepository = updateProfileRepository;
        }

        [HttpPut("UpdateProfile/{id}")]
        public IActionResult Update(int id, [FromForm] KhachHangVM khachHangVM)
        {
            updateProfileRepository.UpdateProflie(id, khachHangVM);
            return Ok();
        }
        [HttpGet("GetKhachHangById/{id}")]
        public IActionResult GetKhachHangById(int id)
        {
            try
            {
                var khachHang = updateProfileRepository.GetKhachHangById(id);
                if (khachHang == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy khách hàng" });
                }
                return Ok(new { success = true, data = khachHang });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
