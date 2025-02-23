namespace APIBanXeDap.ViewModels
{
    public class CommentVM
    {
        public int MaBinhLuan { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        public double Rating { get; set; }
        public bool IsDelete { get; set; }
        public string TenSanPham { get; set; }
        public string TenKhachHang { get; set; }
    }
}
