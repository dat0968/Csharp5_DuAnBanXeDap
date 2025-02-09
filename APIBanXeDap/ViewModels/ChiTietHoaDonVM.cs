namespace APIBanXeDap.ViewModels
{
    public class ChiTietHoaDonVM
    {
        public int MaHoaDon { get; set; }

        public int MaSp { get; set; }

        public string TenSp { get; set; } = null!;

        public int MaMau { get; set; }

        public string TenMau { get; set; } = null!;

        public int MaKichThuoc { get; set; }

        public string TenKichThuoc { get; set; } = null!;
        public float GiamGiaMaCoupon { get; set; } = 0;
        public float PhiVanChuyen { get; set; } = 0;

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }

        public decimal ThanhTien { get; set; }

        // Optional: Including product description for better context
        public string MoTa { get; set; } = null!;

        // Optional: Image link for the product
        public string Hinh { get; set; } = null!;
    }
}
