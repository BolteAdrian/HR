namespace HR.DataModels
{
    public class EmailSettings
    {
        public string SenderEmail { get; set; }
        public string EmailPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
