using System.Net;
using System.Net.Mail;
using System.Text;

namespace TriChem.Helpers.Utilities
{
    public class MailManager
    {        
        public static void Send(string EMailFrom, string EMailTo, string MailSubject, string MailBody)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(EMailFrom);

            smtpClient.Host = "mail.trichem-eg.com";
            smtpClient.Port = 25;
            //smtpClient.Port = 465; Secure            
            smtpClient.Credentials = new NetworkCredential("info@trichem-eg.com", "P@ssw0rd");

            message.From = fromAddress;
            message.To.Add(EMailTo);
            message.Subject = MailSubject;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.BodyEncoding = Encoding.GetEncoding("windows-1256");
            message.Body = MailBody;

            smtpClient.Send(message);
        }
        public static void SendToMailList(string Credential, string EMailFrom, string MailSubject, string MailBody, string[] EMailToList)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress(EMailFrom);

            smtpClient.Host = "mail.trichem-eg.com";
            //smtpClient.Port = 25;
            smtpClient.Credentials = new NetworkCredential(Credential, "987654321");

            message.From = fromAddress;
            message.To.Add(Credential);
            for (int i = 0; i < EMailToList.Length; i++)
            {
                message.Bcc.Add(EMailToList[i]);
            }

            message.Subject = MailSubject;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.BodyEncoding = Encoding.GetEncoding("windows-1256");
            message.Body = MailBody;

            smtpClient.Send(message);
        }
    }
}