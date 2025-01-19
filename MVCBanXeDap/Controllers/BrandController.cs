using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MVCBanXeDap.Controllers
{
    public class BrandController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        public BrandController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? keywords, string? sort, int page = 1)
        {

            var ListBrand = new List<BrandVM>();
            HttpResponseMessage responseBrand = await _client.GetAsync(_client.BaseAddress + $"Brands/GetAllBrand?keywords={keywords}&sort={sort}&page={page}");            
            if (responseBrand.IsSuccessStatusCode)
            {
                string data = await responseBrand.Content.ReadAsStringAsync();
                ListBrand = JsonConvert.DeserializeObject<List<BrandVM>>(data);
                ViewBag.Brand = ListBrand;
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể tải danh sách thương hiệu.";
            }
            return View(ListBrand);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var brand = new BrandVM();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"Brands/GettBrandById/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                brand = JsonConvert.DeserializeObject<BrandVM>(data);
            };
            return PartialView("_BrandDetails", brand);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBrand(BrandVM newBrand)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(newBrand);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_client.BaseAddress + "Brands/CreateBrand", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Thêm thương hiệu thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể thêm thương hiệu mới.";
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
                var jsonContent = JsonConvert.SerializeObject(updatedBrand);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(_client.BaseAddress + "Brands/EditBrand", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật thương hiệu thành công!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Không thể cập nhật thương hiệu.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrand(int id)
        {
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
            return RedirectToAction("index", "Brand");
        }
    }
}
