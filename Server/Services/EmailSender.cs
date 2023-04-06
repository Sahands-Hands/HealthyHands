using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace HealthyHands.Server.Services
{
    public class EmailSender : IEmailSender
    {
        
        public EmailSender()
        {
 
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            string fromMail = "[YOUREMAILID]";
            string fromPassword = "[APPPASSWORD]";
            
            MailMessage message = new MailMessage();
            message.From =new MailAddress("healthyhandsappteam@gmail.com");
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body ="<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;
            
            
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp-relay.sendinblue.com", //or another email sender provider
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("ry.m.wei.99@gmail.com", "IzHCTFtayA0Jhr16")
            };

            client.Send(message);
        }
    }
}
