using Domain.Entities;

namespace Domain.Interfaces;

public interface IMailRepository
{
    Task<bool> SendEmailConfirmation(UserAccountAggregate userAccount);
}