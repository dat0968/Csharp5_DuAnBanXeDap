using System;
using System.Collections.Generic;

namespace MVCBanXeDap.Models;

public partial class BrandVM
{
    public int MaThuongHieu { get; set; }

    public string TenThuongHieu { get; set; }

    public bool? IsDelete { get; set; }
}
