using APIBanXeDap.Models;
namespace APIBanXeDap.Repository
{
    public interface ICommentRepository
    {
        IEnumerable<Binhluan> GetAll();
        Binhluan? GetById(int id);
        void Create(Binhluan binhLuan);

        void Edit(Binhluan binhLuan);
    }
}
