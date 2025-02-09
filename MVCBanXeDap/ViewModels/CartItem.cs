using System.Runtime.CompilerServices;

namespace MVCBanXeDap.ViewModels
{
    public class CartItem
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public int MaMau { get; set; }
        public string Mau { get; set; }
        public int MaKichThuoc { get; set; }
        public string KichThuoc { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongToiDa { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => (decimal)DonGia * SoLuong;
    }
}
