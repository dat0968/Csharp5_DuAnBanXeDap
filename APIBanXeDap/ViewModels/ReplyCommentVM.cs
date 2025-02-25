namespace APIBanXeDap.ViewModels
{
    public class ReplyCommentVM
    {
        public int MaPhanHoi { get; set; }         // Mã phản hồi
        public int MaBinhLuan { get; set; }        // Mã bình luận
        public string? NoiDung { get; set; }       // Nội dung trả lời
        public string? TenNguoiDung { get; set; }  // Tên người dùng (hiển thị)
        public int MaNV { get; set; }        // ✅ Mã nhân viên thêm vào đây
        public DateTime NgayTao { get; set; }      // Ngày tạo
    }
}
