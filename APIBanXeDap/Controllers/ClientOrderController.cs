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
        [HttpPatch]
        public async Task<IActionResult> ChangeOrderStatus(int idOrder, string statusOrder, string? reason)
        {
            //Giá trị mặc định
            bool Success = false;
            string Message = "Hiện không thể xử lí yêu cầu, vui lòng thử lại.";
            try
            {

                int? idCustomer = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (!idCustomer.HasValue)
                {
                    Message = "Bạn chưa đăng nhập tài khoản để thay đổi tình trạng, vui lòng thử lại";
                    return NotFound(new { success = Success, message = Message });
                }

                var currentOrderStatus = await hoaDonRepository.GetAsync(x => x.MaHoaDon == idOrder);

                // Kiểm tra sự tồn tại của đơn hàng
                if (currentOrderStatus == null)
                {
                    return NotFound(new { success = false, message = "Đơn hàng không tồn tại." });
                }

                // Kiểm tra quyền khách hàng
                if (currentOrderStatus.MaKh != idCustomer)
                {
                    Message = "Đơn hàng bạn thay đổi tình trạng không thuộc về tài khoản bạn đang đăng nhập trong phiên.";
                    return Conflict(new { success = Success, message = Message });
                }

                // Thực hiện thay đổi trạng thái đơn hàng
                var result = await hoaDonRepository.ChangeStatusOrder(idOrder, null, statusOrder, reason, idCustomer);

                if (result == null)
                {
                    Message = "Bạn không có quyền hạn để thay đổi tình trạng đơn hàng lúc này.";
                    return Conflict(new { success = Success, message =  Message});
                }
                Success = true;
                Message = $"Bạn đã thay đổi trạng thái đơn hàng sang trạng thái [{result}] thành công.";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                throw;
            }

            return Ok(new { success = Success, message = Message });
        }
    }
}
