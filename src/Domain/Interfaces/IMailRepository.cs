using Domain.Entities;
using Shared.Results;

namespace Domain.Interfaces;

public interface IMailRepository
{
    Task SendEmailConfirmation(UserAccountAggregate userAccount);
    Task<string?> VerifyEmailConfirmationToken(string token);
}