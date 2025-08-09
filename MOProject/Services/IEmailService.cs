public interface IEmailService
{
    Task SendEmailAsync(string name, string email, string phone, string message);
}
