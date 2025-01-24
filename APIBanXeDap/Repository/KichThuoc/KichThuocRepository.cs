using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.KichThuoc
{
    public class KichThuocRepository : IKichThuocRepository
    {
        private readonly Csharp5Context db;

        public KichThuocRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public SizeVM CreateSize(SizeVM size)
        {
            db.Add(size);
            db.SaveChanges();
            return size;
        }

        public List<SizeVM> GetAll()
        {
            return db.Kichthuocs.ToList();
        }
    }
}
