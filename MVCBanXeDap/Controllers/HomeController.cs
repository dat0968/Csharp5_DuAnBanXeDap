using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Models;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace MVCBanXeDap.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _apiUri = new Uri("https://localhost:7137/api/");

        public HomeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = _apiUri;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = new List<ProductVM2>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "Home/SanPhamBanChay");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var convertResponse = JsonConvert.DeserializeObject<List<ProductVM2>>(data);
                foreach(var item in convertResponse)
                {
                    var minPrice = Convert.ToDecimal(item.Chitietsanphams.Min(x => x.DonGia));  
                    var maxPrice = Convert.ToDecimal(item.Chitietsanphams.Max(x => x.DonGia));

                    // C?p nh?t MinPrice và MaxPrice
                    item.MinPrice = minPrice;
                    item.MaxPrice = maxPrice;
                    list.Add(item);
                }
                
            }
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ProductVM = new ProductVM();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"Home/GetProductById/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                ProductVM = JsonConvert.DeserializeObject<ProductVM>(data);
            };
            return View(ProductVM);
        }
        [HttpGet]
        public async Task<IActionResult> Product()
        {
            var ListProducts = new List<ProductVM2>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"Home/GetAllProduct");
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var ConvertResponseProduct = JsonConvert.DeserializeObject<JObject>(data);
                ListProducts = ConvertResponseProduct["data"].ToObject<List<ProductVM2>>();
                foreach (var product in ListProducts)
                {
                    if (product.Chitietsanphams != null && product.Chitietsanphams.Any())
                    {
                        product.MinPrice = Convert.ToDecimal(product.Chitietsanphams.Min(x => x.DonGia));
                        product.MaxPrice = Convert.ToDecimal(product.Chitietsanphams.Max(x => x.DonGia));
                    }
                }
                ViewBag.TotalPages = ConvertResponseProduct["totalPages"].Value<int>();
                ViewBag.Page = ConvertResponseProduct["page"].Value<int>();
            };
            return View(ListProducts);
        }
    }
}
