using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Dal.Emnu
{
    public enum MemberSource
    {
        门店登记 = 0,
        微信注册 = 1,
        网站注册 = 2,
        批量导入 = 3,
        好友推荐 = 4
    }
}
