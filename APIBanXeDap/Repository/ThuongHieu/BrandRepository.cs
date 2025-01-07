using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.ThuongHieu
{
    public class BrandRepository : IBrandRepository
    {
        private readonly Csharp5Context db;

        public BrandRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public List<Thuonghieu> getAllBrand()
        {
            return db.Thuonghieus.ToList();
        }
    }
}
