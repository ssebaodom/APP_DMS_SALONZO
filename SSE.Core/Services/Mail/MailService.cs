using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using SSE.Core.Common.Constants;
using SSE.Core.Common.Extensions;
using SSE.Core.Services.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SSE.Core.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly IConfiguration configuration;
        private EmailConfiguration emailConfiguration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.emailConfiguration = this.configuration.GetSection(CONFIGURATION_KEYS.EMAIL).Get<EmailConfiguration>();
        }

        public List<EmailMessage> ReceiveEmail(bool useSsl = true, int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(emailConfiguration.PopServer, emailConfiguration.PopPort.ToInt32(), useSsl);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(emailConfiguration.PopUsername, emailConfiguration.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emails.Add(emailMessage);
                }

                return emails;
            }
        }

        public void Send(EmailMessage emailMessage, TextFormat textFormat = TextFormat.Text, bool useSsl = false)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc.
            message.Body = new TextPart(textFormat)
            {
                Text = emailMessage.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort.ToInt32(), useSsl);

                //Remove any OAuth functionality as we won't be using it.
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(emailConfiguration.SmtpUsername, CryptHelper.Decrypt(emailConfiguration.SmtpPassword));

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }
    }
}