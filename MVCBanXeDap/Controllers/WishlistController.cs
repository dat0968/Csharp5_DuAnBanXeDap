using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Helper;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVCBanXeDap.Controllers
{
    public class WishlistController : Controller
    {
        private readonly Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;

        public WishlistController()
        {
            _client = new HttpClient { BaseAddress = baseAddress };
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ChangeWishlist(int idProduct, string typeObject)
        {
            int idUser = 100; //Note: Demo
            var wishlistVMs = TakeWishlistFromSession();
            var wishlistOne = wishlistVMs?.FirstOrDefault(x => x.MaYeuThich == idProduct && x.DoiTuongYeuThich == typeObject && x.MaNguoiDung == idUser);

            if (wishlistOne == null) // Nếu sản phẩm chưa có trong danh sách yêu thích
            {
                wishlistVMs.Add(new WishlistVM
                {
                    MaYeuThich = idProduct,
                    MaNguoiDung = idUser
                });

                SetWishlistFromSession(wishlistVMs);

                return Json(new
                {
                    success = true,
                    message = "Sản phẩm đã được thêm vào danh sách yêu thích. (Developer)"
                });
            }

            wishlistVMs.Remove(wishlistOne);
            SetWishlistFromSession(wishlistVMs);

            return Json(new
            {
                success = true,
                message = "Đã xóa sản phẩm khỏi danh sách yêu thích."
            });
        }

        public IActionResult IsOneInWishlist(int idProduct)
        {
            int idUser = 100; //Note: Demo
            var wishlistVMs = TakeWishlistFromSession();
            var exists = wishlistVMs?.Any(x => x.MaYeuThich == idProduct && x.MaNguoiDung == idUser) ?? false;
            return Json(new { data = exists });
        }

        public IActionResult IsManyInWishlist(int[] idProducts)
        {
            int idUser = 100; //Note: Demo
            var wishlistVMs = TakeWishlistFromSession();
            var results = wishlistVMs?.Select(x => idProducts.Contains(x.MaYeuThich) && x.MaNguoiDung == idUser).ToArray();
            return Json(new { data = results});
        }

        private List<WishlistVM> TakeWishlistFromSession()
        {
            var wishlistVMs = HttpContext.Session.Get<List<WishlistVM>>("demoWishlist") ?? new List<WishlistVM>();

            if (wishlistVMs.Count == 0)
            {
                SeedWishlist(wishlistVMs);
            }

            return wishlistVMs;
        }

        private void SeedWishlist(List<WishlistVM> wishlistVMs) //Tạo danh sách yêu thích ngẫu nhiên
        {
            Random rd = new Random();
            for (int i = 0; i < 20; i++)
            {
                bool isComment = rd.NextDouble() > 0.5;
                wishlistVMs.Add(new WishlistVM
                {
                    MaYeuThich = 100 + i,
                    MaNguoiDung = 100,
                    DoiTuongYeuThich = (isComment ? "BinhLuan" : "SanPham"),
                    DanhMuc = "Danh Mục Ngẫu Nhiên",
                    Hinh = "https://picsum.photos/100/150",
                    NoiDungBinhLuan = (isComment ? $"Bình luận ngẫu nhiên tạo {i}" : ""),
                    NgayBinhLuan = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-rd.Next(1, 10))),
                    KhoangGia = "123.123.000 đ - 999.000.000 đ",
                    TenSp = $"Sản Phẩm Ngẫu Nhiên {i}",
                    ThuongHieu = "Ngẫu Nhiên",
                    NhaCungCap = "Công ty Ngẫu Nhiên",
                    SoLuong = new Random().Next(5, 10)
                });
            }
            SetWishlistFromSession(wishlistVMs);
        }

        private void SetWishlistFromSession(List<WishlistVM> wishlistVMs) //Set dữ liệu vào Session
        {
            HttpContext.Session.Set("demoWishlist", wishlistVMs);
        }

        public IActionResult GetWishlistData()
        {
            var wishlistVMs = TakeWishlistFromSession();
            return Json(wishlistVMs);
        }
    }
}
