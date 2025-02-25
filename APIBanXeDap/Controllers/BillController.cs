using APIBanXeDap.Models;
using APIBanXeDap.Repository.HoaDon;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin, Nhân viên")]
    public class BillController : ControllerBase
    {
        private readonly IHoaDonRepository hoaDonRepository;
        public BillController(IHoaDonRepository hoaDonRepository)
        {
            this.hoaDonRepository = hoaDonRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get() //Lấy thông tin danh sách hóa đơn
        {
            return Ok(await hoaDonRepository.GetAllHoadonVMAsync());
        }
        [HttpGet("{maHoadon}")]
        public async Task<IActionResult> Get(int maHoadon) //Lấy 1 thông tin hóa đơn
        {
            return Ok(await hoaDonRepository.GetAsync(x => x.MaHoaDon == maHoadon));
        }
        [HttpPut]
        public async Task<IActionResult> ChangeStatusOrder(int idOrder, int idStaff, string statusOrder, string? reason)
        {
            var currentOrderStatus = await hoaDonRepository.GetOrderStatusById(idOrder);

            // Kiểm tra sự tồn tại của đơn hàng
            if (currentOrderStatus == null)
            {
                return NotFound(new { success = false, message = "Đơn hàng không tồn tại." });
            }

            // Kiểm tra tính hợp lệ của trạng thái tiếp theo
            if (!IsValidNextState(currentOrderStatus, statusOrder))
            {
                return BadRequest(new { success = false, message = "Không thể thay đổi trạng thái đơn hàng sang trạng thái không hợp lệ." });
            }

            // Thực hiện thay đổi trạng thái đơn hàng
            var result = await hoaDonRepository.ChangeStatusOrder(idOrder, idStaff, statusOrder, reason);

            if (result == null)
            {
                return Conflict(new { success = false, message = "Đơn hàng đã được quản lý bởi nhân viên khác. Đơn hàng chỉ có thể thay đổi bởi người xác nhận đơn hàng" });
            }

            return Ok(new { success = true, message = "Đã thay đổi tình trạng đơn hàng thành công." });
        }


        [HttpGet("{maHoaDon}")]
        public async Task<IActionResult> GetInvoiceData(int maHoaDon) // Lấy 1 thông tin hóa đơn cùng sản phẩm cho xuất hóa đơn
        {
            return Ok(await hoaDonRepository.GetInvoiceDataAsync(null, maHoaDon:maHoaDon));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInvoiceData() // Lấy nhiều thông tin hóa đơn cùng sản phẩm cho xuất hóa đơn
        {
            return Ok(await hoaDonRepository.GetAllInvoiceDataAsync(null));
        }

        #region//NonAction

        // Phương thức để kiểm tra tính hợp lệ của trạng thái tiếp theo
        private bool IsValidNextState(string currentStatus, string nextStatus)
        {
            var validNextStatesMap = new Dictionary<string, List<string>>
            {
                { "Chờ xác nhận", new List<string> { "Đã xác nhận", "Đã hủy" } },
                { "Đã xác nhận", new List<string> { "Đã giao cho đơn vị vận chuyển", "Đã hủy" } },
                { "Đã giao cho đơn vị vận chuyển", new List<string> { "Đang giao hàng", "Đã hủy" } },
                { "Đang giao hàng", new List<string> {  "Hoàn trả/Hoàn tiền", "Đã hủy" } },
                { "Chờ thanh toán", new List<string> { "Đã xác nhận", "Đã hủy" } },
                { "Đã thanh toán", new List<string> { "Hoàn trả/Hoàn tiền" } },
                { "Hoàn trả/Hoàn tiền", new List<string> { } },
                { "Đã hủy", new List<string> { } }
            };

            // Loại bỏ khả năng quay về trạng thái "Chờ xác nhận" cho trạng thái "Đã thanh toán"
            if (currentStatus == "Đã thanh toán" && nextStatus == "Chờ xác nhận")
            {
                return false;
            }

            return validNextStatesMap.ContainsKey(currentStatus) && validNextStatesMap[currentStatus].Contains(nextStatus);
        }

        #endregion
    }
}
