using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Dal;
using Shop.Entity;
namespace Shop.Bll
{
    public class OperatorLogic
    {
        public static TOperator GetEntity(Guid guid)
        {
            DalGeneric<TOperator> dal = new DalGeneric<TOperator>();
            return dal.GetBy(c => c.Guid == guid);
        }
    }
}
