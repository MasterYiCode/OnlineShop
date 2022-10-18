using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContentDao
    {
        private OnlineShopDbContext context = null;

        public ContentDao()
        {
            context = new OnlineShopDbContext();
        }

        public Content GetById(long id)
        {
            return context.Contents.Find(id);
        }
    }
}
