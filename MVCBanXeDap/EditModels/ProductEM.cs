using System.ComponentModel.DataAnnotations;

namespace MVCBanXeDap.EditModels
{
    public class ProductEM
    {
        public int MaSP { get; set; }

        public string TenSp { get; set; }

        public int MaThuongHieu { get; set; }

        public string? Hinh { get; set; }

        public string MoTa { get; set; }
        public DateOnly NgaySanXuat { get; set; }
        public int MaNhaCC { get; set; }
        public int MaDanhMuc { get; set; }
        public IFormFile file { get; set; }
        public List<IFormFile> files { get; set; }
    }
}
