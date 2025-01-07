using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.NhaCungCap
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly Csharp5Context db;

        public SupplierRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public List<Nhacungcap> getAllSupplier()
        {
            return db.Nhacungcaps.ToList();
        }
    }
}
