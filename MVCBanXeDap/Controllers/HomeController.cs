using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVCBanXeDap.Models;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace MVCBanXeDap.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _apiUri = new Uri("https://localhost:7137/api/");
        private readonly IjwtToken jwtToken;

        public HomeController(IjwtToken jwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = _apiUri;
            this.jwtToken = jwtToken;
        }
        [NonAction]
        [HttpGet]
        public async void SetAuthorizationHeader()
        {
            var validateAccessToken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                var accesstoken = validateAccessToken;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SetAuthorizationHeader();
            var list = new List<ProductVM>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "Home/SanPhamBanChay");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var convertResponse = JsonConvert.DeserializeObject<List<ProductVM>>(data);
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
            else
            {
                return StatusCode((int)response.StatusCode);
            }
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ProductVM = new ProductVM();
            SetAuthorizationHeader();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"Home/GetProductById/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                ProductVM = JsonConvert.DeserializeObject<ProductVM>(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
            string ten = ProductVM.DanhMuc;
            var relatedProducts = await _client.GetAsync(_client.BaseAddress + $"Home/GetSanPhamLienQuan/{ten}");
            List<ProductVM> relatedProductsList = new List<ProductVM>();

            if (relatedProducts.IsSuccessStatusCode)
            {
                string relatedData = await relatedProducts.Content.ReadAsStringAsync();
                relatedProductsList = JsonConvert.DeserializeObject<List<ProductVM>>(relatedData);
                foreach (var item in relatedProductsList)
                {
                    if (item.Chitietsanphams != null && item.Chitietsanphams.Any())
                    {
                        var minPrice = Convert.ToDecimal(item.Chitietsanphams.Min(x => x.DonGia));
                        var maxPrice = Convert.ToDecimal(item.Chitietsanphams.Max(x => x.DonGia));

                        // Cập nhật MinPrice và MaxPrice cho sản phẩm liên quan
                        item.MinPrice = minPrice;
                        item.MaxPrice = maxPrice;
                    }
                }
            }
            else
            {
                return StatusCode((int)relatedProducts.StatusCode);
            }
            // Truyền dữ liệu sản phẩm liên quan vào ViewBag hoặc View
            ViewBag.RelatedProducts = relatedProductsList;
            return View(ProductVM);
        }
        [HttpGet]
        public async Task<IActionResult> Product(int? maThuongHieu, string? timKiem, int? maDanhMuc, string? sapXep, int page = 1)
        {
            var ListProducts = new List<ProductVM>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"Products/GetAllProduct?keywords={timKiem}&MaDanhMuc={maDanhMuc}&MaThuongHieu={maThuongHieu}&sort={sapXep}&page={page}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var ConvertResponseProduct = JsonConvert.DeserializeObject<JObject>(data);
                ListProducts = ConvertResponseProduct["data"].ToObject<List<ProductVM>>();
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
            
            ViewBag.MaThuongHieu = maThuongHieu;
            ViewBag.TimKiem = timKiem;
            ViewBag.MaDanhMuc = maDanhMuc;
            ViewBag.SapXep = sapXep;
            var ListCategory = new List<DanhmucVM>();
            HttpResponseMessage responseCategory = _client.GetAsync(_client.BaseAddress + "Categories/GetAllCategory").Result;
            if (responseCategory.IsSuccessStatusCode)
            {
                string data = responseCategory.Content.ReadAsStringAsync().Result;
                ListCategory = JsonConvert.DeserializeObject<List<DanhmucVM>>(data);
                ViewBag.Category = ListCategory;
            }
            var ListBrand = new List<BrandVM>();
            HttpResponseMessage responseBrand = _client.GetAsync(_client.BaseAddress + "Brands/GetAllBrand").Result;
            if (responseBrand.IsSuccessStatusCode)
            {
                string data = responseBrand.Content.ReadAsStringAsync().Result;
                ListBrand = JsonConvert.DeserializeObject<List<BrandVM>>(data);
                ViewBag.Brand = ListBrand;
            }
            return View(ListProducts);
        }
        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Oops! Liên kết này không tồn tại.";
                    break;
                case 401:
                    ViewBag.ErrorMessage = "Không thể truy cập vào đường dẫn này hoặc do phiên của bạn đã hết. Vui lòng thử đăng nhập lại.";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Lỗi máy chủ nội bộ! Vui lòng thử lại sau.";
                    break;
                default:
                    ViewBag.ErrorMessage = "Đã xảy ra lỗi!";
                    break;
            }
            return View();
        }

    }
}
