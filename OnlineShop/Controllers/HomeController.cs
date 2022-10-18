using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().GetAll();
            var productDao = new ProductDao();
            ViewBag.AllProduct = productDao.ListAllProduct(6);// Tất cả sản phẩm
            ViewBag.NewProducts = productDao.ListNewProduct(5); // Sản phẩm mới
            ViewBag.FeatureProducts = productDao.ListFeatureProduct(5); // Sản phẩm hot
            return View();
        }


        [ChildActionOnly] // chỉ gọi như PartialView, không gọi như 1 trang được
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var model = new MenuDao().ListByGroupId(2);
            return PartialView(model);
        }

        [ChildActionOnly] 
        public ActionResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }


    }
}