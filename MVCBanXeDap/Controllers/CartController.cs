using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MVCBanXeDap.Helper;
using MVCBanXeDap.ViewModels;
using Org.BouncyCastle.Asn1;
using System.Runtime.CompilerServices;

namespace MVCBanXeDap.Controllers
{
    public class CartController : Controller
    {
        public DetailsCart Cart => HttpContext.Session.Get<DetailsCart>("MYCART") ?? new DetailsCart();
        [HttpGet]
        public IActionResult Index()
        {
            var mycart = Cart;
            var ListItemCart = mycart.ListCartItem;
            //mycart.GiaGoc = ListItemCart != null && ListItemCart.Count > 0 ? ListItemCart.Sum(p => p.ThanhTien) : 0;
            return View(mycart);
        }
        [HttpPost]
        public IActionResult AddToCart( [FromBody] CartItem item)
        {
            var mycart = Cart;
            var ListItemCart = mycart.ListCartItem;
            try
            {
                var checkCart = ListItemCart.FirstOrDefault(p => p.MaSP == item.MaSP && p.MaMau == item.MaMau && p.MaKichThuoc == item.MaKichThuoc);
                if (checkCart == null)
                {
                    ListItemCart.Add(item);
                }
                else
                {
                    checkCart.SoLuong += item.SoLuong;
                    if(checkCart.SoLuong > checkCart.SoLuongToiDa)
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = $"Bạn đã có {checkCart.SoLuong - item.SoLuong} sản phẩm trong giỏ hàng. Không thể thêm số lượng đã chọn vào giỏ hàng vì sẽ vượt quá giới hạn mua hàng của bạn",
                        });
                    }
                    if(checkCart.SoLuong < 1)
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = $"Sản phẩm đã đạt tới số lượng tối thiểu trong giỏ hàng",
                        });
                    }
                }
                mycart.ListCartItem = ListItemCart;
                HttpContext.Session.Set("MYCART", mycart);
                return Json(new
                {
                    Success = true,
                    Message = "Thêm giỏ hàng thành công",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}",
                });
            }
        }
        [HttpGet]
        public IActionResult GetCartCount()
        {
            try
            {
                var mycart = Cart;
                var ListItemCart = mycart.ListCartItem;
                int totalItems = ListItemCart.Sum(item => item.SoLuong);
                decimal totalAmount = ListItemCart != null && ListItemCart.Count > 0 ? ListItemCart.Sum(p => p.ThanhTien) : 0;
                return Json(new { Success = true, Count = totalItems, TotalAmount = totalAmount });
            }
            catch
            {
                return Json(new { Success = false, Count = 0, TotalAmount = 0 });
            }
        }
        [HttpPost]
        public IActionResult RemoveCart([FromBody] CartItem item)
        {
            var mycart = Cart;
            var ListItemCart = mycart.ListCartItem;
            try
            {
                var checkCart = ListItemCart.FirstOrDefault(p => p.MaSP == item.MaSP && p.MaMau == item.MaMau && p.MaKichThuoc == item.MaKichThuoc);
                if (checkCart == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = $"Sản phẩm này không tồn tại trong giỏ hàng",
                    });
                }
                ListItemCart.Remove(checkCart);
                mycart.ListCartItem = ListItemCart;
                HttpContext.Session.Set("MYCART", mycart);
                return Json(new
                {
                    Success = true,
                    Message = $"Đã xóa sản phẩm ra khỏi giỏ hàng",
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}",
                });
            }
        }
        [HttpGet]
        public IActionResult HandlerCouponCode(string? MaCoupon, int? sotiengiam = null, float? phantramgiam = null)
        {
            try
            {
                var mycart = Cart;
                var ListItemCart = mycart.ListCartItem;
                mycart.MaCoupon = !string.IsNullOrEmpty(MaCoupon) ? MaCoupon : null;
                mycart.SoTienGiam = sotiengiam;
                mycart.PhanTramGiam = phantramgiam;
                HttpContext.Session.Set("MYCART", mycart);
                return Json(new
                {
                    Success = true,
                    Message = "Áp dụng mã coupon thành công"
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}"
                });
            }
        }
        [HttpGet]
        public IActionResult GetShippingFee(float? phiship = null, string? tinh = null, string? quanhuyen = null, string? phuong = null)
        {
            try
            {
                var mycart = Cart;
                mycart.phiship = phiship != null ? phiship : 0;
                mycart.Tinh = tinh != null ? tinh : null;
                mycart.Phuong = phuong != null ? phuong : null;
                mycart.QuanHuyen = quanhuyen != null ? quanhuyen : null;
                HttpContext.Session.Set("MYCART", mycart);
                return Json(new
                {
                    Success = true,
                    PhiShip = mycart.phiship,
                    TongTien = mycart.TongTien,
                    Message = "Truy xuất phí vận chuyển thành công"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = $"Error {ex.Message}"
                });
            }
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            var mycart = Cart;
            //var ListItemCart = mycart.ListCartItem;         
            //if (ListItemCart == null || ListItemCart.Count == 0)
            //{
            //    TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống, vui lòng thêm sản phẩm vào giỏ hàng trước khi thanh toán";
            //    return RedirectToAction("Index", "Cart");
            //}
            // Nếu phí ship bằng null thì mặc định lấy giá trị phí ship trước đó đã add vào trước đó, nếu phí ship trước đó bằng null thì lấy giá trị mặc định bằng 0
            //mycart.phiship = phiship != null ? phiship : mycart.phiship != null ? mycart.phiship : 0;
            //mycart.Tinh = tinh != null ? tinh : mycart.Tinh != null ? mycart.Tinh : null;
            //mycart.Phuong = phuong != null ? phuong : mycart.Phuong != null ? mycart.Phuong : null; 
            //mycart.QuanHuyen = quanhuyen != null ? quanhuyen : mycart.QuanHuyen != null ? mycart.QuanHuyen : null; 
            //mycart.TongTien = mycart.GiaGoc - (decimal)mycart.GiamGia + (decimal)mycart.phiship;
            //mycart.MaCoupon = !string.IsNullOrEmpty(MaCoupon) ? MaCoupon : null;
            //HttpContext.Session.Set("MYCART", mycart);
            return View(mycart);
        }
    }
}
