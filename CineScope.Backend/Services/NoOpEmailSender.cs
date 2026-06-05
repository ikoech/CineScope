using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class NoOpEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Do nothing (no email sending in development)
        return Task.CompletedTask;
    }
}
