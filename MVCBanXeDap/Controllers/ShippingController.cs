using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MVCBanXeDap.Controllers
{
    public class ShippingController : Controller
    {
        Uri uri = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;
        public ShippingController(IjwtToken jwtToken)
        {
            this.jwtToken = jwtToken;
            _client = new HttpClient();
            _client.BaseAddress = uri;
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
        public async Task<IActionResult> Index(string? keywords, string? priceFilter, string? SortByPrice, int page = 1)
        {
            var accesstoken = await SetAuthorizationHeader();
            var list = new List<ShippingVM>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"Shippings/GetAll?page={page}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ConvertResponse = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = ConvertResponse["success"].Value<bool>();
                if (isSuccess)
                {
                    list = ConvertResponse["data"].ToObject<List<ShippingVM>>();
                    ViewBag.TotalPages = ConvertResponse["totalPages"].Value<int>();
                    ViewBag.Page = ConvertResponse["page"].Value<int>();
                    ViewBag.Keywords = keywords;
                    ViewBag.PriceFilter = priceFilter;
                    ViewBag.SortByPrice = SortByPrice;
                    ViewBag.Token = accesstoken;
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create(string tinh, string quan, string phuong, float gia)
        {
            SetAuthorizationHeader();
            var model = new ShippingVM
            {
                ThanhPho = tinh,
                QuanHuyen = quan,
                Phuong = phuong,
                Gia = gia
            };
            var ConvertModelToJson = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(ConvertModelToJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "Shippings/CreateShipping", content);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ConvertResponse = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = ConvertResponse["success"].Value <bool>();
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = ConvertResponse["message"].Value<string>();
                }
                else
                {
                    TempData["ErrorMessage"] = ConvertResponse["message"].Value<string>();
                }
;           }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            SetAuthorizationHeader();
            HttpResponseMessage response = await _client.DeleteAsync(_client.BaseAddress + $"Shippings/DeleteShipping/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var ConvertResponse = JsonConvert.DeserializeObject<JObject>(data);
                var isSuccess = ConvertResponse["success"].Value<bool>();
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = ConvertResponse["message"].Value<string>();
                }
                else
                {
                    TempData["ErrorMessage"] = ConvertResponse["message"].Value<string>();
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult GetPartialViewEdit([FromBody] ShippingVM model)
        {
            return PartialView("~/Views/Shared/_ShippingtEdit.cshtml", model);
        }
    }
}
