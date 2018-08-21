using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
namespace Shop.ViewModel
{
    public class GoodsModel
    {
        public TGoods goods { get; set; }

        public List<TImage> goodsImges { get; set; }
    }
}
