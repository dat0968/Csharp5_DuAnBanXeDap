using APIBanXeDap.Models;
using APIBanXeDap.Repository.TraLoiBinhLuan;
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

        [HttpGet]
        public IActionResult GetAllReplies()
        {
            var replies = _replyRepository.GetAll();
            return Ok(replies);
        }

        [HttpGet("{id}")]
        public IActionResult GetReplyById(int id)
        {
            var reply = _replyRepository.GetById(id);
            if (reply == null)
            {
                return NotFound();
            }
            return Ok(reply);
        }

        [HttpPost]
        public IActionResult CreateReply([FromBody] Traloibinhluan reply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _replyRepository.Create(reply);
            _replyRepository.Save();

            return CreatedAtAction(nameof(GetReplyById), new { id = reply.MaTraLoi }, reply);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReply(int id, [FromBody] Traloibinhluan reply)
        {
            if (id != reply.MaTraLoi)
            {
                return BadRequest("Mã trả lời không khớp.");
            }

            _replyRepository.Update(reply);
            _replyRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReply(int id)
        {
            _replyRepository.Delete(id);
            _replyRepository.Save();
            return NoContent();
        }
    }
}
