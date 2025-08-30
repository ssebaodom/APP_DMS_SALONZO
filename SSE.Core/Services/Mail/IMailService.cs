using MimeKit.Text;
using System.Collections.Generic;

namespace SSE.Core.Services.Mail
{
    public interface IMailService
    {
        void Send(EmailMessage message, TextFormat textFormat = TextFormat.Text, bool useSsl = true);

        List<EmailMessage> ReceiveEmail(bool useSsl = true, int maxCount = 10);
    }
}