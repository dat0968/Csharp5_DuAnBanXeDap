    using APIBanXeDap.Models;
    using APIBanXeDap.ViewModels;

    public interface IKhachHangRepository
    {
    List<Khachhang> GetAll(string? keyword, string? sort, string? status, string? gender);
    Khachhang Add(KhachHangVM khachHangVM);
        Khachhang Update(int id, KhachHangVM khachHangVM);
        Khachhang GetKhachHangById(int id);
        void ToggleStatus(int id, string isActive);
        void ToggleIsDelete(int id);
    List<Khachhang> GetPaged(int pageNumber, int pageSize, string? keyword, string? sort, string? status, string? gender);
    int GetTotalCount(string? keyword, string? status, string? gender);
    void Add(Khachhang khachHang);
    // Thêm các phương thức kiểm tra
    bool IsCccdExists(string cccd);
        bool IsSdtExists(string sdt);
        bool IsEmailExists(string email);
        bool IsTenTaiKhoanExists(string tenTaiKhoan);
    }
