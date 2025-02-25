using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Helper;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MVCBanXeDap.Controllers
{
    public class WishlistController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;

        public WishlistController(IjwtToken jwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            this.jwtToken = jwtToken;
        }

        public async Task<IActionResult> Index()
        {
            SetAuthorizationHeader();
            var response = await _client.GetAsync(_client.BaseAddress + "wishlist/IsAuth");
            if (!response.IsSuccessStatusCode)
            {
                int Status = (int)response.StatusCode;
                return RedirectToAction("Error", "Home", Status);
            }
            return View();
        }

        public async Task<IActionResult> ChangeWishlist(int idProduct, string typeObject)
        {
            string? roleUser = GetUserRole();

            if (roleUser == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thay đổi yêu thích.", isLoginAgain = true });
            }

            if (roleUser == "Staff")
            {
                return Json(new { success = false, message = "Bạn không phải là khách hàng để yêu thích sản phẩm." });
            }
            SetAuthorizationHeader();

            var response = await _client.PutAsJsonAsync(_client.BaseAddress + $"Wishlist/ChangeStatusWishlist/{idProduct}", new { });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<object>();
                return Json(result);
            }

            return Json(new { success = false, message = "Có lỗi xảy ra." });
        }

        public async Task<IActionResult> IsOneInWishlist(int idProduct)
        {
            string? roleUser = GetUserRole();

            if (roleUser == null)
            {
                return Json(new { data = false });
            }

            if (roleUser == "Staff")
            {
                return Json(new { data = false });
            }

            SetAuthorizationHeader();

            var response = await _client.GetAsync(_client.BaseAddress + $"Wishlist/IsOneInWishlist/{idProduct}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<object>();
                return Json(result);
            }

            return Json(new { success = false, message = "Có lỗi xảy ra." });
        }

        public async Task<IActionResult> IsManyInWishlist(int[] idProducts)
        {
            string? roleUser = GetUserRole();

            if (roleUser == null)
            {
                return Json(new { data = Enumerable.Repeat(false, idProducts.Length).ToArray() });
            }

            if (roleUser == "Staff")
            {
                return Json(new { data = Enumerable.Repeat(false, idProducts.Length).ToArray() });
            }

            SetAuthorizationHeader();

            var response = await _client.PostAsJsonAsync(_client.BaseAddress + $"Wishlist/IsManyInWishlist/", idProducts);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<object>();
                return Json(result);
            }

            return Json(new { success = false, message = "Có lỗi xảy ra." });
        }

        public async Task<IActionResult> GetWishlistData()
        {
            SetAuthorizationHeader();

            var response = await _client.GetAsync(_client.BaseAddress + $"Wishlist/GetAllWishlistItems/");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<IEnumerable<WishlistVM>>();
                return Json(result);
            }

            return Json(new { success = false, message = "Có lỗi xảy ra." });
        }

        #region //NonAction
        [NonAction]
        private string? GetUserRole()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null)
            {
                return null;
            }

            return role;
        }
        private async void SetAuthorizationHeader()
        {
            var validateAccessToken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                var accesstoken = validateAccessToken;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            }
        }
        #endregion
    }
}
