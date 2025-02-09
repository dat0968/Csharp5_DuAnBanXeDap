using APIBanXeDap.Models;
using APIBanXeDap.Models.ViewModels;
using APIBanXeDap.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace APIBanXeDap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienService _nhanVienService;

        public NhanVienController(INhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll(string? keyword, string? sort, string? status, string? gender)
        {
            var result = _nhanVienService.GetAllNhanVien(keyword, sort, status, gender);
            return Ok(result);
        }
        [HttpPost("Add")]
        public IActionResult Add([FromForm] NhanVienVM nhanVienVM)
        {
            try
            {
                var result = _nhanVienService.AddNhanVien(nhanVienVM);
                return Ok(new { success = true, message = "Thêm nhân viên thành công!", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromForm] NhanVienVM nhanVienVM)
        {
            try
            {
                var result = _nhanVienService.UpdateNhanVien(id, nhanVienVM);
                return Ok(new { success = true, message = "Cập nhật nhân viên thành công!", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
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
                _nhanVienService.ToggleStatus(id, status);
                return Ok(new { success = true, message = "Cập nhật trạng thái thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("ToggleIsDelete/{id}")]
        public IActionResult ToggleIsDelete(int id)
        {
            try
            {
                _nhanVienService.ToggleIsDelete(id);
                return Ok(new { success = true, message = "Đổi trạng thái IsDelete và xóa thông tin tài khoản thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpGet("Search")]
        public IActionResult Search(string keyword, string? sort, string? status = null, string? gender = null)
        {
            var result = _nhanVienService.GetAllNhanVien(keyword, sort, status, gender);
            return Ok(result);
        }


        [HttpGet("GetNhanVienById/{id}")]
        public IActionResult GetNhanVienById(int id)
        {
            try
            {
                var nhanVien = _nhanVienService.GetNhanVienById(id);
                if (nhanVien == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy nhân viên" });
                }
                return Ok(new { success = true, data = nhanVien });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("ImportExcel")]
        public IActionResult ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { success = false, message = "File không hợp lệ!" });
            }

            try
            {
                var importedNhanViens = new List<NhanVienVM>();

                using (var stream = file.OpenReadStream())
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1); // Lấy sheet đầu tiên
                    var rows = worksheet.RowsUsed().Skip(1); // Bỏ qua tiêu đề cột (hàng đầu tiên)

                    foreach (var row in rows)
                    {
                        var nhanVien = new NhanVienVM
                        {
                            HoTen = row.Cell(1).GetString(),
                            GioiTinh = row.Cell(2).GetString(),
                            NgaySinh = row.Cell(3).TryGetValue<DateTime>(out var ngaySinh)
                                ? (DateOnly?)DateOnly.FromDateTime(ngaySinh) : null,
                            DiaChi = row.Cell(4).GetString(),
                            Cccd = row.Cell(5).GetString(),
                            Sdt = row.Cell(6).GetString(),
                            Email = row.Cell(7).GetString(),
                            NgayVaoLam = row.Cell(8).TryGetValue<DateTime>(out var ngayVaoLam)
                                ? DateOnly.FromDateTime(ngayVaoLam) : DateOnly.FromDateTime(DateTime.Now),
                            Luong = row.Cell(9).GetValue<int>(),
                            VaiTro = row.Cell(10).GetString(),
                            TenTaiKhoan = row.Cell(11).GetString(),
                            MatKhau = row.Cell(12).GetString(),
                            TinhTrang = row.Cell(13).GetString(),
                        };

                        importedNhanViens.Add(nhanVien);
                    }
                }

                _nhanVienService.ImportNhanViens(importedNhanViens);

                return Ok(new { success = true, message = "Nhập dữ liệu từ file Excel thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Lỗi khi nhập file Excel: " + ex.Message });
            }
        }


        [HttpGet("ExportExcel")]
        public IActionResult ExportExcel(string? keyword = null, string? sort = null, string? status = null, string? gender = null)
        {
            try
            {
                var nhanViens = _nhanVienService.GetAllNhanVien(keyword, sort, status, gender);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("DanhSachNhanVien");

                    worksheet.Cell(1, 1).Value = "Họ và Tên";
                    worksheet.Cell(1, 2).Value = "Giới Tính";
                    worksheet.Cell(1, 3).Value = "Ngày Sinh";
                    worksheet.Cell(1, 4).Value = "Địa Chỉ";
                    worksheet.Cell(1, 5).Value = "CCCD";
                    worksheet.Cell(1, 6).Value = "Số Điện Thoại";
                    worksheet.Cell(1, 7).Value = "Email";
                    worksheet.Cell(1, 8).Value = "Lương";
                    worksheet.Cell(1, 9).Value = "Vai Trò";

                    for (int i = 0; i < nhanViens.Count; i++)
                    {
                        var nv = nhanViens[i];
                        worksheet.Cell(i + 2, 1).Value = nv.HoTen;
                        worksheet.Cell(i + 2, 2).Value = nv.GioiTinh;
                        worksheet.Cell(i + 2, 3).Value = nv.NgaySinh?.ToString("dd/MM/yyyy");
                        worksheet.Cell(i + 2, 4).Value = nv.DiaChi;
                        worksheet.Cell(i + 2, 5).Value = nv.Cccd;
                        worksheet.Cell(i + 2, 6).Value = nv.Sdt;
                        worksheet.Cell(i + 2, 7).Value = nv.Email;
                        worksheet.Cell(i + 2, 8).Value = nv.Luong;
                        worksheet.Cell(i + 2, 9).Value = nv.VaiTro;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachNhanVien.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Lỗi khi xuất file Excel: " + ex.Message });
            }
        }


        [HttpGet("GetPaged")]
        public IActionResult GetPaged(int pageNumber, int pageSize, string? keyword, string? sort, string? status, string? gender)
        {
            var result = _nhanVienService.GetPagedNhanVien(pageNumber, pageSize, keyword, sort, status, gender);
            return Ok(result);
        }
    }
}
