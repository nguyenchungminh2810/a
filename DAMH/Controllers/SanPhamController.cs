using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAMH.Models;
using PagedList;

namespace DAMH.Controllers
{
    
    public class SanPhamController : Controller
    {
        GiayDataContext data = new GiayDataContext();
        // GET: SanPham
        public ActionResult AllGiay(int ? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var AllGiay = from a in data.sanphams  select a;
            return View(AllGiay.ToPagedList(pageNumber,pageSize));
        }
        public ActionResult ChiTietGiay(string id)
        {
            var chitietgiay = from c in data.sanphams where c.masp == id select c;
            return View(chitietgiay.SingleOrDefault());
        }
    }
}