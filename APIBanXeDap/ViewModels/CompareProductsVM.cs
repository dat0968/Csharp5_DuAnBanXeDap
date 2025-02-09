namespace APIBanXeDap.ViewModels
{
    public class CompareProductVM
    {
        public int MaSp { get; set; }

        public string TenSp { get; set; } = null!;

        public int MaThuongHieu { get; set; }
        public string TenThuongHieu { get; set; } = null!;

        public string Hinh { get; set; } = null!;

        public string MoTa { get; set; } = null!;

        public DateOnly NgaySanXuat { get; set; }

        public int MaNhaCc { get; set; }
        public string TenNhaCc { get; set; } = null!;

        public string DiaChi { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Sdt { get; set; } = null!;

        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; } = null!;
        public List<DetailsProductVM> Chitietsanphams { get; set; }

        public bool? IsDelete { get; set; }
    }
}
