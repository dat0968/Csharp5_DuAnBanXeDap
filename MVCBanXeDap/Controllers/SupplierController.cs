using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVCBanXeDap.Models;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MVCBanXeDap.Controllers
{
    public class SupplierController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;
        public SupplierController(IjwtToken jwtToken)
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
            var accesstoken = await SetAuthorizationHeader();
            var ListSupplier = new List<SupplierVM>();
            HttpResponseMessage responseSupplier = await _client.GetAsync(_client.BaseAddress + $"Suppliers/GetAllSupplierByPage?keywords={keywords}&sort={sort}&page={page}");
            if (responseSupplier.IsSuccessStatusCode)
            {
                string data = responseSupplier.Content.ReadAsStringAsync().Result;
                var ConvertResponseSupplier = JsonConvert.DeserializeObject<JObject>(data);
                ListSupplier = ConvertResponseSupplier["data"].ToObject<List<SupplierVM>>();
                ViewBag.TotalPages = ConvertResponseSupplier["totalPages"].Value<int>();
                ViewBag.Page = ConvertResponseSupplier["page"].Value<int>();
                ViewBag.Keywords = keywords;
                ViewBag.Sort = sort;
                ViewBag.Token = accesstoken;
                var infomation = jwtToken.GetInformationUserFromToken(accesstoken);
                ViewBag.Role = infomation.VaiTro;
            }
            else
            {
                return StatusCode((int)responseSupplier.StatusCode);
            }
            return View(ListSupplier);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplier(SupplierVM newSupplier)
        {
           
            try
            {
                SetAuthorizationHeader();
                var jsonContent = JsonConvert.SerializeObject(newSupplier);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_client.BaseAddress + "Suppliers/CreateSupplier", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Thêm nhà cung cấp thành công!";
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
        public async Task<IActionResult> EditSupplier(SupplierVM updatedSupplier)
        {
            if (updatedSupplier.MaNhaCc <= 0 || string.IsNullOrWhiteSpace(updatedSupplier.TenNhaCc))
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View();
            }

            try
            {
                SetAuthorizationHeader();
                var jsonContent = JsonConvert.SerializeObject(updatedSupplier);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(_client.BaseAddress + "Suppliers/EditSupplier", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật nhà cung cấp thành công!";                    
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
                return RedirectToAction("Index");
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
            SetAuthorizationHeader();
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
            else
            {
                return StatusCode((int)response.StatusCode);
            }
            return RedirectToAction("index", "Supplier");
        }


        [HttpPost]
        public IActionResult GetPartialViewEdit([FromBody] SupplierVM model)
        {
            return PartialView("~/Views/Shared/_SupplierEdit.cshtml", model);
        }
        [HttpPost]
        public IActionResult GetPartialViewDetails([FromBody] SupplierVM model)
        {

            return PartialView("~/Views/Shared/_SupplierDetails.cshtml", model);
        }
    }
}
