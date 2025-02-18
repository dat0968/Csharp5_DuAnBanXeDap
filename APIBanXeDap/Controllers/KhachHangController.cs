using APIBanXeDap.Models;
using APIBanXeDap.Repository;
using APIBanXeDap.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;


//using APIBanXeDap.Service;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Nhân viên")]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll(string? keyword, string? sort)
        {
            var result = _khachHangService.GetAllKhachHang(keyword, sort);
            return Ok(result);
        }

        [HttpPost("Add")]
        public IActionResult Add( KhachHangVM khachHangVM)
        {
            try
            {
                var result = _khachHangService.AddKhachHang(khachHangVM);
                return Ok(new { success = true, message = "Thêm khách hàng thành công!", data = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromForm] KhachHangVM khachHangVM)
        {
            var result = _khachHangService.UpdateKhachHang(id, khachHangVM);
            return Ok(result);
        }

        [HttpPost("ToggleStatus/{id}")]
        public IActionResult ToggleStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest(new { success = false, message = "Trường 'status' không được để trống." });
            }

            try
            {
                _khachHangService.ToggleStatus(id, status);
                return Ok(new { success = true, message = "Cập nhật trạng thái thành công." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { success = false, message = "Không tìm thấy khách hàng." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpPut("ToggleIsDelete/{id}")]
        public IActionResult ToggleIsDelete(int id)
        {
            try
            {
                _khachHangService.ToggleIsDelete(id);
                return Ok(new { success = true, message = "Trạng thái IsDelete đã được thay đổi thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpGet("Search")]
        public IActionResult Search(string keyword, string? sort)
        {
            var result = _khachHangService.GetAllKhachHang(keyword, sort);
            return Ok(result);
        }
        [HttpGet("GetKhachHangById/{id}")]
        public IActionResult GetKhachHangById(int id)
        {
            try
            {
                var khachHang = _khachHangService.GetKhachHangById(id);
                if (khachHang == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy khách hàng" });
                }
                return Ok(new { success = true, data = khachHang });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("ImportExcel")]
        public IActionResult ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { success = false, message = "File không hợp lệ hoặc rỗng!" });
            }

            try
            {
                var importedKhachHangs = new List<KhachHangVM>();

                using (var stream = file.OpenReadStream())
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1); // Lấy sheet đầu tiên
                    var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua tiêu đề cột (hàng đầu tiên)

                    foreach (var row in rows)
                    {
                        try
                        {
                            var khachHang = new KhachHangVM
                            {
                                HoTen = row.Cell(1).GetString(),
                                GioiTinh = row.Cell(2).GetString(),
                                NgaySinh = row.Cell(3).TryGetValue<DateTime>(out var ngaySinh)
                                    ? (DateOnly?)DateOnly.FromDateTime(ngaySinh)
                                    : null,
                                DiaChi = row.Cell(4).GetString(),
                                Cccd = row.Cell(5).GetString(),
                                Sdt = row.Cell(6).GetString(),
                                Email = row.Cell(7).GetString(),
                                TenTaiKhoan = row.Cell(8).GetString(),
                                MatKhau = row.Cell(9).GetString()
                            };

                            importedKhachHangs.Add(khachHang);
                        }
                        catch (Exception cellEx)
                        {
                            return BadRequest(new { success = false, message = $"Lỗi dữ liệu tại dòng {row.RowNumber()}: {cellEx.Message}" });
                        }
                    }
                }

                _khachHangService.ImportKhachHangs(importedKhachHangs);

                return Ok(new { success = true, message = "Nhập dữ liệu từ file Excel thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpGet("GetPaged")]
        public IActionResult GetPaged(int pageNumber, int pageSize, string? keyword, string? sort, string? status, string? gender)
        {
            var result = _khachHangService.GetPagedKhachHang(pageNumber, pageSize, keyword, sort, status, gender);
            return Ok(result);
        }


    }
}

