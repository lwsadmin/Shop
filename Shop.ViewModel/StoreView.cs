using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
namespace Shop.ViewModel
{
    public class StoreView
    {
        public TChainStore store { get; set; }

        public List<TImage> storeImges { get; set; }
    }
}
