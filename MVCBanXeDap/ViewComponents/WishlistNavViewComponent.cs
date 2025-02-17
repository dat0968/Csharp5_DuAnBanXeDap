using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using System.Net.Http.Json;

namespace MVCBanXeDap.ViewComponents
{
    public class WishlistNavViewComponent : ViewComponent
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;

        public WishlistNavViewComponent(IjwtToken jwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            this.jwtToken = jwtToken;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new WishlistNavVM();

            string? userRole = GetUserRole();
            int? userId = await GetUserIdAsync();

            model.IsShowWishlistNav = userRole == "Customer";

            if (!model.IsShowWishlistNav)
            {
                return View("Default", model: model);
            }

            var response = await _client.GetAsync(_client.BaseAddress + $"Wishlist/CountWishlistOfUser/{userId}");

            if (response.IsSuccessStatusCode)
            {
                model.NumberOfWishlists = await response.Content.ReadFromJsonAsync<int>();
                return View("Default", model: model);
            }

            model.NumberOfWishlists = null;
            return View("Default", model: model);
        }


        private string? GetUserRole()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null)
            {
                return null;
            }

            return role;
        }

        private async Task<int?> GetUserIdAsync()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (accessToken == null || refreshToken == null)
            {
                return null;
            }

            var validateAccessToken = await jwtToken.ValidateAccessToken(accessToken, refreshToken);
            if (validateAccessToken == null)
            {
                return null;
            }
            else
            {
                HttpContext.Session.SetString("AccessToken", validateAccessToken);
            }

            return Int32.Parse(jwtToken.GetUserIdFromToken(validateAccessToken));
        }
    }
}
