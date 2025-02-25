using MVCBanXeDap.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Models;
using MVCBanXeDap.Services.Jwt;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using APIBanXeDap.Models;

namespace MVCBanXeDap.Controllers
{
    public class UpdateNhanVienController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _apiUri = new Uri("https://localhost:7137/api/");
        private readonly IjwtToken ijwtToken;
        public UpdateNhanVienController(IjwtToken ijwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = _apiUri;
            this.ijwtToken = ijwtToken;
        }
        [HttpGet]
        public async Task<IActionResult> UpdateNhanVien()
        {
            var validationToken = await ijwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validationToken))
            {
                var accesstoken = validationToken;
                var information = ijwtToken.GetInformationUserFromToken(accesstoken);
                var id = information.Id;
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"UpdateNhanVien/GetNhanVienById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var nv = JsonConvert.DeserializeObject<ApiReponse<NhanVienVM>>(data);
                    var kh = new NhanVienVM();
                    kh = nv.Data;
                    return View(kh);

                }
            }
            return NotFound("Không tìm thấy nhân viên");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNhanVien(NhanVienVM nv)
        {
            string token = HttpContext.Session.GetString("AccessToken");
            var information = ijwtToken.GetInformationUserFromToken(token);
            var id = information.Id;
            var formData = new MultipartFormDataContent();

            // Thêm các trường dữ liệu vào FormData
            formData.Add(new StringContent(nv.HoTen), "HoTen");
            formData.Add(new StringContent(nv.Cccd), "Cccd");
            formData.Add(new StringContent(nv.Sdt), "Sdt");
            formData.Add(new StringContent(nv.Email), "Email");
            formData.Add(new StringContent(nv.MatKhau), "MatKhau");
            formData.Add(new StringContent(nv.GioiTinh.ToString()), "GioiTinh");
            formData.Add(new StringContent(nv.NgaySinh?.ToString("yyyy-MM-dd")), "NgaySinh");
            formData.Add(new StringContent(nv.DiaChi), "DiaChi");
            formData.Add(new StringContent(nv.Luong.ToString()), "Luong");
            formData.Add(new StringContent(nv.VaiTro), "VaiTro");
            formData.Add(new StringContent(nv.NgayVaoLam?.ToString("yyyy-MM-dd")), "NgayVaoLam");
            formData.Add(new StringContent(nv.TinhTrang), "TinhTrang");
            formData.Add(new StringContent(nv.IsDelete?.ToString() ?? "false"), "IsDelete");

            // Thêm file ảnh vào FormData (nếu có)
            if (nv.Anh != null)
            {
                var fileContent = new StreamContent(nv.Anh.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(nv.Anh.ContentType);
                formData.Add(fileContent, "Anh", nv.Anh.FileName);
            }
            var json = JsonConvert.SerializeObject(formData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Gọi API cập nhật
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"UpdateNhanVien/UpdateNhanVien/{id}", formData);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công";
                return RedirectToAction("Index", "Product"); 
            }
            ModelState.AddModelError(string.Empty, "Cập nhật thất bại");
            return View(nv);
        }
    }
}
