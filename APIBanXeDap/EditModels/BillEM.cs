namespace APIBanXeDap.EditModels
{
    public class BillEM
    {
        public int MaHoaDon { get; set; }

        public string DiaChiNhanHang { get; set; } = null!;

        public DateOnly NgayTao { get; set; }

        public string Httt { get; set; } = null!;

        public string TinhTrang { get; set; } = null!;

        public int? MaNv { get; set; }

        public int MaKh { get; set; }

        public string? MoTa { get; set; }

        public string? Hoten { get; set; }

        public string? Sdt { get; set; }

        public DateOnly ThoiGianGiao { get; set; }
    }
}
