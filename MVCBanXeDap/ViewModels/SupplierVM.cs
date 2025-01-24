using System;
using System.Collections.Generic;

namespace MVCBanXeDap.Models;

public partial class SupplierVM
{
    public int MaNhaCc { get; set; }

    public string TenNhaCc { get; set; } 

    public string DiaChi { get; set; } 

    public string Email { get; set; } 

    public string Sdt { get; set; } 

    public bool? IsDelete { get; set; }
}
