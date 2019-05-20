


using DAMH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAMH.Controllers
{
    
    public class GioHangController : Controller
    {
        GiayDataContext db = new GiayDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang==null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        [Authorize]
        public ActionResult ThemGioHang(string id, string strURL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang gioHang = lstGioHang.Find(n => n.sMaSP == id);
            if (gioHang==null)
            {
                gioHang = new GioHang(id);
                lstGioHang.Add(gioHang);
                return Redirect(strURL);    
            }
            else
            {
                gioHang.iSoLuong++;
                return Redirect(strURL);
            }
        }
        public int TongSoLuong()
        {
            int Tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                Tong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return Tong;
        }
        private double TongTien()
        {
            double tong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang!=null)
                        {
                            tong = lstGioHang.Sum(n => n.iThanhTien);
                        }
                        return tong;
        }
        
        [Authorize]
        // GET: GioHang
        public ActionResult Index()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count==0)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [Authorize]
        [HttpGet]
        public ActionResult ThanhToan()
        {
            khachhang khachhangs = db.khachhangs.SingleOrDefault(n => n.makh == int.Parse(Session["makh"].ToString()));
            var view = new Tuple<khachhang, List<GioHang>>(khachhangs,LayGioHang());
            ViewBag.TongTien = TongTien();
            return View(view);
        }
        [HttpPost]
        public ActionResult ThanhToan(FormCollection formCollection,dondathang dondathang)
        {
            var noigiao = formCollection["Item1.diachikh"];
            
            List<GioHang> gh = LayGioHang();
                dondathang.ngaydat = DateTime.Now;
                dondathang.noigiao = noigiao;
                dondathang.makh = int.Parse(Session["makh"].ToString());
                dondathang.thanhtien = (decimal)TongTien();
            dondathang.dathanhtoan = "no";
            dondathang.tinhtranggiaohang = false;
            db.dondathangs.InsertOnSubmit(dondathang);
            db.SubmitChanges();
            foreach (var item in gh)
            {
                chitietdonhang chitietdonhang = new chitietdonhang();
                chitietdonhang.madh = dondathang.madh;
                chitietdonhang.masp = item.sMaSP;
                chitietdonhang.soluong = item.iSoLuong;
                chitietdonhang.dongia = item.iDonGia;
                db.chitietdonhangs.InsertOnSubmit(chitietdonhang);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        //giỏ hàng ở nút đăng nhập góc trái trên khi gê chuột vào
        public ActionResult ParGioHang()
        {
            List<GioHang> gios = LayGioHang();
            ViewBag.TongSL = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.SL = DemSP();
            return PartialView(gios);
        }
        //so sản phẩm hiển thị trên icon giỏ hàng
        public int DemSP()
        {
            List<GioHang> gios = LayGioHang();
            return gios.Count();
        }
    }
}