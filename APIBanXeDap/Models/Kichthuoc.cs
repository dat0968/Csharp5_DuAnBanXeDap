using System;
using System.Collections.Generic;

namespace APIBanXeDap.Models;

public partial class SizeVM
{
    public int MaKichThuoc { get; set; }

    public string TenKichThuoc { get; set; } = null!;

    public bool? IsDelete { get; set; }

    public virtual ICollection<Chitietsanpham> Chitietsanphams { get; set; } = new List<Chitietsanpham>();
}
