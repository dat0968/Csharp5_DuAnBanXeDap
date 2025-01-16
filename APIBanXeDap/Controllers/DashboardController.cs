using APIBanXeDap.Repository.HoaDon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBanXeDap.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IHoaDonRepository _hd;
        public DashboardController(IHoaDonRepository hd)
        {
            this._hd = hd;
        }
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
                            .Sum(x => Convert.ToInt32(x.Items.Sum(it => it.Total)))
                    ).ToList();
                    categories = daysOfWeek.ToList();
                    break;

                case "week":
                    var weeksInMonth = Enumerable.Range(1, 4);
                    columnData = weeksInMonth.Select(week =>
                        listOrders
                            .Where(x => x.NgayTao.Month == today.Month)
                            .Where(x => (x.NgayTao.Day - 1) / 7 + 1 == week)
                            .Sum(x => Convert.ToInt32(x.Items.Sum(it => it.Total)))
                    ).ToList();
                    categories = weeksInMonth.Select(week => "Week " + week).ToList();
                    break;

                case "month":
                    int currentYear = today.Year;
                    columnData = Enumerable.Range(1, 12).Select(month =>
                        listOrders
                            .Where(x => x.NgayTao.Month == month && x.NgayTao.Year == currentYear)
                            .Sum(x => Convert.ToInt32(x.Items.Sum(it => it.Total)))
                    ).ToList();
                    categories = Enumerable.Range(1, 12).Select(month => "Month " + month).ToList();
                    break;

                case "year":
                    currentYear = today.Year;
                    var recentYears = Enumerable.Range(currentYear - 5, 5);
                    columnData = recentYears.Select(year =>
                        listOrders
                            .Where(x => x.NgayTao.Year == year)
                            .Sum(x => Convert.ToInt32(x.Items.Sum(it => it.Total)))
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

        public async Task<IActionResult> GetAllOrderData(string timeRange)
        {
            var listOrders = (await _hd.GetAllAsync());

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


        public async Task<IActionResult> GetOrderStatusData(string timeRange)
        {
            var listOrders = (await _hd.GetAllAsync())
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

        public async Task<IActionResult> GetOrderOverViewData(string timeRange)
        {
            var listOrders = (await _hd.GetAllAsync())
                .Where(x => x.TinhTrang != null);

            var orderStatuses = new[] { "Đã xác nhận", "Đã giao cho đơn vị vận chuyển", "Đang giao hàng", "Chờ thanh toán", "Hoàn trả/Hoàn tiền", "Đã hủy", "Chờ xác nhận" };

            var statusData = new Dictionary<string, List<int>>();
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
                            amountOrders++;
                            return listOrders
                                .Where(x => x.TinhTrang == status && x.ThoiGianGiao.DayOfWeek.ToString().StartsWith(day.Substring(0, 3)))
                                .Count();
                        }).ToList();
                    }
                    categories = daysOfWeek.ToList();
                    break;

                // Các phần khác cho tuần, tháng, năm...

                default:
                    foreach (var status in orderStatuses)
                    {
                        statusData[status] = new List<int> { 0 };
                    }
                    categories = new List<string> { "Unknown" };
                    break;
            }

            var result = orderStatuses.Select(status => new
            {
                name = status,
                data = statusData[status]
            }).ToList();

            return Ok(new
            {
                data = result,
                categories = categories,
                amountOrders = amountOrders,
            });
        }
    }
}
