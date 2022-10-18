using Model.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class SlideDao
    {
        private OnlineShopDbContext context = null;
        public SlideDao()
        {
            context = new OnlineShopDbContext();
        }

        public List<Slide> GetAll()
        {
            return context.Slides.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
    }
}
