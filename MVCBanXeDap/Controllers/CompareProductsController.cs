using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Helper;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;

namespace MVCBanXeDap.Controllers
{
    public class CompareProductsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;

        private const string CompareSessionKey = "ComparesProducts";
        private const int MaxCompareItems = 3;

        public CompareProductsController(IjwtToken jwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            this.jwtToken = jwtToken;
        }

        #region[VIEW DATA]
        public IActionResult Index()
        {
            var productVMs = GetCompareProductsFromSession();
            return View(productVMs);
        }
        public async Task<IActionResult> QuickBarComparesProducts()
        {
            var productVMs = GetCompareProductsFromSession();
            return PartialView(productVMs);
        }
        #endregion

        #region[ACTIONS]
        public async Task<IActionResult> AddProductToCompare(int id)
        {
            var productVMs = GetCompareProductsFromSession();

            if (productVMs.Any(pr => pr.MaSp == id))
            {
                return Json(new { success = false, message = "Sản phẩm đã có trong danh sách so sánh." });
            }

            if (productVMs.Count >= MaxCompareItems)
            {
                return Json(new { success = false, message = "Danh sách so sánh đã đầy." });
            }

            // Gọi API chỉ khi cần
            var response = await _client.GetAsync($"CompareProducts/AddProductToCopare/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin sản phẩm." });
            }

            var data = await response.Content.ReadAsStringAsync();
            var newProduct = JsonConvert.DeserializeObject<CompareProductVM>(data);

            productVMs.Add(newProduct);
            SaveCompareProductsToSession(productVMs);

            return Json(new { success = true, message = "Sản phẩm đã được thêm!" });
        }

        public IActionResult RemoveProductFromCompare(int id)
        {
            // Lấy danh sách sản phẩm từ Session, mặc định là mảng có chứa 3 sản phẩm nếu không tồn tại
            var productVMs = GetCompareProductsFromSession();

            // Tìm và xóa sản phẩm trong danh sách nếu tồn tại
            var productToRemove = productVMs.FirstOrDefault(pr => pr?.MaSp == id);
            if (productToRemove != null)
            {
                productVMs.Remove(productToRemove); // Xóa sản phẩm
                HttpContext.Session.Set("ComparesProducts", productVMs); // Cập nhật vào Session
                return Json(new { success = true, message = "Sản phẩm đã được xóa khỏi danh sách so sánh!" });
            }

            return Json(new { success = false, message = "Sản phẩm không tồn tại trong danh sách so sánh." });
        }
        #endregion

        #region[NON ACTION]

        private List<CompareProductVM> GetCompareProductsFromSession()
        {
            return HttpContext.Session?.Get<List<CompareProductVM>>(CompareSessionKey) ?? new List<CompareProductVM>();
        }

        private void SaveCompareProductsToSession(List<CompareProductVM> productVMs)
        {
            HttpContext.Session.Set(CompareSessionKey, productVMs);
        }
        #endregion
    }
}
