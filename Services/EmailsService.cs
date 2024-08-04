using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using HR.DataModels;
using Microsoft.Extensions.Options;

namespace HR.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        /// <summary>
        /// Sends an email using the specified email model and configuration settings.
        /// </summary>
        /// <param name="model">An object containing the email details including recipient, subject, body, and optional attachment.</param>
        /// <param name="errorMessage">An output parameter that contains the error message if the email sending fails; otherwise, it is set to null.</param>
        /// <returns>Returns true if the email was successfully sent; otherwise, false.</returns>
        /// <remarks>
        /// This method creates a <see cref="MailMessage"/> instance using the sender's email address configured in the settings. It sets up the email subject and body, and attaches a file if provided in the <paramref name="model"/>. The method then configures an <see cref="SmtpClient"/> with the SMTP host, port, and credentials from the configuration settings to send the email.
        /// If the email sending process fails, the method catches the exception, sets the <paramref name="errorMessage"/> to the exception message, and returns false.
        /// </remarks>
        public bool SendEmail(IEmail model, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                using (var mailMessage = new MailMessage(_emailSettings.SenderEmail, model.To))
                {
                    mailMessage.Subject = model.Subject;
                    mailMessage.Body = model.Body;
                    mailMessage.IsBodyHtml = false;

                    if (model.Attachment != null && model.Attachment.Length > 0)
                    {
                        var fileName = Path.GetFileName(model.Attachment.FileName);
                        mailMessage.Attachments.Add(new Attachment(model.Attachment.OpenReadStream(), fileName));
                    }

                    using (var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort))
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.EmailPassword);
                        smtpClient.Send(mailMessage);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}