using APIBanXeDap.Models.ViewModels;
using APIBanXeDap.Repository.UpdateNhanVien;
using APIBanXeDap.Repository.UpdateProfile;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateNhanVienController : ControllerBase
    {
        private readonly IUpdateNhanVienRepository updateNhanVienRepository;
        public UpdateNhanVienController(IUpdateNhanVienRepository updateNhanVienRepository)
        {
            this.updateNhanVienRepository = updateNhanVienRepository;
        }
        [HttpPut("UpdateNhanVien/{id}")]
        public IActionResult UpdateNhanVien([FromRoute]int id, [FromForm] NhanVienVM nv)
        {
            updateNhanVienRepository.UpdateProflie(id, nv);
            return Ok();    
        }
        [HttpGet("GetNhanVienById/{id}")]
        public IActionResult GetKhachHangById(int id)
        {
            try
            {
                var khachHang = updateNhanVienRepository.GetNhanVienById(id);
                if (khachHang == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy nhân viên" });
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
