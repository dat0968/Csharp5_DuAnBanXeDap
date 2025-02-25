using APIBanXeDap.Models;
using APIBanXeDap.Repository.TraLoiBinhLuan;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplysController : ControllerBase
    {
        private readonly IReplyCommentRepository _replyRepository;

        public ReplysController(IReplyCommentRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        // 🟢 API lấy tất cả trả lời bình luận
        [HttpGet]
        public IActionResult GetAllReplies()
        {
            var replies = _replyRepository.GetAll()
                .Select(r => new ReplyCommentVM
                {
                    MaPhanHoi = r.MaTraLoi,
                    MaBinhLuan = r.MaBinhLuan,
                    NoiDung = r.NoiDung,
                    TenNguoiDung = r.NhanVien?.HoTen ?? "Ẩn danh",
                    NgayTao = r.ThoiGian
                })
                .ToList();

            if (replies.Count == 0)
            {
                return NotFound(new { message = "Không có câu trả lời nào." });
            }

            return Ok(replies);
        }


        // 🟢 API lấy trả lời theo mã trả lời
        [HttpGet("GetReplyBy/{id}")]
        public IActionResult GetReplyById(int id)
        {
            var reply = _replyRepository.GetById(id);

            if (reply == null)
            {
                return NotFound(new { message = "Không tìm thấy trả lời với ID này." });
            }

            var viewModel = new ReplyCommentVM
            {
                MaPhanHoi = reply.MaTraLoi,
                MaBinhLuan = reply.MaBinhLuan,
                NoiDung = reply.NoiDung,
                TenNguoiDung = reply.NhanVien?.HoTen ?? "Ẩn danh",
                NgayTao = reply.ThoiGian
            };

            return Ok(viewModel);
        }

        // 🟢 API tạo trả lời mới
        [HttpPost("Create")]
        public IActionResult CreateReply(ReplyCommentVM replyVM)
        {
            var nhanVien = _replyRepository.Create;
            if (nhanVien == null)
            {
                return BadRequest(new { message = "Mã nhân viên không tồn tại!" });
            }

            var reply = new Traloibinhluan
            {
                MaBinhLuan = replyVM.MaBinhLuan,
                NoiDung = replyVM.NoiDung,
                MaNV = replyVM.MaNV,
                ThoiGian = DateTime.Now
            };

            _replyRepository.Create(reply);
            _replyRepository.Save();

            return Ok(new { message = "Trả lời bình luận thành công!" });
        }


        // 🟢 API cập nhật trả lời
        [HttpPut("Edit")]
        public IActionResult UpdateReply(int id, [FromBody] ReplyCommentVM replyVM)
        {
            var existingReply = _replyRepository.GetById(id);
            if (existingReply == null)
            {
                return NotFound(new { message = "Không tìm thấy trả lời để cập nhật." });
            }

            existingReply.NoiDung = replyVM.NoiDung;
            existingReply.ThoiGian = DateTime.Now;

            _replyRepository.Update(existingReply);
            _replyRepository.Save();

            return NoContent();
        }

        // 🟢 API xóa trả lời
        [HttpDelete("Delete")]
        public IActionResult DeleteReply(int id)
        {
            var reply = _replyRepository.GetById(id);
            if (reply == null)
            {
                return NotFound(new { message = "Không tìm thấy trả lời để xóa." });
            }

            _replyRepository.Delete(id);
            _replyRepository.Save();
            return NoContent();
        }
    }
}
