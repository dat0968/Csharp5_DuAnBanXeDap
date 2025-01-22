using APIBanXeDap.EditModels;
using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.NhaCungCap
{
    public interface ISupplierRepository
    {
        public SupplierEM CreateSupplier(SupplierEM supplier);
        public void DeleteSupplier(int id);
        public List<Nhacungcap> GetAllSupplier(string? keywords, string? sort);
        public SupplierEM GetSupplierById(int id);
        public void UpdateSupplier(SupplierEM supplier);
    }
}
