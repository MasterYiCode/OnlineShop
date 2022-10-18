using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        private OnlineShopDbContext context = null;
        public ProductCategoryDao()
        {
            context = new OnlineShopDbContext();
        }

        public List<ProductCategory> GetAll()
        {
            return context.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public ProductCategory ViewDetail(long id)
        {
            return context.ProductCategories.Find(id);
        }
    }
}
