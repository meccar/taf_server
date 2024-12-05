using System.Net;
using System.Net.Mail;
using System.Text;
using Domain.Aggregates;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Shared.Configurations.Environment;
using Shared.Model;
using Shared.Results;

namespace Infrastructure.Repositories;

public class MailRepository : IMailRepository
{
    private readonly EnvironmentConfiguration _environment;
    private readonly UserManager<UserAccountAggregate> _userManager;

    public MailRepository(
        EnvironmentConfiguration environment,
        UserManager<UserAccountAggregate> userManager
    )
    {
        _environment = environment;
        _userManager = userManager;
    }

    public async Task<Result> SendEmailConfirmation(UserAccountAggregate userAccount, MfaViewModel mfaViewModel)
    {
        string baseToken = await _userManager.GenerateEmailConfirmationTokenAsync(userAccount);
        string token = $"{userAccount.Email}:{baseToken}";
        
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        
        string url = $"https://localhost:7293?token={token}";
        
        string qrCodeImageUrl = GenerateQrCode(mfaViewModel.AuthenticatorUri);
        
        string body = $@"
            <html>
                <body>
                    <p>Hello {userAccount.UserName},</p>
                    <p>Thank you for registering with us. Please confirm your email address by clicking the link below:</p>
                    <p><a href='{url}'>Click here to confirm your email</a></p>
                    <p>Please scan the QR code below to set up MFA on your authenticator app:</p>
                    <p>
                        <img src='data:image/png;base64,{qrCodeImageUrl}' 
                        alt='MFA QR Code'
                        style='width:150px; height:150px;' />
                    </p>
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
        return Result.Success();
    }

    public async Task<string?> VerifyEmailConfirmationToken(string token)
    {
        string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        string[] parts = decodedToken.Split(':');

        string email = parts[0];
        string baseToken = parts[1];
        
        var tokenProvider = _userManager.Options.Tokens.EmailConfirmationTokenProvider;
        var purpose = UserManager<UserAccountAggregate>.ConfirmEmailTokenPurpose;

        var user = await _userManager.FindByEmailAsync(email);

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
    
    private string GenerateQrCode(string authenticatorUri)
    {
        // Create a QR code for the Authenticator URI
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(authenticatorUri, QRCodeGenerator.ECCLevel.Q);
            using (var qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeBytes = qrCode.GetGraphic(10);
                return Convert.ToBase64String(qrCodeBytes);
            }
        }
    }
}