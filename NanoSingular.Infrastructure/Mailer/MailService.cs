using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


// Mailer implentation with MailKit (3rd-party package)
namespace NanoSingular.Infrastructure.Mailer
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings) 
        {
            _settings = settings.Value;
        }
        public async Task SendAsync(MailRequest request)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From));


            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();

            // send email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
