using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVCBanXeDap.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MVCBanXeDap.Controllers
{
    public class SupplierController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        public SupplierController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? keywords, string? sort, int page = 1)
        {
            var ListSupplier = new List<SupplierVM>();
            HttpResponseMessage responseSupplier = await _client.GetAsync(_client.BaseAddress + $"Suppliers/GetAllSupplier?keywords={keywords}&sort={sort}&page={page}");
            if (responseSupplier.IsSuccessStatusCode)
            {
                string data = responseSupplier.Content.ReadAsStringAsync().Result;
                var ConvertResponseSupplier = JsonConvert.DeserializeObject<JObject>(data);
                ListSupplier = ConvertResponseSupplier["data"].ToObject<List<SupplierVM>>();
                ViewBag.TotalPages = ConvertResponseSupplier["totalPages"].Value<int>();
                ViewBag.Page = ConvertResponseSupplier["page"].Value<int>();
                ViewBag.Keywords = keywords;
                ViewBag.Sort = sort;
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể tải danh sách thương hiệu.";
            }
            return View(ListSupplier);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier(SupplierVM newSupplier)
        {
           
            try
            {
                var jsonContent = JsonConvert.SerializeObject(newSupplier);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_client.BaseAddress + "Suppliers/CreateSupplier", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Thêm nhà cung cấp thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể thêm nhà cung cấp mới.";
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
        public async Task<IActionResult> EditSupplier(SupplierVM updatedSupplier)
        {
            if (updatedSupplier.MaNhaCc <= 0 || string.IsNullOrWhiteSpace(updatedSupplier.TenNhaCc))
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View();
            }

            try
            {
                var jsonContent = JsonConvert.SerializeObject(updatedSupplier);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(_client.BaseAddress + "Suppliers/EditSupplier", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật nhà cung cấp thành công!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Không thể cập nhật nhà cung cấp.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            StringContent content = new StringContent($"{{\"id\": {id}}}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"Suppliers/DeleteSupplier/{id}", content);
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
            return RedirectToAction("index", "Supplier");
        }
    }
}
