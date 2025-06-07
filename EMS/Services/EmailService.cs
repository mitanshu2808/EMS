using MailKit.Net.Smtp;
using MimeKit;
using EMS.Models;
using Microsoft.Extensions.Options;

namespace EMS.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            //var emailSettings = _emailSettings.GetSection("EmailSettings");
            //var senderEmail = emailSettings["SenderEmail"];
            //var senderPassword = emailSettings["SenderPassword"];
            //var smtpServer = emailSettings["SMTPServer"];
            //var smtpPort = int.Parse(emailSettings["SMTPPort"]);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EMS System", _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SMTPServer, _emailSettings.SMTPPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
