﻿using APIBanXeDap.Repository.Token;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using MVCBanXeDap.Helper;
using MVCBanXeDap.Services.Jwt;
using MVCBanXeDap.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1;
using System.Runtime.CompilerServices;
using System.Text;


namespace MVCBanXeDap.Controllers
{
    public class CartController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7137/api/");
        private readonly IjwtToken jwtToken;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public CartController(IjwtToken jwtToken)
        {
            _client = new HttpClient();
            this.jwtToken = jwtToken;
            _client.BaseAddress = baseAddress;
        }
        public DetailsCart Cart => HttpContext.Session.Get<DetailsCart>("MYCART") ?? new DetailsCart();
        [HttpGet]
        public IActionResult Index()
        {
            var mycart = Cart;
            var ListItemCart = mycart.ListCartItem;
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
            ViewBag.PhoneNumber = HttpContext.Session.GetString("PhoneNumber")?.Trim('"');
            ViewBag.FullName = HttpContext.Session.GetString("FullName")?.Trim('"');
            return View(mycart);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(string fullname, string sdt, string? mota, string diachicuthe, string paymentMethod)
        {
            var mycart = Cart;
            var accesstoken = HttpContext.Session.GetString("AccessToken")?.Trim('"');
            var refreshtoken = HttpContext.Session.GetString("RefreshToken")?.Trim('"');
            if (accesstoken != null && refreshtoken != null)
            {
                var validateAccessToken = await jwtToken.ValidateAccessToken(accesstoken, refreshtoken);
                if (!string.IsNullOrEmpty(validateAccessToken))
                {
                    var idUser = jwtToken.GetUserIdFromToken(validateAccessToken);
                    var CouponCode = mycart.MaCoupon != null ? mycart.MaCoupon : null;
                    var thongtinhoadon = new ThongTinHoaDonVM();
                    thongtinhoadon.ChiTietHoaDons = new List<ChiTietHoaDonVM>();
                    if(CouponCode != null)
                    {
                        thongtinhoadon.MaCoupon = CouponCode;
                    }
                    var hoadon = new HoadonVM
                    {
                        DiaChiNhanHang = diachicuthe + " " + mycart.DiaChi,
                        NgayTao = DateOnly.FromDateTime(DateTime.Now),
                        Httt = paymentMethod,
                        TinhTrang = "Chờ xác nhận",
                        MaNv = null,
                        MaKh = int.Parse(idUser),
                        MoTa = mota,
                        Hoten = fullname,
                        Sdt = sdt,
                        ThoiGianGiao = DateOnly.MinValue,
                        GiamGiaMaCoupon = (float)mycart.GiamGia,
                        PhiVanChuyen = mycart.phiship != null ? (float)mycart.phiship : 0,
                        TienGoc = (float)mycart.GiaGoc,
                        TongTien = (float)mycart.TongTien,
                    };
                    thongtinhoadon.HoaDon = hoadon;
                    foreach(var chitiet in mycart.ListCartItem)
                    {
                        /*Thuộc tích mã hóa đơn của đối tượng ChiTietHoaDonVM sẽ được xử lý sau ở bên project Webapi 
                      để lấy mã hóa đơn ở phần tử mới nhất vừa được thêm vào trong bảng hóa đơn*/
                        var chitiethoadon = new ChiTietHoaDonVM
                        {
                            MaSp = chitiet.MaSP,
                            MaMau = chitiet.MaMau,
                            MaKichThuoc = chitiet.MaKichThuoc,
                            SoLuong = chitiet.SoLuong,
                            Gia = chitiet.DonGia,
                            ThanhTien = chitiet.SoLuong * chitiet.DonGia,
                        };
                        thongtinhoadon.ChiTietHoaDons.Add(chitiethoadon);
                    }


                    if(paymentMethod == "VNPAY")
                    {
                        return RedirectToAction("CreatePaymentUrl", new { mycart.TongTien, mota });
                    }
                    var model = JsonConvert.SerializeObject(thongtinhoadon);
                    StringContent content = new StringContent(model, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "Checkouts/CreatePaymentUrl", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var redirectUrl = response.Headers.Location?.ToString();
                        if (!string.IsNullOrEmpty(redirectUrl))
                        {
                            return Redirect(redirectUrl);
                        }
                       // return RedirectToAction("Index");
                    }
                    return NotFound();
                }
            }
            return RedirectToAction("LogoutAccount", "Accounts");
        }
       
    }
}
