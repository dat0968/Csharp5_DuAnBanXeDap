using APIBanXeDap.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace APIBanXeDap.Repository.BinhLuan
{
    public class CommentRepository : ICommentRepository
    {
        private readonly Csharp5Context _context;

        public CommentRepository(Csharp5Context context)
        {
            _context = context;
        }
        // Lấy danh sách tất cả bình luận (lọc những bình luận chưa bị xóa)
        public IEnumerable<Binhluan> GetAll()
        {
            return _context.Binhluans
                .Where(bl => !bl.IsDelete)
                .Include(bl => bl.SanPham)
                .Include(bl => bl.KhachHang)
                .ToList();
        }
        public Binhluan? GetById(int id)
        {
            return _context.Binhluans.FirstOrDefault(bl => bl.MaBinhLuan == id && !bl.IsDelete);
        }
        public void Create(Binhluan binhLuan)
        {
            if (binhLuan == null)
            {
                throw new ArgumentNullException(nameof(binhLuan), "Dữ liệu bình luận không hợp lệ.");
            }

            _context.Binhluans.Add(binhLuan);
            _context.SaveChanges();
        }
        public void Edit(Binhluan binhLuan)
        {
            var existingBinhLuan = _context.Binhluans.FirstOrDefault(bl => bl.MaBinhLuan == binhLuan.MaBinhLuan);
            if (existingBinhLuan == null)
            {
                throw new Exception("Không tìm thấy bình luận.");
            }

            existingBinhLuan.NoiDung = binhLuan.NoiDung;
            existingBinhLuan.Rating = binhLuan.Rating;
            _context.SaveChanges();
        }
    }
}
