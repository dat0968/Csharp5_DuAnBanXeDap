using APIBanXeDap.Helper;
using APIBanXeDap.Models;
using APIBanXeDap.Repository.MaCoupon;
using APIBanXeDap.Repository.ThanhToan;
using APIBanXeDap.ViewModels;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using System.Web;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;
        private readonly ICheckoutRepository CheckoutRepository;
        private readonly IMaCouponRepository MaCouponRepository;
        private readonly Csharp5Context db;
        public CheckoutsController(IVnpay vnpay, IConfiguration configuration, ICheckoutRepository CheckoutRepository, IMaCouponRepository MaCouponRepository, Csharp5Context db)
        {
            _vnpay = vnpay;
            _configuration = configuration;
            _vnpay.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:ReturnUrl"]);
            this.CheckoutRepository = CheckoutRepository;
            this.MaCouponRepository = MaCouponRepository;
            this.db = db;
        }
        [HttpPost]
        public IActionResult CheckoutOrders([FromBody] ThongTinHoaDonVM model)
        {
            int? MaHoaDon = null;
            db.Database.BeginTransaction();
            try
            {
                var hoadon = CheckoutRepository.CreateOrder(model.HoaDon);
                MaHoaDon = hoadon.MaHoaDon;
                foreach (var chitiet in model.ChiTietHoaDons)
                {
                    chitiet.MaHoaDon = (int)MaHoaDon;
                }
                if (!string.IsNullOrEmpty(model.MaCoupon))
                {
                    MaCouponRepository.RevokeCouponCode(model.MaCoupon);
                }
                CheckoutRepository.CreateDetailOrder(model.ChiTietHoaDons);
                db.Database.CommitTransaction();
                return Ok(new
                {
                    Success = true,
                    Message = "Thanh toán thành công"
                });
            }
            catch (Exception ex)
            {
                db.Database.RollbackTransaction();
                return Ok(new
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                });
            }
        }
        //[HttpPost("CreatePaymentUrl")]
        //public ActionResult<string> CreatePaymentUrl([FromBody] ThongTinHoaDonVM model)
        //{

        //    try
        //    {
        //        var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch
        //        var request = new PaymentRequest
        //        {
        //            PaymentId = (long)MaHoaDon, // Mã Xác định giao dịch
        //            Money = (double)model.ChiTietHoaDons.Sum(p => p.ThanhTien),
        //            Description = HttpUtility.UrlEncode(model.HoaDon.MoTa),
        //            IpAddress = ipAddress, // Địa chỉ IP của thiết bị thực hiện giao dịch
        //            BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
        //            CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
        //            Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
        //            Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
        //        };

        //        var paymentUrl = _vnpay.GetPaymentUrl(request);
        //        return Created(paymentUrl, paymentUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet("CreatePaymentUrl")]
        public ActionResult<string> CreatePaymentUrl(double moneyToPay, string description)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch

                var request = new PaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = moneyToPay,
                    Description = description,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                    CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
                    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                    Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
                };

                var paymentUrl = _vnpay.GetPaymentUrl(request);

                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*IPN cho phép backend nhận thông báo từ VNPAY khi trạng thái thanh toán thay đổi đề từ đó xử lí tiếp 
         mà không cần người dùng quay lại trang web*/
        //[HttpGet("IpnAction")]
        //public IActionResult IpnAction()
        //{
        //    if (Request.QueryString.HasValue)
        //    {
        //        try
        //        {
        //            var paymentResult = _vnpay.GetPaymentResult(Request.Query);
        //            if (paymentResult.IsSuccess)
        //            {
        //                // Thực hiện hành động nếu thanh toán thành công tại đây. Ví dụ: Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu.
        //                return Ok();
        //            }

        //            // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.
        //            return BadRequest("Thanh toán thất bại");
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //    }

        //    return NotFound("Không tìm thấy thông tin thanh toán.");
        //}
        //[HttpGet("Callback")]
        //public ActionResult<string> Callback()
        //{
        //    if (Request.QueryString.HasValue)
        //    {
        //        try
        //        {
        //            var paymentResult = _vnpay.GetPaymentResult(Request.Query);
        //            var orderId = paymentResult.PaymentId;
        //            var findOder = db.Hoadons.FirstOrDefault(p => p.MaHoaDon == orderId);
        //            var resultDescription = $"{paymentResult.PaymentResponse.Description}. {paymentResult.TransactionStatus.Description}.";

        //            if (paymentResult.IsSuccess && findOder != null)
        //            {                        
        //                return Ok(resultDescription);
        //            }

        //            return BadRequest(resultDescription);
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //    }

        //    return NotFound("Không tìm thấy thông tin thanh toán.");
        //}
    }
}
