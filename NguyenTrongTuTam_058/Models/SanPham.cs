using System;
using System.Collections.Generic;

namespace OnThi.Models;

public partial class SanPham
{
    public string MaSp { get; set; } = null!;

    public string TenSp { get; set; } = null!;

    public decimal DonGia { get; set; }

    public int SoLuongCo { get; set; }

    public string? MaLoai { get; set; }

    public virtual ICollection<HoaDonChiTiet> HoaDonChiTiets { get; } = new List<HoaDonChiTiet>();

    public virtual LoaiSp? MaLoaiNavigation { get; set; }
}
