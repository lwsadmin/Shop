using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Dal;
using Shop.Entity;
using Shop.Dal.Emnu;
using System.Data.Entity;
using System.Data.Entity.Validation;
namespace Shop.Bll
{
    public class StoreCapitalSetLogic
    {

        public static List<TChainStoreSettleSet> GetStoreSettleSet(Guid ChainStoreGuid)
        {
            dbContext db = new dbContext();

            return db.TChainStoreSettleSet.Where(c => c.ChainStoreGuid == ChainStoreGuid).ToList();
        }


        public static bool SaveSet(TChainStoreSettleSet set)
        {
            dbContext db = new dbContext();

            TChainStoreSettleSet OldSet = db.TChainStoreSettleSet.Where(c => c.MemberCategoryGuid == set.MemberCategoryGuid
              && c.ChainStoreGuid == set.ChainStoreGuid).FirstOrDefault();
            if (OldSet == null)
            {
                set.Guid = Guid.NewGuid();
                db.TChainStoreSettleSet.Add(set);
                db.SaveChanges();
                return true;
            }

            OldSet.OffLineConsumeDiscount = set.OffLineConsumeDiscount;
            OldSet.OffLineConsumeRebate = set.OffLineConsumeRebate;
            OldSet.OnLineConsumeRebate = set.OnLineConsumeRebate;
            OldSet.PointPaidRebate = set.PointPaidRebate;
            OldSet.BusinessProfitProportion = set.BusinessProfitProportion;
            OldSet.MemberProfitProportion = set.MemberProfitProportion;

            db.SaveChanges();
            return true;
        }
    }
}
