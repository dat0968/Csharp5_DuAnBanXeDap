using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.DanhMuc
{
    public class DanhMucRepository : IDanhMucRepository
    {
        private readonly Csharp5Context db;

        public DanhMucRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public List<Danhmuc> GetAllCategory()
        {
            return db.Danhmucs.ToList();
        }
    }
}
