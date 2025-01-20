﻿using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using MVCBanXeDap.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using ClosedXML.Excel;

namespace MVCBanXeDap.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress;

        public KhachHangController()
        {
            _baseAddress = new Uri("https://localhost:7137/api/");
            _client = new HttpClient
            {
                BaseAddress = _baseAddress
            };
        }

        // GET: KhachHang
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? keyword = null, string? sort = "asc")
        {
            try
            {
                var response = await _client.GetAsync($"KhachHang/GetPaged?pageNumber={pageNumber}&pageSize={pageSize}&keyword={keyword}&sort={sort}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var pagedResult = JsonConvert.DeserializeObject<PagedResult<KhachHangVM>>(data);

                    ViewBag.Page = pageNumber;
                    ViewBag.TotalPages = pagedResult.TotalPages;
                    ViewBag.Keyword = keyword;
                    ViewBag.Sort = sort;
                    ViewBag.PageSize = pageSize;

                    return View(pagedResult);
                }

                TempData["ErrorMessage"] = "Không thể tải dữ liệu khách hàng!";
                return View(new PagedResult<KhachHangVM>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi hệ thống: {ex.Message}";
                return View(new PagedResult<KhachHangVM>());
            }
        }


        // POST: KhachHang/Create
        [HttpPost]
        public async Task<IActionResult> Create(KhachHangVM model, IFormFile? Anh)
        {
            if (!ModelState.IsValid)
            {
                // Lấy danh sách lỗi từ ModelState
                var errors = ModelState.Where(ms => ms.Value.Errors.Any())
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                return Json(new { success = false, errors });
            }

            try
            {
                using var formData = new MultipartFormDataContent();

                // Thêm các thuộc tính của model vào form-data
                formData.Add(new StringContent(model.HoTen ?? ""), "HoTen");
                formData.Add(new StringContent(model.Cccd ?? ""), "Cccd");
                formData.Add(new StringContent(model.Sdt ?? ""), "Sdt");
                formData.Add(new StringContent(model.Email ?? ""), "Email");
                formData.Add(new StringContent(model.TenTaiKhoan ?? ""), "TenTaiKhoan");
                formData.Add(new StringContent(model.MatKhau ?? ""), "MatKhau");

                // Xử lý file nếu có
                if (Anh != null)
                {
                    var memoryStream = new MemoryStream();
                    await Anh.CopyToAsync(memoryStream); // Copy dữ liệu file vào memory stream
                    memoryStream.Seek(0, SeekOrigin.Begin); // Đặt vị trí đọc lại từ đầu

                    var fileContent = new StreamContent(memoryStream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Anh.ContentType);
                    formData.Add(fileContent, "Anh", Anh.FileName);
                }

                // Gửi yêu cầu POST với form-data
                var response = await _client.PostAsync("KhachHang/Add", formData);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Thêm khách hàng thành công!" });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    var khachHang = JsonConvert.DeserializeObject<ApiReponse<KhachHangVM>>(errorMessage);
                    return Json(new { success = false, message = khachHang.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }



        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] KhachHangVM model, IFormFile? Anh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using var formData = new MultipartFormDataContent();

                    // Thêm các thuộc tính của model vào form-data
                    formData.Add(new StringContent(model.HoTen ?? ""), "HoTen");
                    formData.Add(new StringContent(model.Cccd ?? ""), "Cccd");
                    formData.Add(new StringContent(model.Sdt ?? ""), "Sdt");
                    formData.Add(new StringContent(model.Email ?? ""), "Email");
                    formData.Add(new StringContent(model.TenTaiKhoan ?? ""), "TenTaiKhoan");
                    formData.Add(new StringContent(model.MatKhau ?? ""), "MatKhau");
                    formData.Add(new StringContent(model.GioiTinh ?? ""), "GioiTinh");
                    formData.Add(new StringContent(model.NgaySinh?.ToString("MM-dd-yyyy") ?? ""), "NgaySinh");
                    formData.Add(new StringContent(model.DiaChi ?? ""), "DiaChi");
                    formData.Add(new StringContent(model.TinhTrang.ToString()), "TinhTrang");


                    // Xử lý file nếu có
                    if (Anh != null)
                    {
                        var memoryStream = new MemoryStream();
                        await Anh.CopyToAsync(memoryStream); // Copy dữ liệu file vào memory stream
                        memoryStream.Seek(0, SeekOrigin.Begin); // Đặt vị trí đọc lại từ đầu

                        var fileContent = new StreamContent(memoryStream);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Anh.ContentType);
                        formData.Add(fileContent, "Anh", Anh.FileName); // Thêm file vào form-data
                    }

                    // Gửi yêu cầu PUT với form-data
                    var response = await _client.PutAsync($"KhachHang/Update/{id}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Cập nhật khách hàng thành công!" });
                    }
                    else
                    {

                        var errorMessage = await response.Content.ReadAsStringAsync();
                        var khachHang = JsonConvert.DeserializeObject<ApiReponse<KhachHangVM>>(errorMessage);
                        return Json(new { success = false, message = khachHang.Message });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Thông tin không hợp lệ!" });
        }



        // GET: KhachHang/GetKhachHangById/{id}
        [HttpGet]
        public async Task<IActionResult> GetKhachHangById(int id)
        {
            try
            {
                var response = await _client.GetAsync($"KhachHang/GetKhachHangById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var khachHang = JsonConvert.DeserializeObject<ApiReponse<KhachHangVM>>(data);
                    return Json(new { success = true, data = khachHang.Data });
                }
                return Json(new { success = false, message = "Không tìm thấy khách hàng" });
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
                // Tạo nội dung JSON để gửi
                var jsonContent = new StringContent(JsonConvert.SerializeObject(new { status = status }), Encoding.UTF8, "application/json");

                // Gửi yêu cầu tới API backend
                var response = await _client.PostAsync($"KhachHang/ToggleStatus/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = "API Error: " + errorMessage });
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
                // Gửi yêu cầu POST tới API backend
                var response = await _client.PostAsync($"KhachHang/ToggleIsDelete/{id}", null);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Khách hàng đã được ẩn khỏi danh sách." });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = "API Error: " + errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                // Lấy danh sách khách hàng từ API
                var response = await _client.GetAsync("KhachHang/GetAll?isDelete=false"); // Chỉ lấy khách hàng IsDelete = false
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                var data = await response.Content.ReadAsStringAsync();
                var khachHangs = JsonConvert.DeserializeObject<List<KhachHangVM>>(data);

                // Tạo workbook Excel
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("DanhSachKhachHang");

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

                // Định dạng tiêu đề
                var titleRow = worksheet.Range("A1:G1");
                titleRow.Style.Font.Bold = true;
                titleRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                titleRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                titleRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // Đổ dữ liệu vào Excel
                for (int i = 0; i < khachHangs.Count; i++)
                {
                    var kh = khachHangs[i];
                    worksheet.Cell(i + 2, 1).Value = kh.HoTen;
                    worksheet.Cell(i + 2, 2).Value = kh.GioiTinh;
                    worksheet.Cell(i + 2, 3).Value = kh.NgaySinh?.ToString("dd/MM/yyyy");
                    worksheet.Cell(i + 2, 4).Value = kh.DiaChi;
                    worksheet.Cell(i + 2, 5).Value = kh.Cccd;
                    worksheet.Cell(i + 2, 6).Value = kh.Sdt;
                    worksheet.Cell(i + 2, 7).Value = kh.Email;
                }

                // Tạo bảng rõ ràng
                var dataRange = worksheet.Range(1, 1, khachHangs.Count + 1, 7);
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // Tự động điều chỉnh kích thước cột
                worksheet.Columns().AdjustToContents();

                // Lưu file Excel vào MemoryStream
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                // Trả về file Excel
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachKhachHang.xlsx");
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
                    var response = await _client.PostAsync("KhachHang/ImportExcel", content);

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
