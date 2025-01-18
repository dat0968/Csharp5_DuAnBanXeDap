using System;

namespace APIBanXeDap.Models.ViewModels
{
    public class NhanVienVM
    {
        public int MaNv { get; set; }

        public string HoTen { get; set; } = null!;

        public string GioiTinh { get; set; } = null!;

        public DateOnly? NgaySinh { get; set; }

        public string? DiaChi { get; set; }

        public string Sdt { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateOnly NgayVaoLam { get; set; }

        public int Luong { get; set; }

        public string VaiTro { get; set; } = null!;

        public string? TinhTrang { get; set; }

        public bool IsDeleted { get; set; }
        public int SoDonHangXuLy { get; set; }
        public int DoanhThuMangLai { get; set; } // Giá trị không cố định
    }
}
