using APIBanXeDap.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APIBanXeDap.Repository.MauSac
{
    public class MauSacRepository : IMauSacRepository
    {
        private readonly Csharp5Context db;

        public MauSacRepository(Csharp5Context db)
        {
            this.db = db;
        }
        public Mausac CreateColor(Mausac color)
        {
            db.Add(color);
            db.SaveChanges();
            return color;
        }

        public List<Mausac> GetAll()
        {
            return db.Mausacs.ToList();
        }
    }
}
