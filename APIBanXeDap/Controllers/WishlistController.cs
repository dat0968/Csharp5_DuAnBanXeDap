using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        public WishlistController()
        {

        }
        [HttpPut("{idProduct}&{idUser}")]
        public IActionResult ChangeStatusWishlist(int idProduct, int idUser)
        {
            return Ok();
        }
        [HttpPut("{idUser}")]
        public IActionResult ChangeListStatusWishlist([FromBody] int[]idProducts, int idUser)
        {
            return Ok();
        }
        [HttpGet("{idProduct}&{idUser}")]
        public IActionResult IsOneInWishlist(int idProduct, int idUser)
        {
            return Ok(new Random().NextDouble() > 0.5); //Type: bool
        }
        [HttpGet("{idUser}")]
        public IActionResult IsManyInWishlist([FromBody] int[]idProducts, int idUser)
        {
            return Ok(Enumerable.Range(0, 10).Select(x => new Random().NextDouble() > 0.5).ToArray()); //Type: bool[]
        }
    }
}
