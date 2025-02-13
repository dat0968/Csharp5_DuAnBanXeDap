using APIBanXeDap.Models;

namespace APIBanXeDap.ViewModels
{
    public class ChiTietHoaDonVM
    {
        public int MaHoaDon { get; set; } 

        public int MaSp { get; set; }

        public string? TenSp { get; set; }

        public int MaMau { get; set; }

        public string? TenMau { get; set; } 

        public int MaKichThuoc { get; set; }

        public string? TenKichThuoc { get; set; }

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }

        public decimal ThanhTien { get; set; }

        // Optional: Including product description for better context
        public string? MoTa { get; set; } 

        // Optional: Image link for the product
        public string? Hinh { get; set; } 
    }
}
