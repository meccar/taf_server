using Domain.Aggregates;
using Domain.Entities;
using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces;

public interface IMailRepository
{
    Task<Result> SendEmailConfirmation(UserAccountAggregate userAccount, MfaViewModel mfaViewModel);
    Task<string?> VerifyEmailConfirmationToken(string token);
}