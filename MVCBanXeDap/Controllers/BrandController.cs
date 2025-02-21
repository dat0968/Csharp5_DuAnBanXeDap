using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.ViewModels;
using MVCBanXeDap.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using MVCBanXeDap.Services.Jwt;
using System.Net.Http.Headers;
using Azure;

namespace MVCBanXeDap.Controllers
{
    public class BrandController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;
        public BrandController(IjwtToken jwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            this.jwtToken = jwtToken;
        }
        [NonAction]
        [HttpGet]
        public async Task<string?> SetAuthorizationHeader()
        {
            var validateAccessToken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                var accesstoken = validateAccessToken;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                return accesstoken;
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? keywords, string? sort, int page = 1)
        {
            var ListBrand = new List<BrandVM>();
            try
            {
                var accesstoken = await SetAuthorizationHeader();
                HttpResponseMessage responseBrand = await _client.GetAsync(_client.BaseAddress + $"Brands/GetAllBrand?keywords={keywords}&sort={sort}&page={page}");
                if (responseBrand.IsSuccessStatusCode)
                {
                    string data = await responseBrand.Content.ReadAsStringAsync();
                    var ConvertResponseSupplier = JsonConvert.DeserializeObject<JObject>(data);
                    ListBrand = ConvertResponseSupplier["data"].ToObject<List<BrandVM>>();
                    ViewBag.TotalPages = ConvertResponseSupplier["totalPages"].Value<int>();
                    ViewBag.Page = ConvertResponseSupplier["page"].Value<int>();
                    ViewBag.Keywords = keywords;
                    ViewBag.Sort = sort;
                    ViewBag.Token = accesstoken;
                }
                else
                {
                    return StatusCode((int)responseBrand.StatusCode);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi gọi API.";
            }
            return View(ListBrand);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            SetAuthorizationHeader();
            var BrandVM = new BrandVM();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"Brands/GettBrandById/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                BrandVM = JsonConvert.DeserializeObject<BrandVM>(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
            return PartialView("_BrandDetails", BrandVM);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBrand(BrandVM newBrand)
        {
            try
            {
                SetAuthorizationHeader();
                var jsonContent = JsonConvert.SerializeObject(newBrand);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_client.BaseAddress + "Brands/CreateBrand", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }

                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditBrand(BrandVM updatedBrand)
        {
            if (updatedBrand.MaThuongHieu <= 0 || string.IsNullOrWhiteSpace(updatedBrand.TenThuongHieu))
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View();
            }

            try
            {
                SetAuthorizationHeader();
                var jsonContent = JsonConvert.SerializeObject(updatedBrand);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(_client.BaseAddress + "Brands/EditBrand", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật thương hiệu thành công!";;
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            SetAuthorizationHeader(); 
            StringContent content = new StringContent($"{{\"id\": {id}}}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"Brands/DeleteBrand/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                var dataResponse = await response.Content.ReadAsStringAsync();
                var ConvertResponse = JsonConvert.DeserializeObject<JObject>(dataResponse);
                var isSuccess = ConvertResponse["success"].Value<bool>();
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = ConvertResponse["message"].Value<string>();
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện thao tác";
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
            return RedirectToAction("index", "Brand");
        }
    }
}
