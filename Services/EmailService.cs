using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using static BlogApi.JwtConfigure.Configure;
using Microsoft.Extensions.Options;

namespace BlogApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpConfiguration _smtp;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpConfiguration> smtpSettings, IWebHostEnvironment env, ILogger<EmailService> logger)
        {
            _smtp = smtpSettings.Value;
            _env = env;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ValidateSmtpConfiguration(_smtp);
        }

        private void ValidateSmtpConfiguration(SmtpConfiguration smtp)
        {
            if (string.IsNullOrWhiteSpace(smtp.Server))
                throw new ArgumentException("Servidor SMTP não está configurado.");
            if (smtp.Port <= 0)
                throw new ArgumentException("Porta SMTP não está configurada corretamente.");
            if (string.IsNullOrWhiteSpace(smtp.Username))
                throw new ArgumentException("Nome de usuário SMTP não está configurado.");
            if (string.IsNullOrWhiteSpace(smtp.Password))
                throw new ArgumentException("Senha SMTP não está configurada.");
            if (string.IsNullOrWhiteSpace(smtp.SenderEmail))
                throw new ArgumentException("Email do remetente não está configurado.");
            if (string.IsNullOrWhiteSpace(smtp.SenderName))
                throw new ArgumentException("Nome do remetente não está configurado.");
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtp.SenderName, _smtp.SenderEmail));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    SecureSocketOptions options = _env.IsDevelopment() ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                    await client.ConnectAsync(_smtp.Server, _smtp.Port, options);
                    await client.AuthenticateAsync(_smtp.Username, _smtp.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao enviar email para {Email}", email);
                    throw new InvalidOperationException("Erro ao enviar email.", ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
