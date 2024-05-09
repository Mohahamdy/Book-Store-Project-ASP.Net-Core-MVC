using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;

namespace Project.Models
{
    //interface to apply dependency injection
    public interface ISenderEmail
    {
        Task SendEmailAsync(string ToEmail,string Subject,string Body,bool IsBodyHtml = false);
    }
    public class EmailSender : ISenderEmail
    {
        private readonly IConfiguration configuration;

        //inject configuration to read from app setting json file
        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
        {
            //read from EmailSettings in appSetting.json
            string MailServer = configuration["EmailSettings:MailServer"];
            string FromEmail = configuration["EmailSettings:FromEmail"];
            string Password = configuration["EmailSettings:Password"];
            int Port = int.Parse(configuration["EmailSettings:MailPort"]);

            //create client of stmp server pass params to it
            var client = new SmtpClient(MailServer, Port)
            {
                Credentials = new NetworkCredential(FromEmail, Password),
                EnableSsl = true,
            };

            //create mail message that will send to user
            MailMessage mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Body)
            {
                IsBodyHtml = IsBodyHtml
            };

            //send mail through client
            return client.SendMailAsync(mailMessage);
        }
    }
}
