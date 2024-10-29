using Microsoft.Extensions.Options;
using MOBILEAPI2024.BLL.Services.IServices;
using MOBILEAPI2024.DTO.Common;
using MimeKit;

namespace MOBILEAPI2024.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceOptionsFirst Options;

        public EmailService(IOptions<AppSettings> aAppSettings)
        {
            Options = aAppSettings.Value.EmailServiceOptionsFirst;
        }

        public void SendEmail(string aRecipientName, string aRecipientAddress, string aSubject, string aText)
        {
            var vSender = new MailboxAddress(Options.SenderName, Options.SenderAddress);
            var vRecipient = new MailboxAddress(aRecipientName, aRecipientAddress);

            var vMessage = new MimeMessage
            {
                Subject = aSubject,
                Body = new TextPart("html")
                {
                    Text = aText
                }
            };

            vMessage.From.Add(vSender);
            vMessage.To.Add(vRecipient);
            //var copyAddress = new MailboxAddress(string.Empty, Options.CopyAddress);
            //vMessage.Cc.Add(copyAddress);

            try
            {
                Task.Run(() =>
                {
                    using var vClient = new MailKit.Net.Smtp.SmtpClient();
                    vClient.Connect(Options.Host, Options.Port, Options.UseSsl);
                    // uncomment if the SMTP server requires authentication
                    vClient.Authenticate(Options.UserName, Options.Password);
                    vClient.Send(vMessage);
                    vClient.Disconnect(true);
                });
            }
            catch(Exception ex)
            {

            }
        }
    }
}
