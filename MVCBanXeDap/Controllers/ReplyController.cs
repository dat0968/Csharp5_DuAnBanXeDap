using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.ViewModels;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace MVCBanXeDap.Controllers
{
    public class ReplyController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7137/api/ReplyComments";

        public ReplyController()
        {
            _httpClient = new HttpClient();
        }

        // Lấy danh sách phản hồi theo commentId
        public async Task<IActionResult> Index(int commentId)
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/GetRepliesByCommentId/{commentId}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var replies = JsonSerializer.Deserialize<List<ReplyCommentVM>>(data, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(replies);
            }
            return View(new List<ReplyCommentVM>());
        }

        // Tạo mới phản hồi
        public IActionResult Create(int commentId)
        {
            ViewBag.CommentId = commentId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReplyCommentVM reply)
        {
            var json = JsonSerializer.Serialize(reply);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiUrl}/create", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", new { commentId = reply.MaPhanHoi });
            }

            ModelState.AddModelError("", "Không thể tạo phản hồi.");
            return View(reply);
        }
    }
}
