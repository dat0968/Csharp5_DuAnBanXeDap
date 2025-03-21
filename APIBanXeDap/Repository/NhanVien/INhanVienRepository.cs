﻿using APIBanXeDap.Models;
using APIBanXeDap.Models.ViewModels;
using APIBanXeDap.ViewModels;
public interface INhanVienRepository
    {
     List<Nhanvien> GetAll(string? keyword, string? sort, string? status, string? gender);

        //Nhanvien Add(NhanVienVM nhanVienVM);
    Nhanvien Add(Nhanvien nhanVien);

    Nhanvien Update(int id, NhanVienVM nhanVienVM);
        Nhanvien GetNhanVienById(int id);
        void ToggleStatus(int id, string isActive);
        void ToggleIsDelete(int id);
    List<Nhanvien> GetPaged(int pageNumber, int pageSize, string? keyword, string? sort, string? status, string? gender);
    int GetTotalCount(string? keyword);
        bool IsCccdExists(string cccd);
        bool IsSdtExists(string sdt);
        bool IsEmailExists(string email);
        bool IsTenTaiKhoanExists(string tenTaiKhoan);

    }

