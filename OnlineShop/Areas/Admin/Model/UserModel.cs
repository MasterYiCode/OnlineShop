using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Model
{
    public class UserModel
    {
        [Display(Name = "Tên người dùng")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên người dùng không hợp lệ.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không hợp lệ.")]
        [DataType(DataType.EmailAddress)]
        [StringLength(254)]
        public string Email { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không hợp lệ.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password phải có tối thiểu 6 ký tự.")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu và mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

    }

    public class UserVM : UserModel
    {
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} không hợp lệ.")]
        [Display(Name = "Địa chỉ")]
        [StringLength(250)]
        public string Address { get; set; }

        [Phone(ErrorMessage = "{0} không hợp lệ.")]
        [StringLength(10, ErrorMessage = "{0} phải có 10 số.", MinimumLength = 10)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
    }
    public class UserEditVM : UserVM
    {
        public long ID { get; set; }
    }
}