using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using MVCBanXeDap.ViewModels;
using ClosedXML.Excel;

namespace MVCBanXeDap.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress;

        public NhanVienController()
        {
            _baseAddress = new Uri("https://localhost:7137/api/");
            _client = new HttpClient
            {
                BaseAddress = _baseAddress
            };
        }

        // GET: NhanVien
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? keyword = null, string? sort = "asc")
        {
            try
            {
                var response = await _client.GetAsync($"NhanVien/GetPaged?pageNumber={pageNumber}&pageSize={pageSize}&keyword={keyword}&sort={sort}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var pagedResult = JsonConvert.DeserializeObject<PagedResult<NhanVienVM>>(data);

                    ViewBag.Page = pageNumber;
                    ViewBag.TotalPages = pagedResult.TotalPages;
                    ViewBag.Keyword = keyword;
                    ViewBag.Sort = sort;
                    ViewBag.PageSize = pageSize;

                    return View(pagedResult);
                }

                TempData["ErrorMessage"] = "Không thể tải dữ liệu nhân viên!";
                return View(new PagedResult<NhanVienVM>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
                return View(new PagedResult<NhanVienVM>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhanVienVM model, IFormFile? Anh)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(ms => ms.Value.Errors.Any())
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return Json(new { success = false, errors });
            }

            try
            {
                using var formData = new MultipartFormDataContent();

                formData.Add(new StringContent(model.HoTen ?? ""), "HoTen");
                formData.Add(new StringContent(model.Cccd ?? ""), "Cccd");
                formData.Add(new StringContent(model.Sdt ?? ""), "Sdt");
                formData.Add(new StringContent(model.Email ?? ""), "Email");
                formData.Add(new StringContent(model.VaiTro ?? ""), "VaiTro");
                formData.Add(new StringContent(model.Luong.ToString()), "Luong");
                formData.Add(new StringContent(model.TenTaiKhoan ?? ""), "TenTaiKhoan");
                formData.Add(new StringContent(model.MatKhau ?? ""), "MatKhau");
                formData.Add(new StringContent(model.VaiTro ?? ""), "VaiTro");
                formData.Add(new StringContent(model.GioiTinh ?? ""), "GioiTinh");

                if (Anh != null)
                {
                    var memoryStream = new MemoryStream();
                    await Anh.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var fileContent = new StreamContent(memoryStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Anh.ContentType);
                    formData.Add(fileContent, "Anh", Anh.FileName);
                }

                var response = await _client.PostAsync("NhanVien/Add", formData);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Thêm nhân viên thành công!" });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    var nhanVien = JsonConvert.DeserializeObject<ApiReponse<NhanVienVM>>(errorMessage);
                    return Json(new { success = false, message = nhanVien.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NhanVienVM model, IFormFile? Anh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using var formData = new MultipartFormDataContent();

                    formData.Add(new StringContent(model.HoTen ?? ""), "HoTen");
                    formData.Add(new StringContent(model.Cccd ?? ""), "Cccd");
                    formData.Add(new StringContent(model.Sdt ?? ""), "Sdt");
                    formData.Add(new StringContent(model.Email ?? ""), "Email");
                    formData.Add(new StringContent(model.VaiTro ?? ""), "VaiTro");
                    formData.Add(new StringContent(model.Luong.ToString()), "Luong");
                    formData.Add(new StringContent(model.TenTaiKhoan ?? ""), "TenTaiKhoan");
                    formData.Add(new StringContent(model.MatKhau ?? ""), "MatKhau");
                    formData.Add(new StringContent(model.VaiTro ?? ""), "VaiTro");
                    formData.Add(new StringContent(model.GioiTinh ?? ""), "GioiTinh");
                    formData.Add(new StringContent(model.TinhTrang.ToString()), "TinhTrang");
                    formData.Add(new StringContent(model.DiaChi ?? ""), "DiaChi");
                    formData.Add(new StringContent(model.NgaySinh.HasValue ? model.NgaySinh.Value.ToString("yyyy-MM-dd") : ""), "NgaySinh");



                    if (Anh != null)
                    {
                        var memoryStream = new MemoryStream();
                        await Anh.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileContent = new StreamContent(memoryStream);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Anh.ContentType);
                        formData.Add(fileContent, "Anh", Anh.FileName);
                    }

                    var response = await _client.PutAsync($"NhanVien/Update/{id}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Cập nhật nhân viên thành công!" });
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        var nhanVien = JsonConvert.DeserializeObject<ApiReponse<NhanVienVM>>(errorMessage);
                        return Json(new { success = false, message = nhanVien.Message });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Thông tin không hợp lệ!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetNhanVienById(int id)
        {
            try
            {
                var response = await _client.GetAsync($"NhanVien/GetNhanVienById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var nhanVien = JsonConvert.DeserializeObject<ApiReponse<NhanVienVM>>(data);
                    return Json(new { success = true, data = nhanVien.Data });
                }
                return Json(new { success = false, message = "Không tìm thấy nhân viên" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id, string status)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(new { status = status }), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"NhanVien/ToggleStatus/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleIsDelete(int id)
        {
            try
            {
                var response = await _client.PostAsync($"NhanVien/ToggleIsDelete/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Nhân viên đã được ẩn khỏi danh sách." });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        // Phương thức ExportToExcel bổ sung cột Lương
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                // Lấy danh sách nhân viên từ API
                var response = await _client.GetAsync("NhanVien/GetAll?isDelete=false"); // Chỉ lấy nhân viên IsDelete = false
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                var data = await response.Content.ReadAsStringAsync();
                var nhanViens = JsonConvert.DeserializeObject<List<NhanVienVM>>(data);

                // Tạo workbook Excel
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("DanhSachNhanVien");

                // Cấu hình font và cỡ chữ toàn bộ worksheet
                worksheet.Style.Font.FontName = "Times New Roman";
                worksheet.Style.Font.FontSize = 14;

                // Tiêu đề cột
                worksheet.Cell(1, 1).Value = "Họ và Tên";
                worksheet.Cell(1, 2).Value = "Giới Tính";
                worksheet.Cell(1, 3).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Value = "Địa Chỉ";
                worksheet.Cell(1, 5).Value = "CCCD";
                worksheet.Cell(1, 6).Value = "Số Điện Thoại";
                worksheet.Cell(1, 7).Value = "Email";
                worksheet.Cell(1, 8).Value = "Vai Trò";
                worksheet.Cell(1, 9).Value = "Lương";

                // Định dạng tiêu đề
                var titleRow = worksheet.Range("A1:I1");
                titleRow.Style.Font.Bold = true;
                titleRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                titleRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                titleRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // Đổ dữ liệu vào Excel
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
                    worksheet.Cell(i + 2, 8).Value = nv.VaiTro;
                    worksheet.Cell(i + 2, 9).Value = nv.Luong;
                }

                // Tạo bảng rõ ràng
                var dataRange = worksheet.Range(1, 1, nhanViens.Count + 1, 9);
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // Tự động điều chỉnh kích thước cột
                worksheet.Columns().AdjustToContents();

                // Lưu file Excel vào MemoryStream
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                // Trả về file Excel
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachNhanVien.xlsx");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xuất file Excel: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn tệp Excel hợp lệ!";
                return RedirectToAction("Index");
            }

            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    var fileStreamContent = new StreamContent(file.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileStreamContent, "file", file.FileName);

                    // Call API to process the file
                    var response = await _client.PostAsync("NhanVien/ImportExcel", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        var khachHang = JsonConvert.DeserializeObject<ApiReponse<KhachHangVM>>(errorMessage);
                        TempData["SuccessMessage"] = khachHang.Message;
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        var khachHang = JsonConvert.DeserializeObject<ApiReponse<KhachHangVM>>(errorMessage);
                        TempData["ErrorMessage"] = khachHang.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
            }

            return RedirectToAction("Index");
        }



    }
}
