using APIBanXeDap.Repository.YeuThich;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IYeuThichRepository _yeuThichRepo;

        public WishlistController(IYeuThichRepository yeuThichRepo)
        {
            _yeuThichRepo = yeuThichRepo;
        }

        [HttpGet]
        public IActionResult IsAuth()
        {
            return Ok();
        }

        [HttpPut("{idProduct}")]
        public async Task<IActionResult> ChangeStatusWishlist(int idProduct)
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!idUser.HasValue)
            {
                return Unauthorized();
            }
            var result = await _yeuThichRepo.ChangeWishlist(idProduct, "SanPham", idUser.Value); // type Binhluan still not support
            return result;
        }

        [HttpPut]
        public async Task<IActionResult> ChangeListStatusWishlist([FromBody] int[] idProducts)
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!idUser.HasValue)
            {
                return Unauthorized();
            }
            foreach (var idProduct in idProducts)
            {
                await _yeuThichRepo.ChangeWishlist(idProduct, "SanPham", idUser.Value); // type Binhluan still not support
            }
            return Ok();
        }

        [HttpGet("{idProduct}")]
        public async Task<IActionResult> IsOneInWishlist(int idProduct)
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!idUser.HasValue)
            {
                return Unauthorized();
            }
            var exists = await _yeuThichRepo.IsOneInWishlist(idProduct, idUser.Value);
            return Ok(exists);
        }

        [HttpGet]
        public async Task<IActionResult> IsManyInWishlist([FromBody] int[] idProducts)
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!idUser.HasValue)
            {
                return Unauthorized();
            }
            var results = await _yeuThichRepo.IsManyInWishlist(idProducts, idUser.Value);
            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWishlistItems()
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!idUser.HasValue)
            {
                return Unauthorized();
            }
            var result = await _yeuThichRepo.GetAllYeuThichVMAsync(x => x.MaNguoiDung == idUser.Value, includeProperties: "Sanpham,Sanpham.MaNhaCcNavigation,Sanpham.MaThuongHieuNavigation,Sanpham.MaDanhMucNavigation");
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> CountWishlistOfUser()
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!idUser.HasValue)
            {
                return Unauthorized();
            }
            var result = await _yeuThichRepo.GetAllYeuThichVMAsync(x => x.MaNguoiDung == idUser.Value, includeProperties: "Sanpham,Sanpham.MaNhaCcNavigation,Sanpham.MaThuongHieuNavigation,Sanpham.MaDanhMucNavigation");
            return Ok(result.Count());
        }

        [HttpGet("{idProduct}")]
        public async Task<IActionResult> GetWishlistItem(int idProduct)
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (!idUser.HasValue)
            {
                return Unauthorized();
            }
            var result = await _yeuThichRepo.GetAsync(x => x.MaDoiTuong == idProduct &&  x.MaNguoiDung == idUser.Value, includeProperties: "Sanpham,Sanpham.MaNhaCcNavigation,Sanpham.MaThuongHieuNavigation,Sanpham.MaDanhMucNavigation");
            return Ok(result);
        }
    }
}
