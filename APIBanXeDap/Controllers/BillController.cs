using APIBanXeDap.Models;
using APIBanXeDap.Repository.HoaDon;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IHoaDonRepository hoaDonRepository;
        public BillController(IHoaDonRepository hoaDonRepository)
        {
            this.hoaDonRepository = hoaDonRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await hoaDonRepository.GetAllAsync());
        }
        [HttpGet("{maHoadon}")]
        public async Task<IActionResult> Get(int maHoadon)
        {
            return Ok(await hoaDonRepository.GetAsync(x => x.MaHoaDon == maHoadon));
        }
        [HttpPut]
        public async Task<IActionResult> ChangeStatusOrder(int idOrder, int idStaff, string statusOrder, string? idStaffChanged)
        {
            // Lấy tình trạng hiện tại của đơn hàng từ cơ sở dữ liệu
            var currentOrderStatus = await hoaDonRepository.GetOrderStatusById(idOrder);

            if (currentOrderStatus == null)
            {
                return NotFound(new { success = false, message = "Đơn hàng không tồn tại." });
            }

            // Định nghĩa các tình trạng hợp lệ tiếp theo
            var validNextStatesMap = new Dictionary<string, List<string>>
    {
        { "Chờ xác nhận", new List<string> { "Đã xác nhận", "Đã hủy" } },
        { "Đã xác nhận", new List<string> { "Đã giao cho đơn vị vận chuyển", "Đã hủy" } },
        { "Đã giao cho đơn vị vận chuyển", new List<string> { "Đang giao hàng", "Đã hủy" } },
        { "Đang giao hàng", new List<string> { "Đã giao cho khách", "Hoàn trả/Hoàn tiền", "Đã hủy" } },
        { "Chờ thanh toán", new List<string> { "Đã xác nhận", "Đã hủy" } },
        { "Hoàn trả/Hoàn tiền", new List<string> { } },
        { "Đã hủy", new List<string> { } }
    };

            // Kiểm tra tính hợp lệ của trạng thái tiếp theo
            if (!validNextStatesMap.ContainsKey(currentOrderStatus) ||
                !validNextStatesMap[currentOrderStatus].Contains(statusOrder))
            {
                return BadRequest(new { success = false, message = "Không thể thay đổi trạng thái đơn hàng sang trạng thái không hợp lệ." });
            }

            // Kiểm tra xem nhân viên thực hiện thay đổi trạng thái có trùng với nhân viên đã thay đổi không
            if (!string.IsNullOrEmpty(idStaffChanged) && idStaff.ToString() != idStaffChanged)
            {
                return Conflict(new { success = false, message = $"Đơn hàng đã được quản lý bởi nhân viên khác mang mã {idStaffChanged}." });
            }

            // Tiến hành thay đổi trạng thái đơn hàng
            await hoaDonRepository.ChangStatusOrder(idOrder, idStaff, statusOrder);

            return Ok(new { success = true, message = "Đã thay đổi tình trạng đơn hàng thành công." });
        }


        [HttpGet("{maHoaDon}")]
        public async Task<IActionResult> GetInvoiceData(int maHoaDon)
        {
            return Ok(await hoaDonRepository.GetInvoiceDataAsync(maHoaDon));
        }
    }
}
