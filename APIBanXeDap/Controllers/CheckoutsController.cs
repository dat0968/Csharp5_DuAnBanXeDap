using APIBanXeDap.Models;
using APIBanXeDap.Repository.MaCoupon;
using APIBanXeDap.Repository.ThanhToan;
using APIBanXeDap.ViewModels;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using System.Web;


namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly ICheckoutRepository CheckoutRepository;
        private readonly IMaCouponRepository MaCouponRepository;
        private readonly Csharp5Context db;
        public CheckoutsController(IConfiguration configuration, ICheckoutRepository CheckoutRepository, IMaCouponRepository MaCouponRepository, Csharp5Context db)
        {
            _configuration = configuration;
            this.CheckoutRepository = CheckoutRepository;
            this.MaCouponRepository = MaCouponRepository;
            this.db = db;
        }
        [HttpPost("CheckoutOrders")]
        public IActionResult CheckoutOrders([FromBody] ThongTinHoaDonVM model)
        {
            int? MaHoaDon = null;
            db.Database.BeginTransaction();
            try
            {
                var hoadon = CheckoutRepository.CreateOrder(model.HoaDon);
                MaHoaDon = hoadon.MaHoaDon;
                foreach (var chitiet in model.ChiTietHoaDons)
                {
                    chitiet.MaHoaDon = (int)MaHoaDon;
                }
                if (!string.IsNullOrEmpty(model.MaCoupon))
                {
                    MaCouponRepository.RevokeCouponCode(model.MaCoupon);
                }
                CheckoutRepository.CreateDetailOrder(model.ChiTietHoaDons);
                CheckoutRepository.UpdateQuantityProduct(model.ChiTietHoaDons);
                db.Database.CommitTransaction();
                return Ok(new
                {
                    Success = true,
                    IDorder = MaHoaDon,
                    Message = "Cập nhật hóa đơn thành công"
                });
            }
            catch (Exception ex)
            {
                db.Database.RollbackTransaction();
                return Ok(new
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                });
            }
        }
    }
}
