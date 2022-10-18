using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserDao
    {
        private OnlineShopDbContext context = null;

        public UserDao()
        {
            context = new OnlineShopDbContext();
        }

        // Insert User
        public void Insert(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        // Update User
        public void Update(User user)
        {
            var u = context.Users.Find(user.ID);
            u.Name = user.Name;
            u.Email = user.Email;
            if (!String.IsNullOrEmpty(user.Password))
            {
                u.Password = user.Password;

            }
            u.DateOfBirth = user.DateOfBirth;
            u.Address = user.Address;
            u.Phone = user.Phone;
            u.ModifiedDate = user.ModifiedDate;
            u.ModifiedBy = user.ModifiedBy;
            u.Status = user.Status;
            context.SaveChanges();

        }

        // Delete User
        public void Delete(long id)
        {
            var user = context.Users.Find(id);
            context.Users.Remove(user);
            context.SaveChanges();
        }

        // Get all user
        public List<User> GetAllUser()
        {
            return context.Users.ToList();
        }

        // Get all user
        public IEnumerable<User> GetAllUserPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> model = context.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Email.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        // Get User By Id
        public User GetUserById(long id)
        {
            return context.Users.FirstOrDefault(x => x.ID == id);
        }


        // Get User by Email
        public User GetUserByEmail(string email)
        {
            return context.Users.FirstOrDefault(x => x.Email == email);
        }

        // Get User By ResetPasswordCode
        public User GetUserByResetPasswordCode(string resetPasswordCode)
        {
            return context.Users.FirstOrDefault(x => x.ResetPasswordCode == resetPasswordCode);
        }

        // Find email First Or Default
        public bool FindEmailFirstOrDefault(string email)
        {
            var u = context.Users.FirstOrDefault(x => x.Email == email);
            return u != null;
        }

        //
        public bool ChangeStatus(long id)
        {
            var user = context.Users.Find(id);
            var status = !user.Status;
            user.Status = status;
            context.SaveChanges();
            return status;
        }

        // Verify Account: Xác thực tải khoản
        public bool VerifyAccount(string id)
        {
            // This line I have added here to avoid
            // Confirm password dose not match issue on save changes
            if (string.IsNullOrEmpty(id)) return false; 
            context.Configuration.ValidateOnSaveEnabled = false;

            var user = context.Users.FirstOrDefault(u => u.ActivationCode == new Guid(id));

            if (user != null)
            {
                user.IsEmailVerified = true;
                context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Public Update ResetPasswordCode
        public bool UpdateResetPasswordCode(string email, string resetCode)
        {
            // This line I have added here to avoid confirm password not match issue, as we had added a confirm password property
            // in cur model class in part 1
            context.Configuration.ValidateOnSaveEnabled = false;
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.ResetPasswordCode = resetCode;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdatePassword(string resetPasswordCode, string newPasswordMD5Hash)
        {
            context.Configuration.ValidateOnSaveEnabled = false;
            var user = context.Users.FirstOrDefault(u => u.ResetPasswordCode == resetPasswordCode);
            if (user != null)
            {
                user.Password = newPasswordMD5Hash;
                user.ResetPasswordCode = "";
                context.SaveChanges();
                return true;
            }
            return false;
        }



    }
}
