namespace SSE.Core.Services.Mail
{
    public class EmailConfiguration
    {
        public string SmtpServer { set; get; }
        public string SmtpPort { set; get; }
        public string SmtpUsername { set; get; }
        public string SmtpPassword { set; get; }
        public string PopServer { set; get; }
        public string PopPort { set; get; }
        public string PopUsername { set; get; }
        public string PopPassword { set; get; }
    }
}