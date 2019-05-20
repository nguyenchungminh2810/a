using DAMH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DAMH.Controllers
{
    public class NguoiDungController : Controller
    {
        GiayDataContext db = new GiayDataContext();
        // GET: NguoiDung
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection,khachhang khachhang)
        {
            var tenkh = collection["tenkh"];
            var sdt = collection["sdtkh"];
            var email = collection["emailkh"];
            var diachi = collection["diachikh"];
            var tentk = collection["tentkkh"];
            var pass = collection["matkhau"];
            var repass = collection["matkhaukh"];
            var ngaysinh = string.Format("{0:MM/dd/yyy}", collection["ngaysinh"]);
            if (!ModelState.IsValid)
            {
                return View("DangKy");
            }
            else
            {
                var a = from b in db.khachhangs where b.emailkh == email select b;
                var c = from d in db.khachhangs where d.tentkkh == tentk select d;
                if (a.Count()!=0)
                {
                    ViewData["Loi1"] = "Email đã tồn tại";
                    
                }
                else if (c.Count()!=0)
                {
                    ViewData["Loi2"] = "Tên tài khoản đã tồn tại";
                }
                else
                {
                    khachhang.emailkh = email;
                    khachhang.tenkh = tenkh;
                    khachhang.sdtkh = sdt;
                    khachhang.diachikh = diachi;
                    khachhang.tentkkh = tentk;
                    khachhang.matkhau = pass;
                    khachhang.matkhaukh = repass;
                    khachhang.ngaysinh = DateTime.Parse(ngaysinh);
                    db.khachhangs.InsertOnSubmit(khachhang);
                    db.SubmitChanges();
                    return RedirectToAction("DangNhap");
                }
                
            } 
            return View("DangKy");
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["pass"];
            if (!ModelState.IsValid)
            {
                return View("DangNhap");
            }
            else
            {
                khachhang kh = db.khachhangs.SingleOrDefault(n => n.tentkkh == tendn && n.matkhaukh== matkhau);
                if (kh != null)
                {
                    bool a = true;
                    ViewBag.ThongBao = "Chúc Mừng Bạn Đăng Nhập Thành Công";
                    Session["tentkkh"] = kh;
                    Session["makh"] = kh.makh;
                    FormsAuthentication.SetAuthCookie(tendn, a);
                    return RedirectToAction("Index", "TrangChu");
                }
                else
                    ViewBag.ThongBao = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng";
            }
            return View();
        }
        public ActionResult TenKhiLogin()
        {
            if (Session["tentkkh"] !=null)
            {
                return PartialView("TenDN", Session["tentkkh"]);
            }
            else
            {
                return PartialView("BtnDangNhap");
            }
        }
        
        public ActionResult DangXuat()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "TrangChu");
        }
        [Authorize]
        public ActionResult DaLuu(string id,luusp luusp,string sURL)
        {
            luusp luusp1 = db.luusps.SingleOrDefault(n => n.masp==id);
            if (luusp1!=null)
            {
                return Redirect(sURL);
            }
            else
            {
                var masp = id;
                var makh =int.Parse (Session["makh"].ToString());
                luusp.masp = masp;
                luusp.makh =makh;
                db.luusps.InsertOnSubmit(luusp);
                db.SubmitChanges();
                return Redirect(sURL);
            }
        }
        [Authorize]
        public ActionResult XemDaLuu()
        {
            List<luusp> luusps = new List<luusp>(from b in db.luusps select b);
            var a = luusps.FindAll(m => m.makh == int.Parse(Session["makh"].ToString()));
            return View(a);
        }
        [Authorize]
        public ActionResult XoaLuu(int id)
        {
            luusp luusp = db.luusps.SingleOrDefault(n=>n.id==id);
            db.luusps.DeleteOnSubmit(luusp);
            db.SubmitChanges();
            return RedirectToAction("XemDaLuu");
        }
        //[HttpPost]
        //public ActionResult phanhoi(FormCollection collection, phanhoi PH)
        //{
        //    var Hoten = collection["Hoten"];
        //    var Email = collection["Email"];
        //    var tinnhan = collection["tinnhan"];
        //    if (String.IsNullOrEmpty(Hoten))
        //    {
        //        ViewData["Loi1"] = "Họ tên khách hàng không được bỏ trống !";
        //    }
        //    else if (String.IsNullOrEmpty(Email))
        //    {
        //        ViewData["Loi2"] = "Email không được bỏ trống !";
        //    }
        //    else if (String.IsNullOrEmpty(tinnhan))
        //    {
        //        ViewData["Loi3"] = "Nội dung không được bỏ trống !";
        //    }
        //    else
        //    {
        //        PH.hoten = Hoten;
        //        PH.mail = Email;
        //        PH.noidungmail = tinnhan;
        //        db.phanhois.InsertOnSubmit(PH);
        //        db.SubmitChanges();
        //        return RedirectToAction("LienHe", "TrangChu");

        //    }
        //    return this.phanhoi();
        //}
    }

}
