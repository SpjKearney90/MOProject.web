using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string name, string email, string phone, string message)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

       var smtpClient = new SmtpClient(emailSettings["SmtpServer"])
{
    Port = int.Parse(emailSettings["Port"]),
    Credentials = new NetworkCredential(emailSettings["Username"], emailSettings["Password"]),
    EnableSsl = true,
    UseDefaultCredentials = false,
    DeliveryMethod = SmtpDeliveryMethod.Network
};

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
            Subject = $"New Contact Message from {name}",
            Body = $"Name: {name}\nEmail: {email}\nPhone: {phone}\n\nMessage:\n{message}",
            IsBodyHtml = false,
        };

        mailMessage.To.Add(emailSettings["SenderEmail"]); // send to yourself

        await smtpClient.SendMailAsync(mailMessage);
    }
}
