using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;

namespace MVCBanXeDap.Controllers
{
    public class DashboardController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        public DashboardController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllOrderData()
        {
            // Gọi API để lấy thông tin hóa đơn
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(_client.BaseAddress + $"dashboard/GetAllOrderData");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                return Json(data);
            }
            return Json(new {success = false}); //Thêm trường hợp dữ liệu trống
        }
        //Lấy số liệu doanh thu đơn hàng theo thời gian
        public async Task<IActionResult> GetEarningData(string timeRange = "day")
        {
            // Gọi API để lấy dữ liệu doanh thu
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"dashboard/GetEarningData/{timeRange}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                return Json(JsonConvert.DeserializeObject(data)); // Deserialize JSON response
            }
            return Json(new { success = false });
        }
        //Lấy số liệu đơn hàng theo tình trạng
        public async Task<IActionResult> GetOrderStatusData(string timeRange = "day")
        {
            // Gọi API để lấy dữ liệu theo trạng thái đơn hàng
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"dashboard/GetOrderStatusData/{timeRange}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                return Json(JsonConvert.DeserializeObject(data)); // Deserialize JSON response
            }
            return Json(new { success = false });
        }
        //Lấy danh sách top 5 sản phẩm được mua nhiều nhất
        public async Task<IActionResult> GetTopSellingProducts()
        {
            try
            {
                // Gọi API để lấy sản phẩm bán chạy nhất
                HttpResponseMessage response = await _client.GetAsync("dashboard/GetTopSellingProducts");

                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu JSON từ response API
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON thành object và trả về dưới dạng JSON
                    TopSellingProductsVM result = JsonConvert.DeserializeObject<TopSellingProductsVM>(responseData);


                    return Json(new { success = true, data = result });
                }
                else
                {
                    // Xử lý trường hợp API trả lỗi
                    return Json(new
                    {
                        success = false,
                        message = $"API trả mã lỗi: {response.StatusCode}"
                    });
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi trong quá trình gọi API hoặc xử lý
                return Json(new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi thực hiện yêu cầu.",
                    error = ex.Message
                });
            }
        }
        //Lấy dữ liệu thống kê thông kê đơn hàng theo nhân viên
        public async Task<IActionResult> GetEmployeeOrderStats()
        {
            // Gọi API để lấy dữ liệu thống kê đơn hàng theo nhân viên
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"dashboard/GetEmployeeOrderStats");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                return Json(JsonConvert.DeserializeObject(data)); // Deserialize JSON response
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> GetStatUserAsync()
        {
            // Gọi API để lấy dữ liệu thống kê trạng thái người dùng
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"dashboard/GetStatUserAsync");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                return Json(JsonConvert.DeserializeObject(data)); // Deserialize JSON response
            }
            return Json(new { success = false });
        }
    }
}
