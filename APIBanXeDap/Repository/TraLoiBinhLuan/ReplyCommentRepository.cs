using APIBanXeDap.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBanXeDap.Repository.TraLoiBinhLuan
{
    public class ReplyCommentRepository : IReplyCommentRepository
    {
        private readonly Csharp5Context _context;

        public ReplyCommentRepository(Csharp5Context context)
        {
            _context = context;
        }

        public IEnumerable<Traloibinhluan> GetAll()
        {
            return _context.Traloibinhluans
                .Include(r => r.BinhLuan)
                .Include(r => r.NhanVien)
                .AsNoTracking()
                .ToList();
        }

        public Traloibinhluan? GetById(int id)
        {
            return _context.Traloibinhluans
                .Include(r => r.BinhLuan)
                .Include(r => r.NhanVien)
                .AsNoTracking()
                .FirstOrDefault(r => r.MaTraLoi == id);
        }

        public void Create(Traloibinhluan reply)
        {
            _context.Traloibinhluans.Add(reply);
        }

        public void Update(Traloibinhluan reply)
        {
            _context.Traloibinhluans.Update(reply);
        }

        public void Delete(int id)
        {
            var reply = _context.Traloibinhluans.Find(id);
            if (reply != null)
            {
                _context.Traloibinhluans.Remove(reply);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
