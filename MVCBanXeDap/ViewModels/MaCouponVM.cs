﻿namespace MVCBanXeDap.ViewModels
{
    public class MaCouponVM
    {
        public string Code { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 10);
        public decimal? SoTienGiam { get; set; }
        public float? PhanTramGiam { get; set; }
        public DateTime NgayHetHan { get; set; }
        public bool TrangThai { get; set; } = true;
        public DateTime NgayTao { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public bool DaSuDung { get; set; } = false;
    }
}
