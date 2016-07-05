using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Evis.VMS.UI.HelperClasses
{
    public class EmailHelper
    {
        public static void SendMail(string sendTo, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage();
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["SMTP_USERNAME"];//"infoafocdubai@gmail.com";
                NetworkCred.Password = ConfigurationManager.AppSettings["SMTP_PASSWORD"];//"welcomeafoc";//Here we will create object of MailMessage class.
                message.From = new MailAddress(NetworkCred.UserName);       //Initilize From in mail address.
                message.To.Add(new MailAddress(sendTo));        //Initilize To in mail address.
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["SMTP_HOST"]; //"smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);//25;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                message = null;

                smtp = null;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}