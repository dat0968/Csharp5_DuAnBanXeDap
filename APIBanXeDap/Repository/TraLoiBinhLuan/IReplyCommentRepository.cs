using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.TraLoiBinhLuan
{
    public interface IReplyCommentRepository
    {
        IEnumerable<Traloibinhluan> GetAll();
        Traloibinhluan GetById(int id);
        void Create(Traloibinhluan reply);
        void Update(Traloibinhluan reply);
        void Delete(int id);
        void Save();
    }
}
