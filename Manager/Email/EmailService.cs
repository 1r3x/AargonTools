using AargonTools.Interfaces.Email;
using AargonTools.Models.Email;
using MimeKit;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using System;

namespace AargonTools.Manager.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(string.Empty, _emailConfig.From));
            email.To.Add(new MailboxAddress(string.Empty, to));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                await smtp.SendAsync(email);
                Serilog.Log.Debug("The email has been sent successfully.: {email}", email);
            }
            catch (Exception ex)
            {
                Serilog.Log.Debug("Failed to send email. : {ex}", ex);
                // Handle or log the exception as needed
                throw new InvalidOperationException("Failed to send email.", ex);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}