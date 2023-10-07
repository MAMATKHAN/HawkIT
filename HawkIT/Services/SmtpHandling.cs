using HawkIT.Models;
using System.Net;
using System.Net.Mail;

namespace HawkIT.Services
{
    public class SmtpHandling
    {
        private SmtpClient? _smtpClient;
        
        public void SendMessage(SenderInfo sender)
        {
            try
            {
                _smtpClient = new SmtpClient("smtp.gmail.com", 587);

                _smtpClient.EnableSsl = true;

                NetworkCredential basicAutheticationInfo = new
                    NetworkCredential("it.hawk.it@gmail.com", "htlqljdecdljjrrp");
                _smtpClient.Credentials = basicAutheticationInfo;

                MailAddress senderMail = new MailAddress("it.hawk.it@gmail.com", sender.Email);
                MailAddress recipientMail = new MailAddress("it.hawk.it@gmail.com", "Компания HAWKIT");
                MailMessage message = new MailMessage(senderMail, recipientMail);

                MailAddress replyTo = new MailAddress(sender.Email);
                message.ReplyToList.Add(replyTo);

                message.Subject = $"Заявка от {sender.Name}";
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                message.Body = sender.GetInfoToHtml();
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;

                _smtpClient.Send(message);
            }
            catch (SmtpException ex)
            {
                throw new ApplicationException
                    ("SmtpException has occured: " + ex.Message);
            }

        }
    }
}
