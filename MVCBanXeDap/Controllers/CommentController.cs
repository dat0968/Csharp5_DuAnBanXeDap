using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.ViewModels;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace MVCBanXeDap.Controllers
{
    public class CommentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7137/api/Comments";

        public CommentController()
        {
            _httpClient = new HttpClient();
        }

        // Lấy danh sách bình luận (có tên sản phẩm và khách hàng)
        public async Task<IActionResult> Index(string? keywords, int? rating, int page = 1)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/GetAllComments?keywords={keywords}&rating={rating}&page={page}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                // Đọc object chứa data và thông tin trang
                var result = JsonSerializer.Deserialize<JsonElement>(data);

                // Lấy danh sách comment
                var commentsJson = result.GetProperty("data").GetRawText();
                var comments = JsonSerializer.Deserialize<List<CommentVM>>(commentsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Lấy thông tin phân trang
                ViewBag.TotalPages = result.GetProperty("totalPages").GetInt32();
                ViewBag.Page = result.GetProperty("page").GetInt32();
                ViewBag.Keywords = keywords;
                ViewBag.Rating = rating;

                return View(comments);
            }
            return View(new List<CommentVM>());
        }

        // Xem chi tiết bình luận
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var comment = JsonSerializer.Deserialize<CommentVM>(data);
                return View(comment);
            }
            return NotFound("Không tìm thấy bình luận.");
        }

        // Tạo mới bình luận
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentVM comment)
        {
            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiUrl}/create", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Không thể tạo bình luận.");
            return View(comment);
        }

        // Chỉnh sửa bình luận
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var comment = JsonSerializer.Deserialize<CommentVM>(data);
                return View(comment);
            }
            return NotFound("Không tìm thấy bình luận.");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentVM comment)
        {
            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiUrl}/edit", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Không thể cập nhật bình luận.");
            return View(comment);
        }
    }
}
