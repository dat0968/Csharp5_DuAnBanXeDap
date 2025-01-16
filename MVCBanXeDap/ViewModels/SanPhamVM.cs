namespace MVCBanXeDap.ViewModels
{
    public class SanPhamVM
    {

        public int MaSp { get; set; }

        public string TenSp { get; set; } = null!;

        public int MaThuongHieu { get; set; }

        public string Hinh { get; set; } = null!;

        public string MoTa { get; set; } = null!;

        public DateOnly NgaySanXuat { get; set; }

        public int MaNhaCc { get; set; }

        public int MaDanhMuc { get; set; }

        public bool? IsDelete { get; set; }

    }
}
