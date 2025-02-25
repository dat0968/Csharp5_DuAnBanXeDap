using APIBanXeDap.Models;
using APIBanXeDap.Repository;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _binhLuanRepository;

        public CommentsController(ICommentRepository binhLuanRepository)
        {
            _binhLuanRepository = binhLuanRepository;
        }

        [HttpGet("GetAllComments")]
        public IActionResult GetAllComments(string? keywords = null, int? rating = null, int page = 1, int pageSize = 10)
        {
            var query = _binhLuanRepository.GetAll()
                .Where(bl => !bl.IsDelete
                            && (string.IsNullOrEmpty(keywords) || bl.KhachHang.HoTen.Contains(keywords))
                            && (rating == null || bl.Rating == rating));

            // Lấy số lượng bình luận sau khi lọc
            var totalComments = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalComments / pageSize);

            // Phân trang
            var comments = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                
                .Select(bl => new
                {
                    bl.MaBinhLuan,
                    bl.NoiDung,
                    bl.NgayTao,
                    bl.Rating,
                    TenSanPham = bl.SanPham.TenSp,
                    TenKhachHang = bl.KhachHang.HoTen
                })
                .ToList();

            return Ok(new { data = comments, totalPages, page });
        }


        [HttpGet("GetCommentBy/{id}")]
        public ActionResult<Binhluan> GetById(int id)
        {
            var binhLuan = _binhLuanRepository.GetById(id);
            if (binhLuan == null)
            {
                return NotFound("Không tìm thấy bình luận.");
            }
            return Ok(binhLuan);
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] Binhluan binhLuan)
        {
            try
            {
                _binhLuanRepository.Create(binhLuan);
                return Ok("Tạo bình luận thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("edit")]
        public IActionResult Edit(Binhluan binhLuan)
        {
            try
            {
                _binhLuanRepository.Edit(binhLuan);
                return Ok("Cập nhật bình luận thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
