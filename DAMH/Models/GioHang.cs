using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAMH.Models
{
    public class GioHang
    {
        GiayDataContext data = new GiayDataContext();

        public GioHang(string MaSP)
        {
            this.sMaSP = MaSP;
            sanpham sanpham = data.sanphams.Single(n => n.masp == MaSP);
            sTenSP = sanpham.tensp;
            iDonGia = int.Parse(sanpham.dongia.ToString());
            sAnhBia = sanpham.hinhsp;
            iSoLuong = 1;
        }

        public string sMaSP { get; set; }
        public string sTenSP{ get; set; }
        public int iDonGia { get; set; }
        public int iSoLuong{ get; set; }
        public string sAnhBia { get; set; }
        public int iThanhTien {
            get { return iSoLuong * iDonGia; }
        }
    }
}