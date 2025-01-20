using APIBanXeDap.EditModels;
using APIBanXeDap.Repository.NhaCungCap;
using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.Repository.ThuongHieu;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository SupplierRepository;

        public SuppliersController(ISupplierRepository SupplierRepository)
        {
            this.SupplierRepository = SupplierRepository;
        }
        [HttpGet("GetAllSupplier")]
        public IActionResult GetAllSupplier(string? keywords, string? sort, int page)
        {
            if(page >= 1)
            {
                int pagesize = 10;
                var ListSuppliers = SupplierRepository.GetAllSupplier(keywords, sort);
                var pagedSupplier = ListSuppliers.Skip((page - 1) * pagesize).Take(pagesize).ToList();
                var totalItems = ListSuppliers.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagesize);
                return Ok(new
                {
                    Data = pagedSupplier,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Page = page,
                });
            }
            else
            {
                var ListSuppliers = SupplierRepository.GetAllSupplier(keywords, sort);
                return Ok(ListSuppliers);
            }
           
        }

        // Lấy thông tin nhà cung cấp theo ID
        [HttpGet("{id}")]
        public IActionResult GetSupplierById([FromRoute] int id)
        {
            try
            {
                var detail = SupplierRepository.GetSupplierById(id);
                if (detail == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Không tìm thấy nhà cung cấp với ID = {id}"
                    });
                }
                return Ok(detail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}"
                });
            }
        }
        [HttpPost("CreateSupplier")]
        public IActionResult CreateSupplier(SupplierEM supplier)
        {
            try
            {
                if (supplier == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Dữ liệu nhà cung cấp không hợp lệ"
                    });
                }

                var createdSupplier = SupplierRepository.CreateSupplier(supplier);
                return Ok(new
                {
                    Success = true,
                    Message = "Thêm nhà cung cấp mới thành công",
                    Data = createdSupplier
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}"
                });
            }
        }
        [HttpPut("EditSupplier")]
        public IActionResult EditSupplier(SupplierEM supplier)
        {
            try
            {
                if (supplier == null || supplier.MaNhaCc <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Dữ liệu nhà cung cấp không hợp lệ"
                    });
                }

                SupplierRepository.UpdateSupplier(supplier);
                return Ok(new
                {
                    Success = true,
                    Message = "Sửa thông tin nhà cung cấp thành công"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Lỗi: {ex.Message}"
                });
            }
        }
        [HttpPut("DeleteSupplier/{id}")]
        public IActionResult DeleteSupplier([FromRoute] int id)
        {
            try
            {
                SupplierRepository.DeleteSupplier(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Xóa thông tin nhà cung cấp thành công",
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}"
                });
            }
        }
    }
}
