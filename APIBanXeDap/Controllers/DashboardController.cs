using APIBanXeDap.Extensions;
using APIBanXeDap.Models.ViewModels;
using APIBanXeDap.Repository.ChiTietHoaDon;
using APIBanXeDap.Repository.HoaDon;
using APIBanXeDap.Repository.SanPham;
using APIBanXeDap.Repository.ThongKe;
using APIBanXeDap.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IHoaDonRepository _hd;
        private readonly IChiTietHoaDonRepository _cthd;
        private readonly IThongKeRepository _tk;
        private readonly IProductRepository _product;
        public DashboardController(IHoaDonRepository hd, IChiTietHoaDonRepository cthd, IThongKeRepository tk, IProductRepository product)
        {
            _hd = hd;
            _cthd = cthd;
            _tk = tk;
            _product = product;
        }
        [HttpGet]
        public IActionResult IsAuth()
        {
            return Ok();
        }

        /// <summary>
        /// Dữ liệu thống kê tổng tiền đơn hàng đã xác nhận theo thời gian
        /// </summary>
        /// <param name="timeRange">Nhập thời gian day/week/month/year</param>
        /// <returns></returns>
        [HttpGet("{timeRange}")]
        public async Task<IActionResult> GetEarningData(string timeRange)
        {
            var listOrders = (await _hd.GetAllInvoiceDataAsync())
                .Where(x => x.TinhTrang == "Đã xác nhận");  // So sánh trực tiếp với chuỗi "Đã xác nhận"

            var columnData = new List<int>();
            var categories = new List<string>();

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            switch (timeRange)
            {
                case "day":
                    var daysOfWeek = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
                    columnData = daysOfWeek.Select(day =>
                        listOrders
                            .Where(x => x.NgayTao.DayOfWeek.ToString().StartsWith(day.Substring(0, 3)))
                            .Sum(x => Convert.ToInt32(x.Items.Sum(it => it.Tong)))
                    ).ToList();
                    categories = daysOfWeek.ToList();
                    break;

                case "week":
                    var weeksInMonth = Enumerable.Range(1, 4);
                    columnData = weeksInMonth.Select(week =>
                        listOrders
                            .Where(x => x.NgayTao.Month == today.Month)
                            .Where(x => (x.NgayTao.Day - 1) / 7 + 1 == week)
                            .Sum(x => Convert.ToInt32(x.Items.Sum(it => it.Tong)))
                    ).ToList();
                    categories = weeksInMonth.Select(week => "Week " + week).ToList();
                    break;

                case "month":
                    int currentYear = today.Year;
                    columnData = Enumerable.Range(1, 12).Select(month =>
                        listOrders
                            .Where(x => x.NgayTao.Month == month && x.NgayTao.Year == currentYear)
                            .Sum(x => Convert.ToInt32(x.Items.Sum(it => it.Tong)))
                    ).ToList();
                    categories = Enumerable.Range(1, 12).Select(month => "Month " + month).ToList();
                    break;

                case "year":
                    currentYear = today.Year;
                    var recentYears = Enumerable.Range(currentYear - 5, 5);
                    columnData = recentYears.Select(year =>
                        listOrders
                            .Where(x => x.NgayTao.Year == year)
                            .Sum(x => Convert.ToInt32(x.Items.Sum(it => it.Tong)))
                    ).ToList();
                    categories = recentYears.Select(year => year.ToString()).ToList();
                    break;

                default:
                    return BadRequest("Invalid time range specified.");
            }

            return Ok(new
            {
                name = "Đơn hàng",
                data = columnData,
                categories = categories
            });
        }


        /// <summary>
        /// Dữ liệu thống kê theo tổng các đơn hàng chờ xác nhận và đã nhận
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOrderData()
        {
            var listOrders = (await _hd.GetAllHoadonVMAsync());

            // So sánh trực tiếp với chuỗi "Đã xác nhận" và "Chờ thanh toán"
            int statusApproved = listOrders.Count(lo => lo.TinhTrang == "Đã xác nhận");
            int statusPending = listOrders.Count(lo => lo.TinhTrang == "Chờ thanh toán");
            int statusInAllProgress = listOrders.Count() - statusApproved - statusPending;

            return Ok(new
            {
                approved = statusApproved,
                pending = statusPending,
                inAllProgress = statusInAllProgress
            });
        }

        /// <summary>
        /// Dữ liệu thống kê đơn hàng theo trạng thái
        /// </summary>
        /// <param name="timeRange">Nhập thời gian day/week/month/year</param>
        /// <returns></returns>
        [HttpGet("{timeRange}")]
        public async Task<IActionResult> GetOrderStatusData(string timeRange)
        {
            var listOrders = (await _hd.GetAllHoadonVMAsync())
                .Where(x => x.ThoiGianGiao != default(DateOnly));

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // Lọc đơn hàng theo khoảng thời gian
            switch (timeRange)
            {
                case "day":
                    listOrders = listOrders.Where(x => x.ThoiGianGiao == today);
                    break;
                case "week":
                    var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                    listOrders = listOrders.Where(x => x.ThoiGianGiao >= startOfWeek && x.ThoiGianGiao <= startOfWeek.AddDays(6));
                    break;
                case "month":
                    listOrders = listOrders.Where(x => x.ThoiGianGiao.Month == today.Month && x.ThoiGianGiao.Year == today.Year);
                    break;
                case "year":
                    listOrders = listOrders.Where(x => x.ThoiGianGiao.Year == today.Year);
                    break;
                default:
                    return BadRequest("Invalid time range specified.");
            }

            // Đếm số lượng đơn hàng theo trạng thái
            var orderStatuses = new Dictionary<string, int>
            {
                { "Đã xác nhận", 0 },
                { "Đã giao cho đơn vị vận chuyển", 0 },
                { "Đang giao hàng", 0 },
                { "Chờ thanh toán", 0 },
                { "Hoàn trả/Hoàn tiền", 0 },
                { "Đã hủy", 0 },
                { "Chờ xác nhận", 0 }
            };

            foreach (var order in listOrders)
            {
                if (orderStatuses.ContainsKey(order.TinhTrang))
                {
                    orderStatuses[order.TinhTrang]++;
                }
            }

            return Ok(new
            {
                labels = orderStatuses.Keys.ToList(),
                data = orderStatuses.Values.ToList()
            });
        }
        /// <summary>
        /// Dữ liệu thống kê đơn hàng theo thời gian, cung cấp cho dạng column + sparkline
        /// </summary>
        /// <param name="timeRange">Nhập thời gian day/week/month/year</param>
        /// <returns></returns>
        [HttpGet("{timeRange}")]
        public async Task<IActionResult> GetOrderOverViewData(string timeRange)
        {
            var listOrders = (await _hd.GetAllInvoiceDataAsync())
                .Where(x => x.TinhTrang != null).ToList();

            var orderStatuses = new[] { "Đã xác nhận", "Đã giao cho đơn vị vận chuyển", "Đang giao hàng", "Chờ thanh toán", "Hoàn trả/Hoàn tiền", "Đã hủy", "Chờ xác nhận" };

            var statusData = new Dictionary<string, List<(int Count, decimal Revenue)>>();
            var categories = new List<string>();

            int amountOrders = 0;

            switch (timeRange)
            {
                case "day":
                    var daysOfWeek = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
                    foreach (var status in orderStatuses)
                    {
                        statusData[status] = daysOfWeek.Select(day =>
                        {
                            var filterOrder = listOrders
                                .Where(x => x.TinhTrang == status && x.ThoiGianGiao.DayOfWeek.ToString().StartsWith(day.Substring(0, 3)));
                            amountOrders += filterOrder.Count();
                            return (filterOrder.Count(), filterOrder.Sum(x => x.Items.Sum(it => it.SoLuong * it.DonGia)));
                        }).ToList();
                    }
                    categories = daysOfWeek.ToList();
                    break;

                case "week":
                    var weeks = Enumerable.Range(1, 7).ToList(); // 7 ngày trong tuần
                    foreach (var status in orderStatuses)
                    {
                        statusData[status] = weeks.Select(week =>
                        {
                            var tempDate = DateTime.Now.StartOfWeek(DayOfWeek.Sunday).AddDays(week - 1);
                            var startDate = tempDate;
                            var endDate = tempDate.AddDays(1);

                            var filterOrder = listOrders
                                .Where(x => x.TinhTrang == status && x.ThoiGianGiao >= startDate && x.ThoiGianGiao < endDate);
                            amountOrders += filterOrder.Count();
                            return (filterOrder.Count(), filterOrder.Sum(x => x.Items.Sum(it => it.SoLuong * it.DonGia)));
                        }).ToList();
                    }
                    categories = weeks.Select(x => $"Thứ {x}").ToList(); // Tên cho các ngày thứ
                    break;

                case "month":
                    var months = Enumerable.Range(1, 12).ToList(); // Các tháng trong năm
                    foreach (var status in orderStatuses)
                    {
                        statusData[status] = months.Select(month =>
                        {
                            var tempDate = new DateTime(DateTime.Now.Year, month, 1);
                            var startDate = tempDate;
                            var endDate = tempDate.AddMonths(1);

                            var filterOrder = listOrders
                                .Where(x => x.TinhTrang == status && x.ThoiGianGiao >= startDate && x.ThoiGianGiao < endDate);
                            amountOrders += filterOrder.Count();
                            return (filterOrder.Count(), filterOrder.Sum(x => x.Items.Sum(it => it.SoLuong * it.DonGia)));
                        }).ToList();
                    }
                    categories = months.Select(x => $"{x} tháng").ToList();
                    break;

                case "year":
                    var years = Enumerable.Range(DateTime.Now.Year - 5, 6).ToList(); // Thời gian từ 5 năm trở đi
                    foreach (var status in orderStatuses)
                    {
                        statusData[status] = years.Select(year =>
                        {
                            var tempDate = new DateTime(year, 1, 1);
                            var startDate = tempDate;
                            var endDate = tempDate.AddYears(1);

                            var filterOrder = listOrders
                                .Where(x => x.TinhTrang == status && x.ThoiGianGiao >= startDate && x.ThoiGianGiao < endDate);
                            amountOrders += filterOrder.Count();
                            return (filterOrder.Count(), filterOrder.Sum(x => x.Items.Sum(it => it.SoLuong * it.DonGia)));
                        }).ToList();
                    }
                    categories = years.Select(x => $"{x}").ToList();
                    break;

                default:
                    foreach (var status in orderStatuses)
                    {
                        statusData[status] = new List<(int Count, decimal Revenue)> { (0, 0m) };
                    }
                    categories = new List<string> { "Unknown" };
                    break;
            }

            var result = orderStatuses.Select(status => new
            {
                name = status,
                data = statusData[status].Select(d => new { count = d.Count, revenue = d.Revenue }).ToList()
            }).ToList();

            return Ok(new
            {
                data = result, // Kết quả dữ liệu tình trạng đơn hàng với số liệu đơn tương ứng
                categories = categories, // Mục thời gian
                amountOrders = amountOrders, // Tổng đơn tương ứng thời gian
            });
        }

        [HttpGet]
        public async Task<IActionResult> JustGetOneListSellingProducts()
        {
            try
            {
                // Sử dụng repository để lấy danh sách chi tiết hóa đơn
                List<ChiTietHoaDonVM> detailInvoices = (await _cthd.GetAllDetailInvoiceAsync()).ToList();

                // Tính sản phẩm bán chạy nhất
                var topSellingProducts = detailInvoices
                    .GroupBy(g => new { g.MaHoaDon, g.MaSp, g.TenSp, g.MaMau, g.TenMau, g.MaKichThuoc, g.TenKichThuoc, g.Hinh })
                    .Select(g => new ChiTietHoaDonVM
                    {
                        MaHoaDon = g.Key.MaHoaDon,
                        MaSp = g.Key.MaSp,
                        TenSp = g.Key.TenSp,
                        MaMau = g.Key.MaMau,
                        TenMau = g.Key.TenMau,
                        MaKichThuoc = g.Key.MaKichThuoc,
                        TenKichThuoc = g.Key.TenKichThuoc,
                        SoLuong = g.Sum(x => x.SoLuong), // Tổng số lượng
                        ThanhTien = g.Sum(x => x.ThanhTien),  // Tổng tiền
                        Hinh = g.Key.Hinh
                    })
                    .OrderByDescending(x => x.SoLuong)
                    .Take(10) // Lấy 5 sản phẩm bán chạy nhất
                    .ToList();

                // Tạo kết quả trả về
                var result = new
                {
                    TopProducts = topSellingProducts
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                var result = new { success = false, message = ex.Message };
                return BadRequest(result);
            }
        }
        /// <summary>
        /// Dữ liệu thống kê sản phẩm theo được chọn nhiều nhất,...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            try
            {
                // Sử dụng repository để lấy danh sách chi tiết hóa đơn
                List<ChiTietHoaDonVM> detailInvoices = (await _cthd.GetAllDetailInvoiceAsync()).ToList();

                // Tính sản phẩm bán chạy nhất
                var topSellingProducts = detailInvoices
                    .GroupBy(g => new { g.MaHoaDon, g.MaSp, g.TenSp, g.MaMau, g.TenMau, g.MaKichThuoc, g.TenKichThuoc, g.Hinh })
                    .Select(g => new ChiTietHoaDonVM
                    {
                        MaHoaDon = g.Key.MaHoaDon,
                        MaSp = g.Key.MaSp,
                        TenSp = g.Key.TenSp,
                        MaMau = g.Key.MaMau,
                        TenMau = g.Key.TenMau,
                        MaKichThuoc = g.Key.MaKichThuoc,
                        TenKichThuoc = g.Key.TenKichThuoc,
                        SoLuong = g.Sum(x => x.SoLuong), // Tổng số lượng
                        ThanhTien = g.Sum(x => x.ThanhTien),  // Tổng tiền
                        Hinh = g.Key.Hinh
                    })
                    .OrderByDescending(x => x.ThanhTien)
                    .Take(5) // Lấy 5 sản phẩm bán chạy nhất
                    .ToList();

                // Tính màu sắc được mua nhiều nhất
                var topSellingColors = detailInvoices
                    .GroupBy(g => new { g.MaHoaDon, g.MaSp, g.TenSp, g.MaMau, g.TenMau, g.MaKichThuoc, g.TenKichThuoc, g.Hinh })
                    .Select(g => new ChiTietHoaDonVM
                    {
                        MaHoaDon = g.Key.MaHoaDon,
                        MaSp = g.Key.MaSp,
                        TenSp = g.Key.TenSp,
                        MaMau = g.Key.MaMau,
                        TenMau = g.Key.TenMau,
                        MaKichThuoc = g.Key.MaKichThuoc,
                        TenKichThuoc = g.Key.TenKichThuoc,
                        SoLuong = g.Sum(x => x.SoLuong), // Tổng số lượng
                        ThanhTien = g.Sum(x => x.ThanhTien),  // Tổng tiền
                        Hinh = g.Key.Hinh
                    })
                    .OrderByDescending(x => x.SoLuong)
                    .Take(5) // Lấy 5 màu sắc bán chạy nhất
                    .ToList();

                // Tính kích thước được mua nhiều nhất
                var topSellingSizes = detailInvoices
                    .GroupBy(g => new { g.MaHoaDon, g.MaSp, g.TenSp, g.MaMau, g.TenMau, g.MaKichThuoc, g.TenKichThuoc, g.Hinh })
                    .Select(g => new ChiTietHoaDonVM
                    {
                        MaHoaDon = g.Key.MaHoaDon,
                        MaSp = g.Key.MaSp,
                        TenSp = g.Key.TenSp,
                        MaMau = g.Key.MaMau,
                        TenMau = g.Key.TenMau,
                        MaKichThuoc = g.Key.MaKichThuoc,
                        TenKichThuoc = g.Key.TenKichThuoc,
                        SoLuong = g.Sum(x => x.SoLuong), // Tổng số lượng
                        ThanhTien = g.Sum(x => x.ThanhTien),  // Tổng tiền
                        Hinh = g.Key.Hinh
                    });

                // Tạo kết quả trả về
                var result = new
                {
                    TopProducts = topSellingProducts,
                    TopColors = topSellingColors,
                    TopSizes = topSellingSizes
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                var result = new { success = false, message = ex.Message };
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết cho sản phẩm trong Datatable
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailProduct(int id)
        {
            var detailProduct = await _product.GetCompareProductVmByIdAsync(id);
            return Ok(detailProduct);
        }

        /// <summary>
        /// Dữ liệu thống kê danh sách nhân viên mang lại doanh thu nhiều nhất trong day/week/month/year
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEmployeeOrderStats()
        {
            List<NhanVienVM> nhanVienVMs = [.. (await _tk.GetEmployeeOrderStatsAsync())];
            List<InvoiceVM> hoadonVMs = [.. (await _hd.GetAllInvoiceDataAsync())];

            // Initialize revenue and order count for each employee
            foreach (var nhanVien in nhanVienVMs)
            {
                nhanVien.SoDonHangXuLy = 0; // Start with zero processed orders
                nhanVien.DoanhThuMangLai = 0; // Start with zero revenue
            }

            // Process invoices and update employee stats
            foreach (var invoice in hoadonVMs)
            {
                // Assuming there is a property in InvoiceVM that indicates the employee responsible
                // Here, I'm assuming it's named `MaNv`.
                // Replace it with the actual property used to link invoices to employees.
                int employeeId = invoice.MaNhanVien; // Adjust this as per your actual property name

                var nhanVien = nhanVienVMs.FirstOrDefault(n => n.MaNv == employeeId);
                if (nhanVien != null)
                {
                    // Increment the count of processed orders
                    nhanVien.SoDonHangXuLy++;

                    // Add to the revenue the total of the invoice
                    nhanVien.DoanhThuMangLai += (int)invoice.TongTien; // Assuming revenue is in integer
                }
            }

            return Ok(nhanVienVMs);
        }

        /// <summary>
        /// Dữ liệu thống kê sản phẩm theo được chọn nhiều nhất,...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStatUserAsync()
        {
            var data = await _tk.GetStatUser();

            // Chuyển đổi dữ liệu từ (int, int) thành một đối tượng JSON dễ xử lý trên Front-end
            var formattedData = new[]
            {
                new { UserType = "Employee", Active = data.ElementAt(0).Item1, Inactive = data.ElementAt(0).Item2 },
                new { UserType = "Customer", Active = data.ElementAt(1).Item1, Inactive = data.ElementAt(1).Item2 }
            };

            // Trả về dữ liệu JSON định dạng đúng cho front-end
            return Ok(formattedData);
        }


        #region//Non API

        #endregion
    }
}
