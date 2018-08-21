using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
namespace Shop.ViewModel
{
    public class GroupBuyImage
    {
        public TRushBuy GroupBuy { get; set; }
        public List<TImage> ImageList { get; set; }
    }
}
