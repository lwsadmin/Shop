<%@ WebHandler Language="C#" Class="Upload" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;
using Shop.Bll;
public class Upload : IHttpHandler
{
    private HttpContext context;

    public void ProcessRequest(HttpContext context)
    {
        String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

        //文件保存目录路径
        String savePath = "../../../Upload/";

        //文件保存目录URL
        String saveUrl = aspxUrl + "../../../Upload/";

        //定义允许上传的文件扩展名
        Hashtable extTable = new Hashtable();
        extTable.Add("image", "gif,jpg,jpeg,png,bmp");
        extTable.Add("flash", "swf,flv");
        extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
        extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

        this.context = context;

        HttpPostedFile imgFile = context.Request.Files["imgFile"];
        Hashtable hash = new Hashtable();

        if (context.Request["type"] != "bs")//不是fileInput上传
        {
            hash["error"] = 0;
        }
        else
        {
            HttpFileCollection Files = HttpContext.Current.Request.Files;
            int count = Files.Count;
            //该集合是所有fileupload文件的集合。
        }
        //hash["error"] = 0;
        string url = "";
        HttpPostedFileBase fileBase = new HttpPostedFileWrapper(imgFile) as HttpPostedFileBase;
        CommonLogic.SaveSingleImg(fileBase, Guid.Empty, out url);
        String fileUrl = url;


        hash["url"] = fileUrl;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        // context.Response.Write(new  { url=url});
        context.Response.End();
    }

    private void showError(string message)
    {
        Hashtable hash = new Hashtable();
        hash["error"] = 1;
        hash["message"] = message;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonMapper.ToJson(hash));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
