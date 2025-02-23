using MVCBanXeDap.Models;
using System.ComponentModel.DataAnnotations;

namespace MVCBanXeDap.ViewModels
{
    public class ProductVM
    {
        public int MaSP { get; set; }
        
        public string TenSp { get; set; }

        public string ThuongHieu { get; set; }

        public string Hinh { get; set; }

        public string MoTa { get; set; }
        public DateOnly NgaySanXuat { get; set; }
        public string NhaCungCap { get; set; }

        public string DanhMuc { get; set; }
        public int SoLuong { get; set; }
        public string KhoangGia { get; set; }
        public List<DetailsProductVM> Chitietsanphams { get; set; }
        public List<ImgProductVM> Hinhanhs { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        private double _rating;
        public double Rating
        {
            get => Math.Round(_rating, 1);
            set => _rating = value;
        }
        public int TotalReviews { get; set; }
        public List<CommentVM>? Comments { get; set; }
    }
}
