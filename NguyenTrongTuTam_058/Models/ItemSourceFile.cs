using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnThi.Models
{
    public class ItemSourceFile
    {
        public string MaSp { get; set; } = null!;

        public string TenSp { get; set; } = null!;
        public string MaLoai { get; set; } = null!;
        public int SoLuongCo { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public ItemSourceFile(string maSp, string tenSp, string maLoai, int soLuongCo, decimal donGia)
        {
            MaSp = maSp;
            TenSp = tenSp;
            MaLoai = maLoai;
            SoLuongCo = soLuongCo;
            DonGia = donGia;
            ThanhTien = donGia * soLuongCo;
        }
        public ItemSourceFile()
        {
            MaSp = "";
            TenSp = "";
            MaLoai = "";
            SoLuongCo = 0;
            DonGia = 0;
            ThanhTien = 0;
        }
    }
}
