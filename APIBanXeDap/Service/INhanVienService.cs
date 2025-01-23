using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

public interface INhanVienService
    {
        List<NhanVienVM> GetAllNhanVien(string? keyword, string? sort);
        NhanVienVM AddNhanVien(NhanVienVM nhanVienVM);
        NhanVienVM UpdateNhanVien(int id, NhanVienVM nhanVienVM);
        void ToggleStatus(int id, string isActive);
        void ToggleIsDelete(int id);
        NhanVienVM GetNhanVienById(int id);
        void ImportNhanViens(List<NhanVienVM> nhanVienList);
        PagedResult<NhanVienVM> GetPagedNhanVien(int pageNumber, int pageSize, string? keyword, string? sort);
    }

