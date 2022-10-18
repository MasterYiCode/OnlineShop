using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly] // chỉ cho PartialView gọi
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().GetAll();
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Category(long cateId)
        {
            var category = new ProductCategoryDao().ViewDetail(cateId);
            return View(category);
        }

        [HttpGet]
        public ActionResult Detail(long id)
        {
            var dao = new ProductDao();
            var product = dao.Detail(id);
            ViewBag.RelatedProducts = dao.ListRelatedProduct(id ,6);
            return View(product);
        }
    }
}