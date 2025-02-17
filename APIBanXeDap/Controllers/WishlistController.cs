using APIBanXeDap.Repository.YeuThich;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut("{idProduct}&{idUser}")]
        public async Task<IActionResult> ChangeStatusWishlist(int idProduct, int idUser)
        {
            var result = await _yeuThichRepo.ChangeWishlist(idProduct, "SanPham", idUser); // type Binhluan still not support
            return result;
        }

        [HttpPut("{idUser}")]
        public async Task<IActionResult> ChangeListStatusWishlist([FromBody] int[] idProducts, int idUser)
        {
            foreach (var idProduct in idProducts)
            {
                await _yeuThichRepo.ChangeWishlist(idProduct, "SanPham", idUser); // type Binhluan still not support
            }
            return Ok();
        }

        [HttpGet("{idProduct}&{idUser}")]
        public async Task<IActionResult> IsOneInWishlist(int idProduct, int idUser)
        {
            var exists = await _yeuThichRepo.IsOneInWishlist(idProduct, idUser);
            return Ok(exists);
        }

        [HttpGet("{idUser}")]
        public async Task<IActionResult> IsManyInWishlist([FromBody] int[] idProducts, int idUser)
        {
            var results = await _yeuThichRepo.IsManyInWishlist(idProducts, idUser);
            return Ok(results);
        }

        [HttpGet("{idUser}")]
        public async Task<IActionResult> GetAllWishlistItems(int idUser)
        {
            var result = await _yeuThichRepo.GetAllYeuThichVMAsync(x => x.MaNguoiDung == idUser, includeProperties: "Sanpham");
            return Ok(result);
        }
        [HttpGet("{idUser}")]
        public async Task<IActionResult> CountWishlistOfUser(int idUser)
        {
            var result = await _yeuThichRepo.GetAllYeuThichVMAsync(x => x.MaNguoiDung == idUser, includeProperties: "Sanpham");
            return Ok(result.Count());
        }

        [HttpGet("{idProduct}&{idUser}")]
        public async Task<IActionResult> GetWishlistItem(int idProduct, int idUser)
        {
            var result = await _yeuThichRepo.GetAsync(x => x.MaDoiTuong == idProduct && x.MaNguoiDung == idUser, includeProperties: "Sanpham");
            return Ok(result);
        }
    }
}
