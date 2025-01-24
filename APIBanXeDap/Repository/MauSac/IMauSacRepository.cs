using APIBanXeDap.Models;
namespace APIBanXeDap.Repository.MauSac
{
    public interface IMauSacRepository
    {
        public List<Mausac> GetAll();
        public Mausac CreateColor(Mausac color);
    }
}
