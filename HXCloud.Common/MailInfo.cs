using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Common
{
    public class MailInfo
    {
        public static bool sendTheMail(string userName, string pwd, string strfrom, string strto, string subj, string bodys, string smtpserver = "smtp.hichina.com", string smptport = "25")
        {
            SmtpClient _smtpClient = new SmtpClient();
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            _smtpClient.Host = smtpserver;//指定SMTP服务器
                                          //if (YXShop.Common.WebUtility.isNumeric(smptport))
                                          //{
            int port = Convert.ToInt32(smptport);
            if (port > 0)
                _smtpClient.Port = port;
            //}
            _smtpClient.Credentials = new System.Net.NetworkCredential(userName, pwd);//用户名和密码

            MailMessage _mailMessage = new MailMessage(strfrom, strto);
            _mailMessage.Subject = subj;//主题
            _mailMessage.Body = bodys;//内容
            _mailMessage.BodyEncoding = System.Text.Encoding.Default;//正文编码
            _mailMessage.IsBodyHtml = true;//设置为HTML格式
            _mailMessage.Priority = MailPriority.High;//优先级

            try
            {
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private bool SendTEmail(string strto, string subj, string bodys)
        {
            // userReg_Accessor target = new userReg_Accessor(); // TODO: 初始化为适当的值
            string smtpServer = "smtp.163.com"; // TODO: 初始化为适当的值
            string smptport = "25"; // TODO: 初始化为适当的值
            string userName = "**@163.com"; // TODO: 初始化为适当的值
            string pwd = "**"; // TODO: 初始化为适当的值
            string strFrom = "**.com"; // TODO: 初始化为适当的值
            strto = "**.com"; // TODO: 初始化为适当的值
            subj = "hello Miss lu"; // TODO: 初始化为适当的值
            bodys = "----------"; // TODO: 初始化为适当的值
            bool bl = sendTheMail(smtpServer, smptport, userName, pwd, strFrom, strto, subj, bodys);
            return bl;
        }
    }
}
