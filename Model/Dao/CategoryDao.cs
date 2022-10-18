using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CategoryDao
    {
        private OnlineShopDbContext context = null; 

        public CategoryDao()
        {
            context = new OnlineShopDbContext();
        }

        public List<Category> GetAll()
        {
            return context.Categories.Where(x => x.Status == true).ToList();
        }
    }
}
