namespace Shared.SeedWork.Interfaces;

public interface ISmtpConfig
{
    string GetSmtpServer();
    string GetSmtpPort();
    string GetSmtpUsername();
    string GetSmtpPassword();
    string GetSmtpEmail();
}