using System.Net;
using System.Net.Mail;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Configurations.Environment;

namespace Infrastructure.Repositories;

public class MailRepository : IMailRepository
{
    private readonly EnvironmentConfiguration _environment;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly SignInManager<UserAccountAggregate> _signInManager;

    public MailRepository(
        EnvironmentConfiguration environment,
        UserManager<UserAccountAggregate> userManager,
        SignInManager<UserAccountAggregate> signInManager
    )
    {
        _environment = environment;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> SendEmailConfirmation(UserAccountAggregate userAccount)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(userAccount);
        
        var url = $"{_environment.GetSmtpServer()}?token={token}";
        
        var body = $@"
            <html>
                <body>
                    <p>Hello {userAccount.UserName},</p>
                    <p>Thank you for registering with us. Please confirm your email address by clicking the link below:</p>
                    <p><a href='{url}'>Click here to confirm your email</a></p>
                    <p>If you didn't create an account, you can ignore this email.</p>
                </body>
            </html>";
        
         return await SendEmailAsync(
            userAccount.Email,
            "Welcome to Our App! Please confirm your Email",
            body
            );
    }

    private async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
        var smtpServer = _environment.GetSmtpServer();
        var smtpPort = int.Parse(_environment.GetSmtpPort());
        var smtpPassword = _environment.GetSmtpPassword();
        var smtpUsername = _environment.GetSmtpUsername();
        var fromEmail = _environment.GetSmtpEmail();
        
        SmtpClient smtpClient = new SmtpClient(smtpServer)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = true
        };
        
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;

        MailMessage mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(fromEmail);
        mailMessage.To.Add(toEmail);
        mailMessage.Subject = subject;
        mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
        mailMessage.Body = body;
        mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
        mailMessage.IsBodyHtml = true;
        
        await smtpClient.SendMailAsync(mailMessage);
        
        return true;
        }
        catch (Exception ex)
        {
            return false;

        }
    }
}