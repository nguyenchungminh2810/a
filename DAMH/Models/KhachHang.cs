using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAMH.Models
{
    public class KhachHang
    {
        public KhachHang(string tendn, string matkhau, bool remember)
        {
            this.tendn = tendn;
            this.matkhau = matkhau;
            Remember = remember;
        }

        public string tendn{ get; set; }
        public string matkhau { get; set; }
        public bool Remember{ get; set; }
    }
}