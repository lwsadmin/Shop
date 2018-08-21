using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Dal;
namespace Shop.Bll.GroupBuy
{
    public class RushOrderLogic
    {

        public object GetRushOrderItems(Guid  OrderGuid)
        {
            dbContext db = new dbContext();
            var data = from u in db.TRushBuy.Where(g => g.Guid == OrderGuid)
                       select new
                       {
                           Guid = u.Guid,
 
                           Price = u.Price,
                           Total = u.MarketPrice
                       };
            return data;
        }
    }
}
