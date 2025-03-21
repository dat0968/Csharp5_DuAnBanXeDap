﻿using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MVCBanXeDap.Controllers
{
    public class DashboardController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken _jwt;
        public DashboardController(IjwtToken jwt)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _jwt = jwt;
        }
        public async Task<IActionResult> Index()
        {
            var validateAccessToken = await _jwt.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                ViewBag.ValueAuth = validateAccessToken;
            }
            SetAuthorizationHeader();
            var response = await _client.GetAsync(_client.BaseAddress + "dashboard/IsAuth");
            if (!response.IsSuccessStatusCode)
            {
                int Status = (int)response.StatusCode;
                return RedirectToAction("Error","Home",Status);
            }
            return View();
        }
        #region [GET APIS]
        public async Task<IActionResult> GetAllOrderData()
        {
            SetAuthorizationHeader();
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
            SetAuthorizationHeader();
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
            SetAuthorizationHeader();
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
            SetAuthorizationHeader();
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
            SetAuthorizationHeader();
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
            SetAuthorizationHeader();
            // Gọi API để lấy dữ liệu thống kê trạng thái người dùng
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"dashboard/GetStatUserAsync");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                return Json(JsonConvert.DeserializeObject(data)); // Deserialize JSON response
            }
            return Json(new { success = false });
        }
        #endregion

        #region [NON ACTION]
        [NonAction]
        [HttpGet]
        public async void SetAuthorizationHeader()
        {
            var validateAccessToken = await _jwt.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                var accesstoken = validateAccessToken;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            }
        }
        #endregion
    }
}
