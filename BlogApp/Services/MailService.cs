using BlogApp.Services;
using MailKit.Net.Smtp;
using MimeKit;

public class MailService : IMailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress("Ilayda Kackin", "kackinilayda@gmail.com"));
        mimeMessage.To.Add(new MailboxAddress("John Doe", email));

        mimeMessage.Subject = subject;
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, false);
        smtp.Authenticate("kackinilayda@gmail.com", "gdmczumyliydwtyz");

        await smtp.SendAsync(mimeMessage);
        await smtp.DisconnectAsync(true);
    }
}
