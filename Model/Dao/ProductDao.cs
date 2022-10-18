using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        private OnlineShopDbContext context = null;
        public ProductDao()
        {
            context = new OnlineShopDbContext();
        }

        public Product Detail(long id)
        {
            return context.Products.Find(id);
        }
        public List<Product> ListAllProduct(int top)
        {
            return context.Products.OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<Product> ListRelatedProduct(long productId, int top)
        {
            var product = context.Products.Find(productId);
            return context.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).Take(top).ToList();
        }
        public List<Product> ListNewProduct(int top)
        {
            return context.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return context.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).ToList();
        }
    }
}
