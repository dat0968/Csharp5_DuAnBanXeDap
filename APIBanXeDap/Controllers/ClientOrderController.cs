using APIBanXeDap.Repository.HoaDon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClientOrderController : ControllerBase
    {

        private readonly IHoaDonRepository hoaDonRepository;
        public ClientOrderController(IHoaDonRepository hoaDonRepository)
        {
            this.hoaDonRepository = hoaDonRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get() //Lấy thông tin danh sách hóa đơn
        {
            int? maKhachHang = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await hoaDonRepository.GetAllHoadonVMAsync(x => x.MaKh == maKhachHang));
        }
        [HttpGet("{maHoadon}")]
        public async Task<IActionResult> Get(int maHoadon) //Lấy 1 thông tin hóa đơn
        {
            return Ok(await hoaDonRepository.GetAsync(x => x.MaHoaDon == maHoadon));
        }
        [HttpGet("{maHoaDon}")]
        public async Task<IActionResult> GetInvoiceData(int maHoaDon) // Lấy 1 thông tin hóa đơn cùng sản phẩm cho xuất hóa đơn
        {
            return Ok(await hoaDonRepository.GetInvoiceDataAsync(null, maHoaDon: maHoaDon));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInvoiceData() // Lấy nhiều thông tin hóa đơn cùng sản phẩm cho xuất hóa đơn
        {
            int? maKhachHang = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await hoaDonRepository.GetAllInvoiceDataAsync(maKhachHang));
        }
        [HttpGet]
        public async Task<IActionResult> CountOrder()
        {
            int? maKhachHang = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await hoaDonRepository.CountOrder(maKhachHang));
        }
    }
}
