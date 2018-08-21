using Shop.Bll;
using Shop.Entity;
using Shop.Help;
using Shop.Help.ValidateCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Controllers
{
    public class ShopController : Controller
    {


        [HttpGet]
        public ActionResult Login()
        {
            ViewData["msg"] = "";
            return View();
        }
        public ActionResult Index()
        {
            string mangerCooike = HttpUtility.UrlDecode(Safe.GetCookie(Config.GetWebConfigValueByKey("LoginInfo")));
            if (string.IsNullOrEmpty(mangerCooike))
            {
                return Redirect("/shop/loginout");
            }
            TManager model = ManagerLogic.GetLoginInfo();
            if (model == null)
            {
                return Redirect("/shop/loginout");
            }
            //    ManagerLogic.Add(model);

            TOperator Operator = OperatorLogic.GetEntity(model.OperatorGuid);
            TChainStore ChainStore = ChainStoreLogic.GetEntity(model.ChainStoreGuid);
            ViewData["OperatorName"] = Operator.Name;
            ViewData["ChainStoreName"] = ChainStore.Name;
            return View(model);
        }

        public ActionResult LoginOut()
        {
            CommonLogic.ManagerLoginOut();
            return Redirect("/shop/login");
        }

        public ActionResult YanZhengMa()
        {
            ValidateCode_Style8 sc = new ValidateCode_Style8();

            Session["vcode"] = null;
            string code = string.Empty;
            byte[] bytes = sc.CreateImage(out code);
            Safe.SessionAdd("vcode", code.ToLower(), 2);   //Session中保存验证码
            return File(bytes, @"image/jpeg");
        }
        [HttpPost]
        public ActionResult Login(string loginAccount, string loginName, string pwd, string yzm)
        {
            string msg = string.Empty;
            ViewData["loginAccount"] = loginAccount;
            ViewData["loginName"] = loginName;
            ViewData["pwd"] = pwd;
            if (Session["vcode"] == null)
            {
                msg = "验证码错误!";
                ViewData["msg"] = "<script type=\"text/javascript\">layer.msg(\"验证码丢失!\");$(\"#yzm\").focus();</script>";
                return View();
            }
            if (!Session["vcode"].ToString().Equals(yzm))
            {
                msg = "验证码错误!";
                //ViewData["msg"] = "layer.msg(\"验证码错误!\");";
                ViewData["msg"] = "<script type=\"text/javascript\">layer.msg(\"验证码错误!\");$(\"#yzm\").focus();</script>";
                return View();
            }
            bool flag = CommonLogic.Login(loginAccount, loginName, pwd, out msg);
            ViewData["msg"] = "<script type=\"text/javascript\">layer.msg(\"" + msg + "\");$(\"#yzm\").focus();</script>";
            if (flag)
            {
                LogLogic.WriteLog("登录系统");
                return Redirect("/shop/index");
            }
            else
            { return View(); }
        }
    }
}