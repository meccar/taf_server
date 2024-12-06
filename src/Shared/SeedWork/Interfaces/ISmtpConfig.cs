namespace Shared.SeedWork.Interfaces;

/// <summary>
/// Represents a configuration interface for SMTP (Simple Mail Transfer Protocol) settings.
/// This interface defines the methods to retrieve SMTP server-related configuration values
/// required for sending emails, such as the server address, port, username, password, and email.
/// </summary>
public interface ISmtpConfig
{
    /// <summary>
    /// Gets the SMTP server address.
    /// This method returns the address of the SMTP server used for sending emails.
    /// It may return null if no server address is configured.
    /// </summary>
    /// <returns>The SMTP server address or null if not configured.</returns>
    string? GetSmtpServer();

    /// <summary>
    /// Gets the SMTP server port.
    /// This method returns the port number used by the SMTP server. Typically, this would be 25, 587, or 465.
    /// It may return null if no port is configured.
    /// </summary>
    /// <returns>The SMTP port or null if not configured.</returns>
    string? GetSmtpPort();

    /// <summary>
    /// Gets the SMTP server username.
    /// This method returns the username used for authenticating with the SMTP server.
    /// It may return null if no username is configured.
    /// </summary>
    /// <returns>The SMTP username or null if not configured.</returns>
    string? GetSmtpUsername();

    /// <summary>
    /// Gets the SMTP server password.
    /// This method returns the password used for authenticating with the SMTP server.
    /// It may return null if no password is configured.
    /// </summary>
    /// <returns>The SMTP password or null if not configured.</returns>
    string? GetSmtpPassword();

    /// <summary>
    /// Gets the email address associated with the SMTP server.
    /// This method returns the email address that will be used as the sender for outgoing emails.
    /// It may return null if no email address is configured.
    /// </summary>
    /// <returns>The SMTP email address or null if not configured.</returns>
    string? GetSmtpEmail();
}