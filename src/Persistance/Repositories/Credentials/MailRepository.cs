using System.Net;
using System.Net.Mail;
using System.Text;
using Domain.Aggregates;
using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using QRCoder;
using Shared.Configurations.Environment;
using Shared.Model;
using Shared.Results;

namespace Persistance.Repositories.Credentials;

/// <summary>
/// Provides methods for sending email confirmations and verifying email confirmation tokens.
/// </summary>
public class MailRepository : IMailRepository
{
    private readonly EnvironmentConfiguration _environment;
    private readonly UserManager<UserAccountAggregate> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="MailRepository"/> class.
    /// </summary>
    /// <param name="environment">The environment configuration used for SMTP settings.</param>
    /// <param name="userManager">The UserManager to manage user-related tasks.</param>
    public MailRepository(
        EnvironmentConfiguration environment,
        UserManager<UserAccountAggregate> userManager
    )
    {
        _environment = environment;
        _userManager = userManager;
    }

    /// <summary>
    /// Sends an email confirmation message to the user with a link to confirm their email address.
    /// </summary>
    /// <param name="userAccount">The user account to send the confirmation email to.</param>
    /// <param name="mfaViewModel">The MFA view model containing MFA information.</param>
    /// <returns>A <see cref="Result"/> indicating the success or failure of the operation.</returns>
    public async Task<Result> SendEmailConfirmation(UserAccountAggregate userAccount, MfaViewModel mfaViewModel)
    {
        string userToken = await _userManager.GenerateUserTokenAsync(
            userAccount,
            _userManager.Options.Tokens.EmailConfirmationTokenProvider,
            UserManager<UserAccountAggregate>.ConfirmEmailTokenPurpose
        );

        string emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(userAccount);
        
        string token = $"{userAccount.Email}:{mfaViewModel.SharedKey}:{userToken}:{emailToken}";
        
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
        
        string smtpServer = _environment.GetSmtpServer() ?? string.Empty;
        int smtpPort = int.Parse(_environment.GetSmtpPort() ?? string.Empty);
        string smtpPassword = _environment.GetSmtpPassword() ?? string.Empty;
        string smtpUsername = _environment.GetSmtpUsername() ?? string.Empty;
        string fromEmail = _environment.GetSmtpEmail() ?? string.Empty;
            
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
        mailMessage.To.Add(userAccount.Email!);
        mailMessage.Subject = "Welcome to Our App! Please confirm your Email";
        mailMessage.SubjectEncoding = Encoding.UTF8;
        mailMessage.Body = body;
        mailMessage.BodyEncoding = Encoding.UTF8;
        mailMessage.IsBodyHtml = true;
            
        await smtpClient.SendMailAsync(mailMessage);
        return Result.Success();
    }

    /// <summary>
    /// Verifies the email confirmation token and confirms the user's email address.
    /// </summary>
    /// <param name="token">The confirmation token received by the user.</param>
    /// <returns>The email of the user if the token is valid, or null if the token is invalid.</returns>
    public async Task<Result<UserAccountAggregate>> VerifyEmailConfirmationToken(string token)
    {
        string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        string[] parts = decodedToken.Split(':');

        if (parts.Length < 3)
            return Result<UserAccountAggregate>.Failure("Could not verify your account");
            
        string email = parts[0];
        string userKey = parts[2];
        string emailKey = parts[3];
        
        var tokenProvider = _userManager.Options.Tokens.EmailConfirmationTokenProvider;
        var purpose = UserManager<UserAccountAggregate>.ConfirmEmailTokenPurpose;

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return Result<UserAccountAggregate>.Failure("Could not verify your account");

        if (await _userManager.IsEmailConfirmedAsync(user))
            return Result<UserAccountAggregate>.Success(user);
        
        bool validation = await _userManager.VerifyUserTokenAsync(
            user, 
            tokenProvider, 
            purpose, 
            userKey
        );

        if (!validation)
            return Result<UserAccountAggregate>.Failure("Could not verify your account");
        
        var confirmResult = await _userManager.ConfirmEmailAsync(user, emailKey); 
        
        if (!confirmResult.Succeeded) 
            return Result<UserAccountAggregate>.Failure("Could not verify your account");

        return Result<UserAccountAggregate>.Success(user);

    }
    
    /// <summary>
    /// Generates a QR code image for setting up MFA using the provided authenticator URI.
    /// </summary>
    /// <param name="authenticatorUri">The URI used for MFA setup in the authenticator app.</param>
    /// <returns>A Base64-encoded string representing the QR code image.</returns>
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