using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Shared.Configurations.Environment;
using Microsoft.AspNetCore.Mvc;

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

    public async Task SendEmailConfirmation(UserAccountAggregate userAccount)
    {
        string baseToken = await _userManager.GenerateEmailConfirmationTokenAsync(userAccount);
        string token = $"{userAccount.EId}:{baseToken}";
        
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        
        string url = $"https://localhost:7293?token={token}";
        
        string body = $@"
            <html>
                <body>
                    <p>Hello {userAccount.UserName},</p>
                    <p>Thank you for registering with us. Please confirm your email address by clicking the link below:</p>
                    <p><a href='{url}'>Click here to confirm your email</a></p>
                    <p>If you didn't create an account, you can ignore this email.</p>
                </body>
            </html>";
        
        string smtpServer = _environment.GetSmtpServer();
        int smtpPort = int.Parse(_environment.GetSmtpPort());
        string smtpPassword = _environment.GetSmtpPassword();
        string smtpUsername = _environment.GetSmtpUsername();
        string fromEmail = _environment.GetSmtpEmail();
            
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
        mailMessage.To.Add(userAccount.Email);
        mailMessage.Subject = "Welcome to Our App! Please confirm your Email";
        mailMessage.SubjectEncoding = Encoding.UTF8;
        mailMessage.Body = body;
        mailMessage.BodyEncoding = Encoding.UTF8;
        mailMessage.IsBodyHtml = true;
            
        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task<string?> VerifyEmailConfirmationToken(string token)
    {
        string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        string[] parts = decodedToken.Split(':');

        string userEId = parts[0];
        string baseToken = parts[1];
        
        var tokenProvider = _userManager.Options.Tokens.EmailConfirmationTokenProvider;
        var purpose = UserManager<UserAccountAggregate>.ConfirmEmailTokenPurpose;
        
        var user = await _userManager.Users
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.EId == userEId);

        if (user == null)
            return null;
        
        bool validation = await _userManager.VerifyUserTokenAsync(
            user, 
            tokenProvider, 
            purpose, 
            baseToken
        );

        var confirmResult = await _userManager.ConfirmEmailAsync(user, baseToken); 
        
        if (!confirmResult.Succeeded)
        {
            foreach (var error in confirmResult.Errors)
            {
                Console.WriteLine($"Token Verification Error: {error.Description}");
            }
            string test = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var testResult = await _userManager.ConfirmEmailAsync(user, test); 
            bool testValidation = await _userManager.VerifyUserTokenAsync(
                user, 
                tokenProvider, 
                purpose,
                test
            );
        }

        return validation && confirmResult.Succeeded ? user.Email : null;
    }
}