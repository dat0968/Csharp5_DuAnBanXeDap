using APIBanXeDap.Models;
using APIBanXeDap.ViewModels;

public interface IKhachHangService
{
    List<KhachHangVM> GetAllKhachHang(string? keyword, string? sort);
    KhachHangVM AddKhachHang(KhachHangVM khachHangVM);
    KhachHangVM UpdateKhachHang(int id, KhachHangVM khachHangVM);
    void ToggleStatus(int id, string isActive);
    void ToggleIsDelete(int id);
    KhachHangVM GetKhachHangById(int id);
    void ImportKhachHangs(List<KhachHangVM> khachHangList);
    PagedResult<KhachHangVM> GetPagedKhachHang(int pageNumber, int pageSize, string? keyword, string? sort);
}
