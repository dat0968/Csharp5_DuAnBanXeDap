using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MVCBanXeDap.Models;

public class DanhmucVM
{
    public int MaDanhMuc { get; set; }

    public string TenDanhMuc { get; set; }

    public bool? IsDelete { get; set; }

}
