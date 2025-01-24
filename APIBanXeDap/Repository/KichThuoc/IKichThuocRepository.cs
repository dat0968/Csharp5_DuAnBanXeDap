using APIBanXeDap.Models;

namespace APIBanXeDap.Repository.KichThuoc
{
    public interface IKichThuocRepository
    {
        public List<SizeVM> GetAll();
        public SizeVM CreateSize(SizeVM size);
    }
}
