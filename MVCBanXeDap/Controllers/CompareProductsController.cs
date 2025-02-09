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

        public CompareProductsController(IjwtToken jwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            this.jwtToken = jwtToken;
        }

        public IActionResult Index()
        {
            var productVMs = HttpContext.Session?.Get<List<CompareProductVM>>("ComparesProducts") ?? new List<CompareProductVM>(3);
            return View(productVMs);
        }

        public async Task<IActionResult> AddProductToCompare(int id)
        {
            // Lấy danh sách sản phẩm từ Session, mặc định là mảng có chứa 3 sản phẩm nếu không tồn tại
            var productVMs = HttpContext.Session?.Get<List<CompareProductVM>>("ComparesProducts") ?? new List<CompareProductVM>(3);

            // Kiểm tra xem sản phẩm đã có trong danh sách so sánh chưa
            if (productVMs.Any(pr => pr?.MaSp == id))
            {
                return Json(new { success = false, message = "Sản phẩm đã tồn tại trong danh sách sản phẩm so sánh." });
            }

            // Gọi API để lấy thông tin sản phẩm theo ID
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"{_client.BaseAddress}CompareProducts/AddProductToCopare/{id}");

            // Kiểm tra xem yêu cầu API có thành công không
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = "Không thể lấy thông tin sản phẩm." });
            }

            // Đọc dữ liệu sản phẩm từ phản hồi
            string data = await httpResponseMessage.Content.ReadAsStringAsync();
            CompareProductVM pVM = JsonConvert.DeserializeObject<CompareProductVM>(data);

            // Thêm sản phẩm vào danh sách so sánh nếu có chỗ trống
            if (productVMs.Count < 3)
            {
                productVMs.Add(pVM); // Thêm sản phẩm vào danh sách
                HttpContext.Session.Set("ComparesProducts", productVMs); // Lưu lại vào Session
                return Json(new { success = true, message = "Sản phẩm đã được thêm vào danh sách so sánh!" });
            }

            return Json(new { success = false, message = "Danh sách sản phẩm so sánh đã đầy." });
        }

        public async Task<IActionResult> RemoveProductFromCompare(int id)
        {
            // Lấy danh sách sản phẩm từ Session, mặc định là mảng có chứa 3 sản phẩm nếu không tồn tại
            var productVMs = HttpContext.Session?.Get<List<CompareProductVM>>("ComparesProducts") ?? new List<CompareProductVM>(3);

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

        public async Task<IActionResult> QuickBarComparesProducts()
        {
            var productVMs = HttpContext.Session?.Get<List<CompareProductVM>>("ComparesProducts") ?? new List<CompareProductVM>(3);
            return PartialView(productVMs);
        }
    }
}
