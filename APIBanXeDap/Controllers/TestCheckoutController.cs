using APIBanXeDap.Models;
using APIBanXeDap.Repository.ThanhToan;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCheckoutController : Controller
    {
        private readonly ICheckoutRepository CheckoutRepository;
        public TestCheckoutController(ICheckoutRepository CheckoutRepository)
        {
            this.CheckoutRepository = CheckoutRepository;
        }
        [HttpPost]
        public IActionResult CreateOder(HoadonVM model)
        {
            try
            {
                var hoadon = CheckoutRepository.CreateOrder(model);
                return Ok(new
                {
                    Success = true,
                    Data = hoadon
                });
            }
            catch(Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                });
            }
        }
        [HttpPost("CreateDetailOder")]
        public IActionResult CreateDetailOder(List<ChiTietHoaDonVM> model)
        {

            try
            {
                CheckoutRepository.CreateDetailOrder(model);
                return Ok(new
                {
                    Success = true,
                    Data = model
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    InnerException = ex.InnerException?.Message
                });
            }

        }
    }
}
