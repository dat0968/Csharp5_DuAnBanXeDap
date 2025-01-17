using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MVCBanXeDap.Controllers
{
    public class ShippingController : Controller
    {
        Uri uri = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        public ShippingController()
        {
            _client = new HttpClient();
            _client.BaseAddress = uri;
        }
        public async Task<IActionResult> Index(string? keywords, string? priceFilter, string? SortByPrice, int page = 1)
        {
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
                }
            }
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create(string tinh, string quan, string phuong, float gia)
        {
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
;            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
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
            return RedirectToAction("index");
        }
    }
}
