using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
namespace Shop.Bll
{
    public class SmsSendLogic
    {
        public static bool Send(string Content, string Sender, out string msg)
        {

            MailMessage email = new MailMessage();
            string[] SenderArrary = Sender.Split(',');
            email.To.Add("919683350@qq.com");
            for (int i = 0; i < SenderArrary.Length; i++)
            {
                //email.To.Add(SenderArrary[i]);

            }

        //    email.From = new MailAddress("a@a.com", "AlphaWu", System.Text.Encoding.UTF8);
            email.From = new MailAddress("wenshang.cool@163.com", "lws", System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            email.Subject = "lws商户联盟消息通知";//邮件标题  
            email.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码  
            email.Body = Content;//邮件内容  
            email.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码  
            email.IsBodyHtml = true;//是否是HTML邮件  
            email.Priority = MailPriority.High;//邮件优先级 

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            client.Host = "smtp.163.com"; ;//指定SMTP服务器
            client.Credentials = new System.Net.NetworkCredential("wenshang.cool@163.com", "919683350ACBEF");//用户名和密码
            object userState = Content;
            try
            {
                client.Send(email);
                // client.SendAsync(email, userState);
                //简单一点儿可以client.Send(msg);  
                msg = "邮件发送成功!";
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                msg = ex.Message;
                return false;

            }
        }
    }
}
