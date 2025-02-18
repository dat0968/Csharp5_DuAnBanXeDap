using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Helper;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public IActionResult Index()
        {
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
            int? idUser = await GetUserIdAsync();
            if (idUser == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thay đổi yêu thích.", isLoginAgain = true });
            }

            var response = await _client.PutAsJsonAsync(_client.BaseAddress + $"Wishlist/ChangeStatusWishlist/{idProduct}&{idUser}", new { });
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

            int? idUser = await GetUserIdAsync();
            if (idUser == null)
            {
                return Json(new { data = false });
            }

            var response = await _client.GetAsync(_client.BaseAddress + $"Wishlist/IsOneInWishlist/{idProduct}&{idUser}");
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

            int? idUser = await GetUserIdAsync();
            if (idUser == null)
            {
                return Json(new { data = Enumerable.Repeat(false, idProducts.Length).ToArray() });
            }

            var response = await _client.PostAsJsonAsync(_client.BaseAddress + $"Wishlist/IsManyInWishlist/{idUser}", idProducts);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<object>();
                return Json(result);
            }

            return Json(new { success = false, message = "Có lỗi xảy ra." });
        }

        public async Task<IActionResult> GetWishlistData()
        {
            int? idUser = await GetUserIdAsync();
            if (idUser == null)
            {
                return Json(new { success = false, message = "Phiên của bạn đã hết, vui lòng đăng nhập lại.", isLoginAgain = true });
            }

            var response = await _client.GetAsync(_client.BaseAddress + $"Wishlist/GetAllWishlistItems/{idUser}");
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
        [NonAction]
        private async Task<int?> GetUserIdAsync()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (accessToken == null || refreshToken == null)
            {
                return null;
            }

            var validateAccessToken = await jwtToken.ValidateAccessToken();
            if (validateAccessToken == null)
            {
                return null;
            }
            else
            {
                HttpContext.Session.SetString("AccessToken", validateAccessToken);
            }
            var information = jwtToken.GetInformationUserFromToken(validateAccessToken);
            var id = information.Id;
            return id;
        }
        #endregion
    }
}
