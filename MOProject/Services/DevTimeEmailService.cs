namespace MOProject.Services;

public class DevTimeEmailService : IEmailService
{
  private readonly ILogger<DevTimeEmailService> _logger;

  public DevTimeEmailService(ILogger<DevTimeEmailService> logger)
  {
    _logger = logger;
  }

  public void SendMail(string to, string from, string subject, string body)
  {
    _logger.LogInformation($"Sending Email to {to} with a subject of {subject}.");
  }
}
