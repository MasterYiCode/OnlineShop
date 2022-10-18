using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class MenuDao
    {
        private OnlineShopDbContext context = null;

        public MenuDao()
        {
            context = new OnlineShopDbContext();
        }

        public List<Menu> ListByGroupId(int groupID)
        {
            return context.Menus.Where(x => x.TypeID == groupID && x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }


    }
}
