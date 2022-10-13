using Common;
using Model.Dao;
using Model.EF;
using OnlineShop.Areas.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        //[Authorize]        
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            var model = dao.GetAllUserPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Create(UserVM model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                dao.Insert(new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = Encryptor.MD5Hash(model.Password),
                    ActivationCode = Guid.NewGuid(),
                    DateOfBirth = model.DateOfBirth,
                    Address = model.Address,
                    Phone = model.Phone,
                    Status = model.Status,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "dochihung492002@gmail.com"
                });

                return RedirectToAction("Index", "User");
            }
            else
            {
                ModelState.AddModelError("", "Thêm người dùng thất bại");
            }    
            return View(model);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var u = new UserDao().GetUserById(id);
            var userEditVM = new UserEditVM()
            {
                ID = id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password,
                DateOfBirth = u.DateOfBirth,
                Address = u.Address,
                Phone = u.Phone,
                Status = u.Status
            };
            return View(userEditVM);
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Edit(UserEditVM userEditVM)
        {
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(userEditVM.Password))
                {
                    userEditVM.Password = Encryptor.MD5Hash(userEditVM.Password);
                }

                dao.Update(new User()
                {
                    ID = userEditVM.ID,
                    Name = userEditVM.Name,
                    Email = userEditVM.Email,
                    Password = userEditVM.Password,
                    DateOfBirth = userEditVM.DateOfBirth,
                    Address = userEditVM.Address,
                    Phone = userEditVM.Phone,
                    Status = userEditVM.Status,
                    ModifiedBy = "dochihung492002@gmail.com",
                    ModifiedDate = DateTime.Now
                });
                return RedirectToAction("Index", "User");
                
            }
            else
            {
                ModelState.AddModelError("", "Cập nhập người dùng thất bại");
            }    
            return View(userEditVM);
        }

        //[Authorize]
        public ActionResult Delete(long id)
        {
            new UserDao().Delete(id);
            return RedirectToAction("Index", "User");
        }

        [NonAction]
        public void GetUserCookie()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if(authCookie != null)
            {
                var ticketInfo = FormsAuthentication.Decrypt(authCookie.Value);
            }

        }
    }
}