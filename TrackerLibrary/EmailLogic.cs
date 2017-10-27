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
        public static void SendEmail(List<string> toAdresses, List<string> bcc, string subject, string body)
        {
            MailAddress fromMailAdress = new MailAddress(GlobalConfig.AppKeyLookup("senderEamil"), GlobalConfig.AppKeyLookup("senderName"));

            MailMessage mail = new MailMessage();
            foreach (string email in toAdresses)
            {
                mail.To.Add(email);
            }
            foreach (string email in bcc)
            {
                mail.Bcc.Add(email);
            }
            mail.From = fromMailAdress;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();

            client.Send(mail);

        }

        public static void SendEmail(string toAdresses, string subject, string body)
        {
            SendEmail(new List<string> { toAdresses }, new List<string>(), subject, body);
        }

    }
}
