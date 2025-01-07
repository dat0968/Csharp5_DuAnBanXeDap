using APIBanXeDap.Repository.DanhMuc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IDanhMucRepository DanhMucRepository;

        public CategoriesController(IDanhMucRepository DanhMucRepository)
        {
            this.DanhMucRepository = DanhMucRepository;
        }
        [HttpGet]
        public IActionResult GetAllCategory()
        {
            var ListCategories = DanhMucRepository.GetAllCategory();
            return Ok(ListCategories);
        }
    }
}
