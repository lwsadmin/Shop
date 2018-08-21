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
    public class StoreCapitalSettlementLogic
    {

        //门店积分充值
        public static bool StoreRechargePoint(TChainStorePointNote note, TManager manger, out string message)
        {
            message = string.Empty;
            DalGeneric<TBusiness> dal = new DalGeneric<TBusiness>();
            TBusiness buiness = dal.GetBy(c => c.Guid == manger.BusinessGuid);
            if (!(bool)buiness.IsSystemBusiness)//总部
            {
                message = "对不起,您不能进行该操作!";
                return false;
            }
            if (note.MoneyFactPay > note.MoneyPayable)
            {
                message = "应收金额有误,请核实金额后重新操作!";
                return false;
            }
            note.BillNumber = CommonLogic.GetBillNumber(BillTypes.店面购买积分);
            note.Way = (int)EnumStorePointType.购买积分;
            dbContext db = new dbContext();
            TChainStore store = db.TChainStore.Where(c => c.Guid == note.ChainStoreGuid).FirstOrDefault();
            DbContextTransaction dbContextTransaction = db.Database.BeginTransaction();

            try
            {
                store.AvailablePoint += note.Point;
                store.TotalPoint += note.Point;
                if (note.MoneyPayable > note.MoneyFactPay)
                {
                    TChainStoreCapitalNote CapitalNote = new TChainStoreCapitalNote();
                    CapitalNote.BusinessGuid = store.BusinessGuid;
                    CapitalNote.ChainStoreGuid = note.ChainStoreGuid;
                    CapitalNote.OperatorGuid = note.OperatorGuid;

                    CapitalNote.Value = note.MoneyFactPay - note.MoneyPayable;
                    CapitalNote.AllValue = CapitalNote.AllValue + (note.MoneyFactPay - note.MoneyPayable);
                    CapitalNote.Type = 1;//1 支出 2 收入
                    CapitalNote.Way = (int)CapitalType.门店充积分;
                    CapitalNote.Memo = note.Memo;
                    CapitalNote.OperatorTime = DateTime.Now;
                    CapitalNote.UserAccount = note.UserAccount;
                    CapitalNote.BillNumber = note.BillNumber;
                    CapitalNote.Guid = Guid.NewGuid();

                    db.TChainStoreCapitalNote.Add(CapitalNote);
                }
                store.SettlementMoney += (note.MoneyFactPay - note.MoneyPayable);

                note.OperatePoint = store.AvailablePoint;
                note.CreateTime = DateTime.Now;
                note.Guid = Guid.NewGuid();
                db.TChainStorePointNote.Add(note);
                db.SaveChanges();
                message = "积分充值成功!";
                dbContextTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                LogLogic.WriteErrorLog(e.StackTrace + e.Message);
                dbContextTransaction.Rollback();
                message = e.Message;
                return false;
            }
        }

        //充余额
        public static bool StoreRechargeValue(TChainStoreValueNote ValueNote, TManager manger, out string message)
        {

            message = "充值成功!";
            dbContext db = new dbContext();
            if (ValueNote.PayValue < 0 || ValueNote.Value <= 0)
            {
                message = "对不起,您输入的充值金额不正确,请核实!";
                return false;
            }
            TBusiness buiness = db.TBusiness.Where(c => c.Guid == manger.BusinessGuid).FirstOrDefault();
            if (!(bool)buiness.IsSystemBusiness)
            {
                message = "对不起,您不能进行该操作!";  //非总部用户不允许对其他门店充值!
                return false;
            }
            DbContextTransaction tran = db.Database.BeginTransaction();
            try
            {
                TChainStore store = db.TChainStore.Where(c => c.Guid == ValueNote.ChainStoreGuid).FirstOrDefault();
                store.TotalValue += ValueNote.Value;
                if (ValueNote.Type == 0)
                {
                    store.AvailableValue = store.AvailableValue + ValueNote.Value;
                    ValueNote.DoneValue = store.AvailableValue;
                }
                else
                {
                    store.AvailableValue = store.AvailableValue - ValueNote.Value;
                    ValueNote.DoneValue = store.AvailableValue - ValueNote.Value;
                }
                ValueNote.OperatorGuid = manger.OperatorGuid;
                ValueNote.Guid = Guid.NewGuid();
                ValueNote.OperateTime = DateTime.Now;
                ValueNote.UserAccount = manger.LoginName;
                ValueNote.BillNumber = CommonLogic.GetBillNumber(BillTypes.店面充值);
                db.TChainStoreValueNote.Add(ValueNote);
                TChainStoreCapitalNote CapitalNote = new TChainStoreCapitalNote();
                if (ValueNote.PayValue != ValueNote.Value)//插入一条结算数据
                {
                    //  CapitalNote.Type = 1;//1 支出 2 收入
                    CapitalNote.Way = (int)CapitalType.门店充值;
                    if (ValueNote.PayValue > ValueNote.Value)
                        CapitalNote.Type = 2;//应收
                    else
                        CapitalNote.Type = 1;//应收

                    CapitalNote.BusinessGuid = store.BusinessGuid;
                    CapitalNote.ChainStoreGuid = ValueNote.ChainStoreGuid;
                    CapitalNote.OperatorGuid = ValueNote.OperatorGuid;

                    CapitalNote.Value = ValueNote.PayValue - ValueNote.Value;
                    CapitalNote.AllValue = CapitalNote.AllValue + CapitalNote.Value;
                    CapitalNote.Memo = ValueNote.Memo;
                    CapitalNote.OperatorTime = DateTime.Now;
                    CapitalNote.UserAccount = ValueNote.UserAccount;
                    CapitalNote.BillNumber = ValueNote.BillNumber;
                    CapitalNote.Guid = Guid.NewGuid();
                    db.TChainStoreCapitalNote.Add(CapitalNote);
                }
                if (db.GetValidationErrors().Count() == 0)
                {
                    db.SaveChanges();
                    tran.Commit();
                    return true;
                }
                else
                {
                    message = db.GetValidationErrors().ElementAt(0).ValidationErrors.First().ErrorMessage;
                    tran.Rollback();
                    return false;
                }
            }
            catch (DbEntityValidationException dbEx)
            {

                message = dbEx.Message;
                tran.Rollback();

                return false;
            }
        }


        //充短信

        public static bool StoreBuySms(TChainStoreBuySmsNote storeSmsNote, TManager manger, out string message)
        {
            message = "短信充值成功!";
            DalGeneric<TBusiness> dal = new DalGeneric<TBusiness>();
            TBusiness buiness = dal.GetBy(c => c.Guid == manger.BusinessGuid);
            if (!(bool)buiness.IsSystemBusiness)//总部
            {
                message = "对不起,您不能进行该操作!";
                return false;
            }
            if (storeSmsNote.ShouldPay < storeSmsNote.FactPay)
            {
                message = "应付金额错误!";
                return false;
            }
            message = "操作成功!";
            dbContext db = new dbContext();
            DbContextTransaction tran = db.Database.BeginTransaction();


            TChainStore store = db.TChainStore.Where(c => c.Guid == storeSmsNote.ChainStoreGuid).FirstOrDefault();

            //TOperator opertor = db.TOperator.Single(o => o.Guid == store.OperatorGuid);
            //if (storeSmsNote.Count > opertor.AvaiableSmsCount)
            //{
            //    message = string.Format("无法充值，当前可充值短信{0}条", opertor.AvaiableSmsCount);
            //    return false;
            //}
            try
            {
                store.TotalSmsCount += storeSmsNote.Count;
                store.AvailableSmsCount += storeSmsNote.Count;

                storeSmsNote.Guid = Guid.NewGuid();
                storeSmsNote.OperateTime = DateTime.Now;
                storeSmsNote.BillNumber = CommonLogic.GetBillNumber(BillTypes.购买短信);
                db.TChainStoreBuySmsNote.Add(storeSmsNote);
                if (storeSmsNote.ShouldPay > storeSmsNote.FactPay)//产生结算
                {
                    TChainStoreCapitalNote CapitalNote = new TChainStoreCapitalNote();

                    //  CapitalNote.Type = 1;//1 支出 2 收入
                    CapitalNote.Way = (int)CapitalType.短信充值;
                    CapitalNote.Type = 2;//应收

                    CapitalNote.BusinessGuid = store.BusinessGuid;
                    CapitalNote.ChainStoreGuid = storeSmsNote.ChainStoreGuid;
                    CapitalNote.OperatorGuid = storeSmsNote.OperatorGuid;

                    CapitalNote.Value = storeSmsNote.ShouldPay - storeSmsNote.FactPay;

                    store.SettlementMoney += CapitalNote.Value;

                    CapitalNote.AllValue = (CapitalNote.AllValue + (storeSmsNote.ShouldPay - storeSmsNote.FactPay));
                    CapitalNote.Memo = storeSmsNote.Memo;
                    CapitalNote.OperatorTime = DateTime.Now;
                    CapitalNote.UserAccount = storeSmsNote.UserAccount;
                    CapitalNote.BillNumber = storeSmsNote.BillNumber;
                    CapitalNote.Guid = Guid.NewGuid();
                    CapitalNote.Memo = storeSmsNote.Memo;
                    db.TChainStoreCapitalNote.Add(CapitalNote);
                }
                if (db.GetValidationErrors().Count() == 0)
                {
                    db.SaveChanges();
                    tran.Commit();
                    return true;
                }
                else
                {
                    message = db.GetValidationErrors().ElementAt(0).ValidationErrors.First().ErrorMessage;
                    tran.Rollback();
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                LogLogic.WriteLog(e.Message + e.StackTrace);
                message = e.Message;
                return false;
                throw;
            }
        }

        public static bool ChainStoreAddValue()
        {
            return true;
        }
    }
}
