namespace APIBanXeDap.EditModels
{
    public class SupplierEM
    {
        public int MaNhaCc { get; set; }

        public string TenNhaCc { get; set; } = null!;

        public string DiaChi { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Sdt { get; set; } = null!;

        public bool? IsDelete { get; set; }
    }
}
