using BlogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly ILogger<EmailsController> _logger;
        private readonly IEmailService _sendEmail;

        public EmailsController(ILogger<EmailsController> logger, IEmailService sendEmail)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sendEmail = sendEmail ?? throw new ArgumentNullException(nameof(sendEmail));
        }

        [HttpPost]
        [Route("v1/sendemail")]
        public async Task<IActionResult> TestEmail([FromQuery] string email, [FromQuery] string subject, [FromQuery] string body)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            {
                _logger.LogWarning("Parâmetros de email inválidos.");
                return BadRequest("Parâmetros de email inválidos.");
            }

            try
            {
                await _sendEmail.SendEmailAsync(email, subject, body);
                _logger.LogInformation($"{StatusCodes.Status200OK} - Email enviado com sucesso para {email}");
                return Ok("Email enviado com sucesso !!!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email para {Email}", email);
                return BadRequest("Erro ao enviar o email");
            }
        }
    }
}
