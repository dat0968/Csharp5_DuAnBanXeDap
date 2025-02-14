using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace MVCBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnpayPayment : ControllerBase
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public VnpayPayment(IVnpay vnpay, IConfiguration configuration)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _vnpay = vnpay;
            _configuration = configuration;
            _vnpay.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:ReturnUrl"]);
        }
        [HttpPost("CreatePaymentUrl")]
        public ActionResult<string> CreatePaymentUrl([FromBody] ThongTinHoaDonVM thongtinhoadon)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch

                
                var model = JsonConvert.SerializeObject(thongtinhoadon);
                StringContent content = new StringContent(model, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "Checkouts/CheckoutOrders", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var DeserializeObj = JsonConvert.DeserializeObject<JObject>(data);
                    var isSuccess = DeserializeObj["success"].Value<bool>();
                    var message = DeserializeObj["message"].ToString();
                    if (isSuccess)
                    {
                        var request = new PaymentRequest
                        {
                            PaymentId = (long)DeserializeObj["iDorder"],
                            Money = thongtinhoadon.HoaDon.TongTien,
                            Description = "ToTalAmout " + thongtinhoadon.HoaDon.TongTien,
                            IpAddress = ipAddress,
                            BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                            CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
                            Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                            Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
                        };
                        var paymentUrl = _vnpay.GetPaymentUrl(request);

                        return Created(paymentUrl, paymentUrl);                   
                    }
                    return NotFound($"Không tìm thấy thông tin thanh toán. Error {message}");
                }               
                return NotFound($"Không tìm thấy thông tin thanh toán. Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Callback")]
        public ActionResult<string> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);
                    var resultDescription = $"{paymentResult.PaymentResponse.Description}. {paymentResult.TransactionStatus.Description}.";

                    if (paymentResult.IsSuccess)
                    {
                        string[] splitstring = paymentResult.Description.Split(' ');
                        string tonngtien = splitstring.Last();
                        return RedirectToAction("SuccessCheckout", "Cart", new { IDorder = paymentResult.PaymentId, AmoutTotal = tonngtien });
                    }

                    return BadRequest(resultDescription);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}
