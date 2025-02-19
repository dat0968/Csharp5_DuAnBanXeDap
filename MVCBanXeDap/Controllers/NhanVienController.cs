using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using MVCBanXeDap.ViewModels;
using ClosedXML.Excel;
using MVCBanXeDap.Services.Jwt;
using System.Net.Http.Headers;

namespace MVCBanXeDap.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress;
        private readonly IjwtToken jwtToken;
        public NhanVienController(IjwtToken jwtToken)
        {
            _baseAddress = new Uri("https://localhost:7137/api/");
            _client = new HttpClient
            {
                BaseAddress = _baseAddress
            };
            this.jwtToken = jwtToken;
        }
        [NonAction]
        [HttpGet]
        public async void SetAuthorizationHeader()
        {
            var validateAccessToken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                var accesstoken = validateAccessToken;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            }
        }
        public async Task<IActionResult> Index(
           int pageNumber = 1,
           int pageSize = 10,
           string? keyword = null,
           string? sort = "asc",
           string? status = null,
           string? gender = null)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _client.GetAsync(_client.BaseAddress + $"NhanVien/GetPaged?pageNumber={pageNumber}&pageSize={pageSize}&keyword={keyword}&sort={sort}&status={status}&gender={gender}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var pagedResult = JsonConvert.DeserializeObject<PagedResult<NhanVienVM>>(data);

                    ViewBag.Page = pageNumber;
                    ViewBag.TotalPages = pagedResult.TotalPages;
                    ViewBag.Keyword = keyword;
                    ViewBag.Sort = sort;
                    ViewBag.PageSize = pageSize;
                    ViewBag.Status = status;
                    ViewBag.Gender = gender;

                    return View(pagedResult);
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
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
                formData.Add(new StringContent(model.TinhTrang.ToString()), "TinhTrang");
                formData.Add(new StringContent(model.DiaChi ?? ""), "DiaChi");
                formData.Add(new StringContent(model.NgaySinh.HasValue ? model.NgaySinh.Value.ToString("yyyy-MM-dd") : ""), "NgaySinh");
                formData.Add(new StringContent(model.NgayVaoLam.HasValue ? model.NgayVaoLam.Value.ToString("yyyy-MM-dd") : ""), "NgayVaoLam");
                if (Anh != null)
                {
                    var memoryStream = new MemoryStream();
                    await Anh.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var fileContent = new StreamContent(memoryStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Anh.ContentType);
                    formData.Add(fileContent, "Anh", Anh.FileName);
                }
                SetAuthorizationHeader();
                var response = await _client.PostAsync(_client.BaseAddress + "NhanVien/Add", formData);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Thêm nhân viên thành công!" });
                }
                else
                {
                    int status = (int)response.StatusCode;
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    var nhanVien = JsonConvert.DeserializeObject<ApiReponse<NhanVienVM>>(errorMessage);
                    return Json(new { success = false, message = nhanVien.Message, status = $"{status}" });
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
                    formData.Add(new StringContent(model.NgayVaoLam.HasValue ? model.NgayVaoLam.Value.ToString("yyyy-MM-dd") : ""), "NgayBatDau");


                    if (Anh != null)
                    {
                        var memoryStream = new MemoryStream();
                        await Anh.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        var fileContent = new StreamContent(memoryStream);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Anh.ContentType);
                        formData.Add(fileContent, "Anh", Anh.FileName);
                    }
                    SetAuthorizationHeader();
                    var response = await _client.PutAsync(_client.BaseAddress + $"NhanVien/Update/{id}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Cập nhật nhân viên thành công!" });
                    }
                    else
                    {
                        int status = (int)response.StatusCode;
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        var nhanVien = JsonConvert.DeserializeObject<ApiReponse<NhanVienVM>>(errorMessage);
                        return Json(new { success = false, message = nhanVien.Message, status = $"{status}" });
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
                SetAuthorizationHeader();
                var response = await _client.GetAsync(_client.BaseAddress + $"NhanVien/GetNhanVienById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var nhanVien = JsonConvert.DeserializeObject<ApiReponse<NhanVienVM>>(data);
                    return Json(new { success = true, data = nhanVien.Data });
                }
                else
                {
                    int status = (int)response.StatusCode;
                    //return Json(new { success = false, message = $"{status}" });
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = "Không tìm thấy khách hàng", status = $"{status}" });
                }
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
                SetAuthorizationHeader();
                var jsonContent = new StringContent(JsonConvert.SerializeObject(new { status = status }), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_client.BaseAddress + $"NhanVien/ToggleStatus/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
                }
                else
                {
                    int Status = (int)response.StatusCode;
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = "API Error: " + errorMessage, status = $"{Status}" });
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
                SetAuthorizationHeader();
                var response = await _client.PutAsync(_client.BaseAddress + $"NhanVien/ToggleIsDelete/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Nhân viên đã được ẩn khỏi danh sách." });
                }
                else
                {
                    int Status = (int)response.StatusCode;
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = "API Error: " + errorMessage, status = $"{Status}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        // Phương thức ExportToExcel bổ sung cột Lương
        public async Task<IActionResult> ExportToExcel(string? keyword = null, string? sort = null, string? status = null, string? gender = null)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await _client.GetAsync(_client.BaseAddress + $"NhanVien/GetAll?keyword={keyword}&sort={sort}&status={status}&gender={gender}");
                if (!response.IsSuccessStatusCode)
                {
                    int Status = (int)response.StatusCode;
                    return RedirectToAction($"/Home/Error/{Status}");
                }

                var data = await response.Content.ReadAsStringAsync();
                var nhanViens = JsonConvert.DeserializeObject<List<NhanVienVM>>(data);

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("DanhSachNhanVien");

                worksheet.Cell(1, 1).Value = "Họ và Tên";
                worksheet.Cell(1, 2).Value = "Giới Tính";
                worksheet.Cell(1, 3).Value = "Ngày Sinh";
                worksheet.Cell(1, 4).Value = "Địa Chỉ";
                worksheet.Cell(1, 5).Value = "CCCD";
                worksheet.Cell(1, 6).Value = "Số Điện Thoại";
                worksheet.Cell(1, 7).Value = "Email";
                worksheet.Cell(1, 8).Value = "Vai Trò";
                worksheet.Cell(1, 9).Value = "Lương";

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

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachNhanVien.xlsx");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xuất file Excel: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Import_Excel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn tệp Excel hợp lệ!";
                return RedirectToAction("Index");
            }

            try
            {
                SetAuthorizationHeader();
                using (var content = new MultipartFormDataContent())
                {
                    var fileStreamContent = new StreamContent(file.OpenReadStream());
                    fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileStreamContent, "file", file.FileName);

                    // Call API to process the file
                    var response = await _client.PostAsync(_client.BaseAddress + "NhanVien/ImportExcel", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        var khachHang = JsonConvert.DeserializeObject<ApiReponse<KhachHangVM>>(errorMessage);
                        TempData["SuccessMessage"] = khachHang.Message;
                    }
                    else
                    {
                        int Status = (int)response.StatusCode;
                        return RedirectToAction($"/Home/Error/{Status}");
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
