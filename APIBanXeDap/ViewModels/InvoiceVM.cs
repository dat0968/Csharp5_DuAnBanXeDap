namespace APIBanXeDap.ViewModels
{
    public class InvoiceVM
    {
        public int MaHoaDon { get; set; }
        public string DiaChiNhanHang { get; set; }
        public DateTime NgayTao { get; set; }
        public string TinhTrang { get; set; }
        public string Httt { get; set; } // Hình thức thanh toán
        public string CustomerName { get; set; } // Họ tên khách hàng
        public string CustomerPhone { get; set; } // Số điện thoại khách hàng
        public string CustomerAddress { get; set; } // Địa chỉ khách hàng
        public List<InvoiceItemViewModel> Items { get; set; } = new List<InvoiceItemViewModel>();
        public decimal TotalAmount { get; set; }

        public class InvoiceItemViewModel
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Total { get; set; }
        }
    }
}
