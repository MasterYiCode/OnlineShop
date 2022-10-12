using Common;
using Model.Dao;
using Model.EF;
using OnlineShop.Areas.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }

        // Registrationn (Hành động đăng ký)
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        // Registration POST Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(UserModel userModel)
        {
            bool status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                #region Email is already exist: email đã tồn tại
                var isExist = IsEmailExist(userModel.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email đã tồn tại");
                    return View(userModel);
                }
                #endregion

                #region Generate activation code: tạo mã kích hoạt
                var ActivationCode = Guid.NewGuid();
                #endregion

                #region Password MD5Hashing
                userModel.Password = Encryptor.MD5Hash(userModel.Password);
                userModel.ConfirmPassword = Encryptor.MD5Hash(userModel.ConfirmPassword);
                #endregion

                #region Save data to databse
                var dao = new UserDao();
                dao.Insert(new User()
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    Password = userModel.Password,
                    CreatedDate = DateTime.Now,
                    ActivationCode = ActivationCode,
                    IsEmailVerified = false
                });
                #endregion

                #region Send email to user
                SendVerificationLinkEmail(userModel.Email, ActivationCode.ToString());
                #endregion

                message = "Đăng ký tài khoản thành công" +
                    "Liên kết kích hoạt tài khoản đã được gửi đến email của bạn: " +
                    userModel.Email;
            }
            else
            {
                message = "Yêu cầu không hợp lệ.";
            }    
            
            ViewBag.Status = true;
            ViewBag.Message = message;  
            return View(userModel);
        }
        

        // Verify account via email
        public ActionResult VerifyAccount(string id)
        {
            bool status = false;
            var dao = new UserDao();
            var result = dao.VerifyAccount(id);
            if (result)
            {
                status = true;
            }
            else
            {
                ViewBag.Message = "Yêu cầu không hợp lệ";
            }
            ViewBag.Status = status;
            return View();
        }

        // Login Get Action
        [HttpGet]        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var user = dao.GetUserByEmail(login.Email);

                if(user != null)
                {
                    if (!user.IsEmailVerified)
                    {
                        ViewBag.Message = $"Vui lòng xác minh tài khoản của bạn trước khi đăng nhập. Xác minh đã được gửi về gmail: {user.Email}";
                        return View();
                    }

                    if(string.Compare(Encryptor.MD5Hash(login.Password), user.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 Minutes = 365 days
                        var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;

                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "User");
                        }
                    }
                    else
                    {
                        message = "Thông tin đăng nhập không hợp lệ.";
                    }
                }
                else
                {
                    message = "Thông tin đăng nhập không hợp lệ.";
                }    
            }
            ViewBag.Message = message;
            return View(login);

        }

        // Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        

        // Email đã tồn tại => return true, ngược lại false
        [NonAction]
        public bool IsEmailExist(string email)
        {
            var dao = new UserDao();
            return dao.FindEmailFirstOrDefault(email);
        }


        // Send Verification Link Email: gửi liên kết xác minh về email người dùng
        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Admin/Account/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("dochihung492002@gmail.com", "Đỗ Chí Hùng");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "Cogangleniy1";

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Tài khoản của bạn đã được tạo thành công!";
                body = "<br/><br/>Chúng tôi vui mừng thông báo với bạn rằng tài khoản OnlineSHop" +
                           "của bạn đã tạo thành công. Vui lòng nhập vào liên kết dưới để xác minh tài khoản của bạn." +
                           " <br/><br/><a href='" + link + "'>" + link + "</a>";

            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Yêu cầu đặt lại mật khẩu!";
                body = "Hi,<br/><br/>Chúng totoi đã nhận được yêu cầu đặt lại mật khẩu tài khoản của bạn. Vui lòng nhấp vào liên kết dưới để đặt lại mật khẩu của bạn. " +
                           " <br/><br/><a href='" + link + "'>Liên kết đặt lại mật khẩu.</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
            {
                //smtp.Send(message);
            };
        }

        // Forgot password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            // Verify Email: kiểm chứng email
            // Generate Reset password link: Tạo liên kết đặt lại mật khẩu
            // Send: gửi link về Email
            string message = "";
            bool status = false;

            var dao = new UserDao();
            var user = dao.GetUserByEmail(email);
            if (user != null)
            {
                // Send email for reset password
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(user.Email, resetCode, "ResetPassword");
                dao.UpdateResetPasswordCode(user.Email, resetCode);
                status = true;
                message = "Link đặt lại mật khẩu đã được gửi về gmail bạn. Vui lòng vào gmail kiểm tra tin nhắn.";
            }
            else
            {
                message = "Tài khoán không hợp lệ.";
            }

            ViewBag.Status = status;
            ViewBag.Message = message;

            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            // verify the reset password link
            // find account associated with this link 
            // redirect to reset password page
            var dao = new UserDao();
            var user = dao.GetUserByResetPasswordCode(id);
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";


            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                string newPasswordMD5Hash = Encryptor.MD5Hash(model.NewPassword);
                bool result = dao.UpdatePassword(model.ResetCode, newPasswordMD5Hash);
                if (result)
                {
                    message = "Cập nhập mật khẩu thành công";
                }
            }
            else
            {
                message = "Một cái gì đó không hợp lệ rùi?";
            }

            ViewBag.Message = message;
            return View();
        }
    }

}
