using APIBanXeDap.Models;
using APIBanXeDap.Models.ViewModels;
using APIBanXeDap.ViewModels;

namespace APIBanXeDap.Repository.UpdateNhanVien
{
    public interface IUpdateNhanVienRepository
    {
        public void UpdateProflie(int id, NhanVienVM proflie);
        public Nhanvien GetNhanVienById(int id);
    }
}
