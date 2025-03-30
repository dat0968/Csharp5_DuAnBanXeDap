using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MVCBanXeDap.Controllers
{
    public class BillController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly HttpClient _client;
        private readonly IjwtToken jwtToken;

        public BillController(IjwtToken jwtToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            this.jwtToken = jwtToken;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChangeStatusOrder(string idOrder, string status, string? reason)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (accessToken == null || refreshToken == null)
            {
                return Json(new { success = false, message = "Phiên của bạn đã hết, vui lòng đăng nhập lại.", isLoginAgain = true });
            }

            var ValidateAccessToken = await jwtToken.ValidateAccessToken();
            if (ValidateAccessToken == null)
            {
                return Json(new { success = false, message = "Phiên của bạn đã hết, vui lòng đăng nhập lại.", isLoginAgain = true });
            }
            else
            {
                HttpContext.Session.SetString("AccessToken", ValidateAccessToken);
            }

            var information = jwtToken.GetInformationUserFromToken(ValidateAccessToken); // Lấy idStaff từ token
            var id = information.Id.ToString();
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "Không tìm thấy mã người dùng đăng nhập, vui lòng liên hệ nhà phát triển để được hỗ trợ." });
            }

            var paramsChange = new Dictionary<string, string?>
            {
                { "idOrder", idOrder },
                { "idStaff", id },
                { "statusOrder", status },
                { "reason", reason }
            };

            // Kiểm tra, cập nhật và đảm bảo "reason" không bị null
            paramsChange["reason"] = paramsChange["reason"] != null
                ? "Đơn hàng bị hủy bởi nhân viên với lý do:\n" + paramsChange["reason"]
                : null;

            // Tạo queryString và xử lý lọc null hoặc chuỗi rỗng đảm bảo an toàn
            string queryString = string.Join("&",
                paramsChange
                    .Where(x => !string.IsNullOrWhiteSpace(x.Value)) // Lọc bỏ giá trị null hoặc chuỗi rỗng
                    .Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value!)}")); // Mã hóa giá trị để gửi trong URL

            Console.WriteLine(queryString);
            SetAuthorizationHeader();
            //var content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage httpResponse = await _client.PutAsync(_client.BaseAddress + "bill/ChangeStatusOrder?" + queryString, null);

            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                return Json(jsonResponse);
            }
            else
            {
                //string jsonError = await httpResponse.Content.ReadAsStringAsync();
                //return Json(jsonError);
                return StatusCode((int)httpResponse.StatusCode);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string idOrder)
        {
            InvoiceVM invoice = null;
            SetAuthorizationHeader();
            // Gọi API để lấy thông tin hóa đơn
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(_client.BaseAddress + $"Bill/GetInvoiceData/{idOrder}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                invoice = JsonConvert.DeserializeObject<InvoiceVM>(data);
            }
            else
            {
                return StatusCode((int)httpResponseMessage.StatusCode);
            }

            if (invoice == null)
            {
                return Json(new { success = false, message = "Không tìm thấy hóa đơn" });
            }
            return PartialView(invoice);
        }
        public async Task<IActionResult> TakeInvoice(string maHoaDon)
        {
            InvoiceVM invoice = null;
            SetAuthorizationHeader();
            // Gọi API để lấy thông tin hóa đơn
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(_client.BaseAddress + $"Bill/GetInvoiceData/{maHoaDon}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                invoice = JsonConvert.DeserializeObject<InvoiceVM>(data);
            }
            else
            {
                return StatusCode((int)httpResponseMessage.StatusCode);
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
                document.Add(new Paragraph($"Tên Người Nhận: {invoice.TenKhachHang}"));
                document.Add(new Paragraph($"Số Điện Thoại: {invoice.SoDienThoaiKhachHang}"));
                document.Add(new Paragraph($"Địa Chỉ: {invoice.DiaChiKhachHang}"));
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
                    double itemTotal = (double)item.SoLuong * (double)item.DonGia;
                    totalAmount += itemTotal;

                    table.AddCell(new Cell().Add(new Paragraph(item.TenSanPham)));
                    table.AddCell(new Cell().Add(new Paragraph(item.SoLuong.ToString())).SetTextAlignment(TextAlignment.CENTER));
                    table.AddCell(new Cell().Add(new Paragraph($"{String.Format("{0:n0}", item.DonGia)} VNĐ")).SetTextAlignment(TextAlignment.RIGHT));
                    table.AddCell(new Cell().Add(new Paragraph($"{String.Format("{0:n0}", itemTotal)} VNĐ"))).SetTextAlignment(TextAlignment.RIGHT);
                }

                document.Add(table.SetMarginBottom(20));

                // Thêm thông tin Tiền Gốc
                document.Add(new Paragraph($"Tiền Gốc: {String.Format("{0:n0}", invoice.TienGoc)} VNĐ")
                    .SetFontSize(12)
                    .SetMarginBottom(20));

                // Thêm thông tin Phí Vận Chuyển
                document.Add(new Paragraph($"Phí Vận Chuyển: {String.Format("{0:n0}", invoice.PhiVanChuyen)} VNĐ")
                    .SetFontSize(12)
                    .SetMarginBottom(10));

                // Thêm thông tin Giảm Giá Mã Coupon
                document.Add(new Paragraph($"Giảm Giá Mã Coupon: {String.Format("{0:n0}", invoice.GiamGiaMaCoupon)} VNĐ")
                    .SetFontSize(12)
                    .SetMarginBottom(10));

                // Thêm thông tin Tổng Tiền
                document.Add(new Paragraph($"Tổng Tiền: {String.Format("{0:n0}", invoice.TongTien)} VNĐ")
                    .SetBold()
                    .SetFontSize(14));

                // Thêm thông tin Trạng Thái Thanh Toán
                document.Add(new Paragraph($"Trạng Thái Thanh Toán: {invoice.TinhTrang}")
                    .SetFontSize(12)
                    .SetMarginBottom(20));

                // Thêm lý do hủy nếu có
                if (!string.IsNullOrEmpty(invoice.LyDoHuy))
                {
                    document.Add(new Paragraph($"Lý Do Hủy Đơn Hàng:")
                        .SetFontSize(12)
                        .SetBold()
                        .SetMarginBottom(5));

                    document.Add(new Paragraph(invoice.LyDoHuy)
                        .SetFontSize(12)
                        .SetItalic()
                        .SetMarginBottom(20));
                }

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
        public async Task<IActionResult> ExportAllInvoicesAsSinglePDF()
        {
            // Lấy danh sách hóa đơn từ API
            List<InvoiceVM> invoices = new List<InvoiceVM>();
            SetAuthorizationHeader();
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(_client.BaseAddress + $"Bill/GetAllInvoiceData/");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                invoices = JsonConvert.DeserializeObject<List<InvoiceVM>>(data);
            }
            else
            {
                return StatusCode((int)httpResponseMessage.StatusCode);
            }
            // Kiểm tra nếu không có hóa đơn
            if (invoices == null || !invoices.Any())
            {
                return NotFound("Không có hóa đơn nào để xuất.");
            }

            // File PDF hợp nhất
            using var outputStream = new MemoryStream();
            using var pdfWriter = new PdfWriter(outputStream);
            using var pdfDocument = new PdfDocument(pdfWriter);
            var document = new Document(pdfDocument);

            // Đặt font chữ
            var fontPath = Path.Combine("wwwroot", "fonts", "Merriweather-Regular.ttf");
            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
            document.SetFont(font);

            foreach (var invoice in invoices)
            {
                document.Add(new Paragraph("HÓA ĐƠN BÁN HÀNG")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold());

                document.Add(new Paragraph($"Mã hóa đơn: {invoice.MaHoaDon}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(14)
                    .SetBold()
                    .SetMarginBottom(10));

                // Thông tin khách hàng
                document.Add(new Paragraph($"Tên: {invoice.TenKhachHang}")
                    .SetTextAlignment(TextAlignment.LEFT));
                document.Add(new Paragraph($"Số điện thoại: {invoice.SoDienThoaiKhachHang}")
                    .SetTextAlignment(TextAlignment.LEFT));
                document.Add(new Paragraph($"Ngày hóa đơn: {invoice.NgayTao:dd/MM/yyyy}")
                    .SetTextAlignment(TextAlignment.LEFT));
                document.Add(new Paragraph($"Địa chỉ: {invoice.DiaChiKhachHang}")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetMarginBottom(20));

                // Bảng sản phẩm
                var table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 1, 2, 2 })).UseAllAvailableWidth();
                table.AddHeaderCell(new Cell().Add(new Paragraph("Tên sản phẩm").SetBold()));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Số lượng").SetBold()));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Đơn giá").SetBold()));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Thành tiền").SetBold()));

                // Nội dung các dòng trong bảng
                double totalAmount = 0;
                foreach (var item in invoice.Items)
                {
                    double subTotal = (double)(item.SoLuong * item.DonGia);
                    totalAmount += subTotal;

                    table.AddCell(new Paragraph(item.TenSanPham));
                    table.AddCell(new Paragraph(item.SoLuong.ToString()).SetTextAlignment(TextAlignment.CENTER));
                    table.AddCell(new Paragraph($"{item.DonGia:n0} VNĐ").SetTextAlignment(TextAlignment.RIGHT));
                    table.AddCell(new Paragraph($"{subTotal:n0} VNĐ").SetTextAlignment(TextAlignment.RIGHT));
                }

                document.Add(table.SetMarginBottom(20));

                // Trạng Thái Thanh Toán
                document.Add(new Paragraph($"Trạng Thái Thanh Toán: {invoice.TinhTrang}")
                    .SetFontSize(12)
                    .SetMarginBottom(20));

                // Thêm lý do hủy nếu có
                if (!string.IsNullOrEmpty(invoice.LyDoHuy))
                {
                    document.Add(new Paragraph($"Lý Do Hủy Đơn Hàng:")
                        .SetFontSize(12)
                        .SetBold()
                        .SetMarginBottom(5));

                    document.Add(new Paragraph(invoice.LyDoHuy)
                        .SetFontSize(12)
                        .SetItalic()
                        .SetMarginBottom(20));
                }

                // Tổng tiền, hiển thị các thông tin đã xử lý sẵn
                document.Add(new Paragraph($"Tiền Gốc: {invoice.TienGoc:n0} VNĐ")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Phí Vận Chuyển: {invoice.PhiVanChuyen:n0} VNĐ")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Giảm Giá Mã Coupon: {invoice.GiamGiaMaCoupon:n0} VNĐ")
                    .SetFontSize(12));
                document.Add(new Paragraph($"Tổng tiền: {invoice.TongTien:n0} VNĐ")
                    .SetBold()
                    .SetFontSize(14)
                    .SetMarginBottom(20));

                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            }


            // Tải xuống file PDF tại đây
            document.Close();
            //outputStream.Seek(0, SeekOrigin.Begin);

            return File(outputStream.ToArray(), "application/pdf", "Tổng_hợp_hóa_đơn_LightTeam.pdf");
        }
        #region//GET API
        [HttpGet]
        public async Task<IActionResult> GetAll(
                [FromQuery] DateOnly? ngayTao,  
                [FromQuery] DateOnly? thoiGianGiao,
                [FromQuery] string? httt,
                [FromQuery] string? tinhTrang,
                [FromQuery] bool? takeYourOrders)
        {
            List<HoadonVM> hoadonVMs = new List<HoadonVM>();
            SetAuthorizationHeader();
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(_client.BaseAddress + "bill/get");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string data = await httpResponseMessage.Content.ReadAsStringAsync();
                hoadonVMs = JsonConvert.DeserializeObject<List<HoadonVM>>(data);

                if (hoadonVMs.Count() != 0)
                {
                    if (ngayTao.HasValue)
                        hoadonVMs = hoadonVMs.Where(hd => hd.NgayTao == ngayTao.Value).ToList();
                    if (thoiGianGiao.HasValue)
                        hoadonVMs = hoadonVMs.Where(hd => hd.ThoiGianGiao == thoiGianGiao.Value).ToList();
                    if (!string.IsNullOrEmpty(httt))
                        hoadonVMs = hoadonVMs.Where(hd => hd.Httt.Contains(httt)).ToList();
                    if (!string.IsNullOrEmpty(tinhTrang))
                        hoadonVMs = hoadonVMs.Where(hd => hd.TinhTrang.Contains(tinhTrang)).ToList();
                    if (takeYourOrders.HasValue && takeYourOrders.Value) {
                        int? userId = await GetUserId();
                        hoadonVMs = hoadonVMs.Where(hd => hd.MaNv == userId).ToList();
                    }
                }
            }
            else
            {
                return StatusCode((int)httpResponseMessage.StatusCode);
            }

            return Json(new { data = hoadonVMs });
        }

        #endregion

        #region [NON ACTION]
        [NonAction]
        [HttpGet]
        private async void SetAuthorizationHeader()
        {
            var validateAccessToken = await jwtToken.ValidateAccessToken();
            if (!string.IsNullOrEmpty(validateAccessToken))
            {
                var accesstoken = validateAccessToken;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            }
        }
        [NonAction]
        private async Task<int?> GetUserId()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (accessToken == null || refreshToken == null)
            {
                return null;
            }

            var ValidateAccessToken = await jwtToken.ValidateAccessToken();
            if (ValidateAccessToken == null)
            {
                return null!;
            }
            else
            {
                HttpContext.Session.SetString("AccessToken", ValidateAccessToken);
            }

            var information = jwtToken.GetInformationUserFromToken(ValidateAccessToken); // Lấy idStaff từ token
            var id = information.Id.ToString();

            return Convert.ToInt32(id);
        }
        #endregion
    }
}

