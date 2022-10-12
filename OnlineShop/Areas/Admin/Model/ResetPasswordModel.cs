using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Model
{
    public class ResetPasswordModel
    {
        [Display(Name = "Mật khẩu mới")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không hợp lệ.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất phải 6 ký tự.")]
        [MaxLength(100, ErrorMessage = "Mật khẩu quá dài, khó nhớ.")]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không trùng nhau.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }

    }
}