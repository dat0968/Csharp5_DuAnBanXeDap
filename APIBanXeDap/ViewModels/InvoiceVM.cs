namespace APIBanXeDap.ViewModels
{
    public class InvoiceVM
    {
        public int MaHoaDon { get; set; }
        public string DiaChiNhanHang { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime ThoiGianGiao { get; set; }
        public string TinhTrang { get; set; }
        public string HinhThucThanhToan { get; set; } // Hình thức thanh toán
        public string TenKhachHang { get; set; } // Họ tên khách hàng
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string SoDienThoaiKhachHang { get; set; } // Số điện thoại khách hàng
        public string DiaChiKhachHang { get; set; } // Địa chỉ khách hàng
        public List<ChiTietHoaDonViewModel> Items { get; set; } = new List<ChiTietHoaDonViewModel>();
        public decimal TongTien { get; set; }
        public float GiamGiaMaCoupon { get; set; } // Giảm giá mã coupon
        public float PhiVanChuyen { get; set; } // Phí vận chuyển
        public float TienGoc { get; set; } // Tiền gốc
        public string? LyDoHuy { get; set; }

        public class ChiTietHoaDonViewModel
        {
            public string TenSanPham { get; set; }
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal Tong { get; set; }
        }
    }
}
