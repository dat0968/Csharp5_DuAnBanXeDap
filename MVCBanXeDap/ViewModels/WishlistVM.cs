namespace MVCBanXeDap.ViewModels
{
    public class WishlistVM
    {
        public int Ma { get; set; }
        public int MaDoiTuong { get; set; }
        public int MaNguoiDung { get; set; }
        public string DoiTuongYeuThich { get; set; } = "SanPham"; // SanPham / BinhLuan
        public string? NoiDungBinhLuan { get; set; }
        public string? TenSp { get; set; }

        public string? ThuongHieu { get; set; }

        public string? Hinh { get; set; }

        public string? MoTa { get; set; }
        public DateOnly NgayBinhLuan { get; set; }
        public string? NhaCungCap { get; set; }

        public string? DanhMuc { get; set; }
        public int SoLuong { get; set; }
        public string? KhoangGia { get; set; }
    }
}
