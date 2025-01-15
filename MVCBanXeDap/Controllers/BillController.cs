using APIBanXeDap.ViewModels;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace MVCBanXeDap.Controllers
{
    public class BillController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        public BillController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            HoadonVM hoadonVM = new HoadonVM();
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(_client.BaseAddress + "bill/get");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                hoadonVM = JsonConvert.DeserializeObject<HoadonVM>(data);
            }
            return PartialView(hoadonVM);
        }
        [HttpPut]
        public async Task<IActionResult> Update(HoadonVM hoadonVM)
        {
            if (ModelState.IsValid)
            {
                var hoadonVM_Js = JsonConvert.SerializeObject(hoadonVM);
                StringContent content = new StringContent(hoadonVM_Js, Encoding.UTF8, "application/json");
                HttpResponseMessage http = await _client.PutAsync(_client.BaseAddress + "bill/update/", content);
                if (http.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(hoadonVM);
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatusOrder(string idOrder, string status, string? idStaffChanged)
        {
            string? idStaff = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (String.IsNullOrEmpty(idStaff))
            {
                return Json(new { success = false, message = "Không tìm thấy mã người dùng đăng nhập, vui lòng liên hệ nhà phát triển để được hỗ trợ." });
            }

            if (String.IsNullOrEmpty(idStaffChanged)) //Nếu chưa ai đổi tình trạng bill
            {
                var paramsChange = new Dictionary<string, string>
                {
                    { "idOrder", idOrder },
                    { "idStaff", idStaff },
                    { "status", status }
                };

                string queryString = string.Join("&", paramsChange.Select(x => $"{x.Key}={Uri.EscapeUriString(x.Value)}"));
                var content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage httpResponse = await _client.PutAsync(_client.BaseAddress + "bill/update/?" + queryString, content);

                if (httpResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Đã thay đổi tình trạng đơn hàng thành công." });
                }
                else
                {
                    return Json(new { success = false, message = "Đã thay đổi tình trạng đơn hàng thất bại." });
                }
            }
            if (idStaff != idStaffChanged) //Nếu người đã đổi tình trạng không phải người đang đổi
            {
                return Json(new { success = false, message = $"Đơn hàng đã được quản lý bởi nhân viên khác mang mã {idStaffChanged}, {idStaff}, {idOrder}." });
            }
            return Json(new { success = false, message = "Gặp vấn đề không xác định, vui lòng liên hệ nhà phát triển để được hỗ trợ." });
        }
        public async Task<IActionResult> TakeInvoice(string maHoaDon)
        {
            InvoiceVM invoice = null;

            // Gọi API để lấy thông tin hóa đơn
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"https://localhost:7137/api/Bill/GetInvoiceData/{maHoaDon}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                invoice = JsonConvert.DeserializeObject<InvoiceVM>(data);
            }

            if (invoice == null)
            {
                return NotFound("Hóa đơn không tồn tại.");
            }

            // Tạo PDF
            using (var memoryStream = new MemoryStream())
            {
                // Khởi tạo tài liệu PDF
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Sử dụng font hỗ trợ tiếng Việt
                var fontPath = Path.Combine("wwwroot", "fonts", "Merriweather-Regular.ttf");
                PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                document.SetFont(font);

                // Thêm tiêu đề hóa đơn
                document.Add(new Paragraph("HÓA ĐƠN BÁN HÀNG")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold());

                // Thông tin cửa hàng
                document.Add(new Paragraph("Cửa Hàng Xe Đạp Online")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetMarginBottom(20));
                document.Add(new Paragraph("Địa chỉ: Ngõ 6 Hà Duy Tập, Buôn Ma Thuột, Đắk Lắk")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12));
                document.Add(new Paragraph("Số điện thoại: (024) 7300 1955 - Website: Caodang@fpt.edu.vn")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetMarginBottom(20));

                // Thông tin khách hàng
                document.Add(new Paragraph($"Thông Tin Khách Hàng")
                    .SetFontSize(14)
                    .SetBold()
                    .SetMarginBottom(10));
                document.Add(new Paragraph($"Tên Người Nhận: {invoice.CustomerName}"));
                document.Add(new Paragraph($"Số Điện Thoại: {invoice.CustomerPhone}"));
                document.Add(new Paragraph($"Địa Chỉ: {invoice.CustomerAddress}"));
                document.Add(new Paragraph($"Ngày Đặt Hàng: {invoice.NgayTao:dd/MM/yyyy}")
                    .SetMarginBottom(20));

                // Bảng chi tiết hóa đơn
                var table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 2, 2, 2 }))
                    .UseAllAvailableWidth();
                table.AddHeaderCell(new Cell().Add(new Paragraph("Sản Phẩm").SetBold()));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Số Lượng").SetBold()));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Giá").SetBold()));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Tổng").SetBold()));

                double totalAmount = 0;
                foreach (var item in invoice.Items)
                {
                    double itemTotal = (double)item.Quantity * (double)item.UnitPrice;
                    totalAmount += itemTotal;

                    table.AddCell(new Cell().Add(new Paragraph(item.ProductName)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Quantity.ToString())).SetTextAlignment(TextAlignment.CENTER));
                    table.AddCell(new Cell().Add(new Paragraph($"{String.Format("{0:n0}", item.UnitPrice)} đ")));
                    table.AddCell(new Cell().Add(new Paragraph($"{String.Format("{0:n0}", itemTotal)} đ")));
                }

                document.Add(table.SetMarginBottom(20));

                // Tổng tiền và trạng thái thanh toán
                document.Add(new Paragraph($"Tổng Tiền: {String.Format("{0:n0}", totalAmount)} đ")
                    .SetBold()
                    .SetFontSize(14));
                document.Add(new Paragraph($"Trạng Thái Thanh Toán: {invoice.TinhTrang}")
                    .SetFontSize(12)
                    .SetMarginBottom(20));

                // Cảm ơn khách hàng
                document.Add(new Paragraph("Cảm ơn quý khách đã mua hàng!")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetItalic());

                // Kết thúc tài liệu
                document.Close();

                // Trả về file PDF
                byte[] pdfBytes = memoryStream.ToArray();
                return File(pdfBytes, "application/pdf", $"hoa_don_{maHoaDon}.pdf");
            }
        }

        #region//GET API
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateOnly? ngayTao,
            [FromQuery] string? httt,
            [FromQuery] string? tinhTrang)
        {
            List<HoadonVM> hoadonVMs = new List<HoadonVM>();
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(_client.BaseAddress + "bill/get");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                hoadonVMs = JsonConvert.DeserializeObject<List<HoadonVM>>(data);

                if (hoadonVMs.Count() != 0)
                {
                    if (ngayTao.HasValue)
                        hoadonVMs = hoadonVMs.Where(hd => hd.NgayTao == ngayTao.Value).ToList();
                    if (!string.IsNullOrEmpty(httt))
                        hoadonVMs = hoadonVMs.Where(hd => hd.Httt.Contains(httt)).ToList();
                    if (!string.IsNullOrEmpty(tinhTrang))
                        hoadonVMs = hoadonVMs.Where(hd => hd.TinhTrang.Contains(tinhTrang)).ToList();
                }
            }

            return Json(new { data = hoadonVMs });
        }

        #endregion
    }
}
