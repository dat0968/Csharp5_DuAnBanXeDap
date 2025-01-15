using APIBanXeDap.Models;
using APIBanXeDap.Repository.HoaDon;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IHoaDonRepository hoaDonRepository;
        public BillController(IHoaDonRepository hoaDonRepository)
        {
            this.hoaDonRepository = hoaDonRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await hoaDonRepository.GetAllAsync());
        }
        [HttpGet("{maHoadon}")]
        public async Task<IActionResult> Get(int maHoadon)
        {
            return Ok(await hoaDonRepository.GetAsync(x => x.MaHoaDon == maHoadon));
        }
        [HttpPut]
        public async Task<IActionResult> ChangeStatusOrder(int idOrder, int idStaff, string statusOrder)
        {
            await hoaDonRepository.ChangStatusOrder(idOrder, idStaff, statusOrder);
            
            return Ok();
        }
        [HttpGet("{maHoaDon}")]
        public async Task<IActionResult> GetInvoiceData(int maHoaDon)
        {
            return Ok(await hoaDonRepository.GetInvoiceDataAsync(maHoaDon));
        }
    }
}
