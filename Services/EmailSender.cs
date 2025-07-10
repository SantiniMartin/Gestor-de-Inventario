
using MailKit.Net.Smtp;
using MimeKit;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Soporte", _config["EmailSettings:From"]));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), true);
        await client.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}

