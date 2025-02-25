using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.ViewModels;
using System.Text;
using System.Text.Json;

namespace MVCBanXeDap.Controllers
{
    public class CommentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7137/api/Comments";
        private readonly string _apiUrls = "https://localhost:7137/api/Replys";

        public CommentController()
        {
            _httpClient = new HttpClient();
        }

        // Lấy danh sách bình luận (kèm phản hồi)
        public async Task<IActionResult> Index(string? keywords, int? rating, int page = 1)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/GetAllComments?keywords={keywords}&rating={rating}&page={page}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JsonElement>(data);
                var commentsJson = result.GetProperty("data").GetRawText();
                var comments = JsonSerializer.Deserialize<List<CommentVM>>(commentsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                foreach (var comment in comments)
                {
                    var replyResponse = await _httpClient.GetAsync($"{_apiUrls}/GetRepliesByCommentId/{comment.MaBinhLuan}");
                    if (replyResponse.IsSuccessStatusCode)
                    {
                        var replyData = await replyResponse.Content.ReadAsStringAsync();
                        comment.PhanHoi = JsonSerializer.Deserialize<List<ReplyVM>>(replyData, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                }

                ViewBag.TotalPages = result.GetProperty("totalPages").GetInt32();
                ViewBag.Page = result.GetProperty("page").GetInt32();
                ViewBag.Keywords = keywords;
                ViewBag.Rating = rating;

                return View(comments);
            }
            return View(new List<CommentVM>());
        }

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentVM comment)
        {
            var json = JsonSerializer.Serialize(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiUrl}/Create", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Không thể tạo bình luận.");
            return View(comment);
        }

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
