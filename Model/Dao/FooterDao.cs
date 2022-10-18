using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class FooterDao
    {
        private OnlineShopDbContext context = null;
        public FooterDao()
        {
            context = new OnlineShopDbContext();
        }
        public Footer GetFooter()
        {
            return context.Footers.FirstOrDefault(x => x.Status == true);
        }
    }
}
