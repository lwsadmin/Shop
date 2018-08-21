using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Shop.Bll;
using Shop.AdminWeb.Controllers;
namespace AdminWeb.Areas.Common.Controllers
{
    public class CommonController : AdminBaseController
    {
        // GET: Common/Common
        public JsonResult Upload()
        {
            HttpPostedFileBase fileBase = Request.Files["File"];
            string url = "";

            if (fileBase == null || fileBase.FileName == "")
            {
                return Json(new { success = false, url = "请选择要上传的图片!" });
            }
            if (fileBase.ContentLength >= 1024 * 1000)
            {
                return Json(new { success = false, url = "图片大小不能超过1M!" });
            }
            string ext = fileBase.FileName.Substring(fileBase.FileName.LastIndexOf('.'));
            if (ext.ToLower() != ".jpg" && ext.ToLower() != ".jpeg" && ext.ToLower() != ".gif" && ext.ToLower() != ".png" && ext.ToLower() != ".bmp")
            {
                return Json(new { success = false, url = "图片文件格式不正确!" });
            }
            bool f = CommonLogic.SaveSingleImg(fileBase, this.OperatorGuid, out url);
            return Json(new { success = f, url = url, size = fileBase.ContentLength, name = fileBase.FileName }, "text/html");
        }



        public JsonResult ImageDelete(Guid id)
        {
            ImagesLogic logic = new ImagesLogic();
            bool success = logic.DeleteImages(id.ToString());
            return Json(new { success = success });
        }
        public JsonResult UploadedImageDelete(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = Request.Form["url"];
            }
            CommonLogic.DeleteImages(url);
            return Json(new { success = true });
        }
        public JsonResult ReadCard(string cardId)
        {
            //  string cardId = Request.Form["cardId"];

            var data = MemberLogic.GetMemberInfoByCardIdOrMobile(cardId, this.OperatorGuid);
            return Json(data);
        }

        //public JsonResult SaveParameter()
        //{

        //    foreach (var key in Request.Form.AllKeys)
        //    {
        //        if (!Request.Form[key].Contains("*"))
        //            ParameterLogic.SaveParameter(key, Request.Form[key].Trim(), this.OperatorGuid);
        //    }
        //    //new LogLogic().WriteLog(this.UserAccount, this.ChainStoreGuid, "保存了微信设置");
        //    return Json(new { success = true, message = "保存成功!" });
        //}
    }
}