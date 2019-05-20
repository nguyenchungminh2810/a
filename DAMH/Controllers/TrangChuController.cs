using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DAMH.Models;
using PagedList;

namespace DAMH.Controllers
{
    public class TrangChuController : Controller
    {

        GiayDataContext data = new GiayDataContext();
        private List<sanpham> LaySPMoi(int count)
        {
            return data.sanphams.OrderByDescending(a => a.ngaynhap).Take(count).ToList();
        }
        // GET: TrangChu
        public ActionResult Index()
        {
            if (Session["tentkkh"]==null)
            {
                FormsAuthentication.SignOut();
            }
            var spmoi = LaySPMoi(8);
            return View(spmoi);
        }
        public ActionResult phanhoi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult phanhoi(FormCollection collection, phanhoi PH)
        {
            var Hoten = collection["Hoten"];
            var Email = collection["Email"];
            var tinnhan = collection["tinnhan"];
            if (String.IsNullOrEmpty(Hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được bỏ trống !";
            }
            else if (String.IsNullOrEmpty(Email))
            {
                ViewData["Loi2"] = "Email không được bỏ trống !";
            }
            else if (String.IsNullOrEmpty(tinnhan))
            {
                ViewData["Loi3"] = "Nội dung không được bỏ trống !";
            }
            else
            {
                PH.hoten = Hoten;
                PH.mail = Email;
                PH.noidungmail = tinnhan;
                data.phanhois.InsertOnSubmit(PH);
                data.SubmitChanges();
                return RedirectToAction("phanhoi", "TrangChu");

            }
            return this.phanhoi();
        }
        public ActionResult HieuGiay()
        {
            var a = from b in data.thuonghieus select b;
            return PartialView(a);
        }
        public ActionResult SPtheothuonghieu(int id, int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var SPtheothuonghieu = from t in data.sanphams where t.mathuonghieu == id select t;
            return View(SPtheothuonghieu.ToPagedList(pageNumber, pageSize));
        }

    }
}