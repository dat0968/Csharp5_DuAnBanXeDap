using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using MVCBanXeDap.ViewModels;
using APIBanXeDap.Repository.UpdateProfile;
using MVCBanXeDap.Services.Jwt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MVCBanXeDap.Models;
using System.Net.Http.Headers;

namespace MVCBanXeDap.Controllers
{
    public class UpdateProfileController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _apiUri = new Uri("https://localhost:7137/api/");
        private readonly IjwtToken ijwtToken;
        public UpdateProfileController(IjwtToken ijwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = _apiUri;
            this.ijwtToken = ijwtToken;
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            string token = HttpContext.Session.GetString("AccessToken");
            var information = ijwtToken.GetInformationUserFromToken(token);
            var id = information.Id;
            // Lấy thông tin khách hàng từ API
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"KhachHang/GetKhachHangById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var khachHang = JsonConvert.DeserializeObject<ApiReponse<KhachHangVM>>(data);
                var kh = new KhachHangVM();
                kh = khachHang.Data;
                return View(kh);
                
            }
            return NotFound("Không tìm thấy khách hàng");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile( KhachHangVM khachHang)
        {
            string token = HttpContext.Session.GetString("AccessToken");
            var information = ijwtToken.GetInformationUserFromToken(token);
            var id = information.Id;
            var formData = new MultipartFormDataContent();

            // Thêm các trường dữ liệu vào FormData
            formData.Add(new StringContent(khachHang.HoTen), "HoTen");
            formData.Add(new StringContent(khachHang.Cccd), "Cccd");
            formData.Add(new StringContent(khachHang.Sdt), "Sdt");
            formData.Add(new StringContent(khachHang.Email), "Email");
            formData.Add(new StringContent(khachHang.TenTaiKhoan), "TenTaiKhoan");
            formData.Add(new StringContent(khachHang.MatKhau), "MatKhau");
            formData.Add(new StringContent(khachHang.GioiTinh.ToString()), "GioiTinh");
            formData.Add(new StringContent(khachHang.NgaySinh?.ToString("yyyy-MM-dd")), "NgaySinh");
            formData.Add(new StringContent(khachHang.DiaChi), "DiaChi");

            // Thêm file ảnh vào FormData (nếu có)
            if (khachHang.Anh != null)
            {
                var fileContent = new StreamContent(khachHang.Anh.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(khachHang.Anh.ContentType);
                formData.Add(fileContent, "Anh", khachHang.Anh.FileName);
            }
            var json = JsonConvert.SerializeObject(formData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Gọi API cập nhật
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"UpdateProfile/UpdateProfile/{id}", formData);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chủ
            }
            ModelState.AddModelError(string.Empty, "Cập nhật thất bại");
            return View(khachHang);
        }
    }
}
