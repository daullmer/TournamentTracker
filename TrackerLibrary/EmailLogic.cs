using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace TrackerLibrary
{
    public static class EmailLogic
    {
        public static void SendEmail(string toAdresses, string subject, string body)
        {
            MailAddress fromMailAdress = new MailAddress(GlobalConfig.AppKeyLookup("senderEamil"), GlobalConfig.AppKeyLookup("senderName"));

            MailMessage mail = new MailMessage();
            mail.To.Add(toAdresses);
            mail.From = fromMailAdress;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();

            client.Send(mail);

        }
    }
}
