using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
namespace APIServerSmartHome.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "kimhuy13032000@gmail.com";
        private readonly string _smtpPassword = "huynhdangkimhuy1303";

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();    
            message.Sender = new MailboxAddress("SmartHome", _smtpUsername);
            message.From.Add(new MailboxAddress("SmartHome", _smtpUsername));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                    await client.SendAsync(message);
                }catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                await client.DisconnectAsync(true);
            }
        }
    }
}
