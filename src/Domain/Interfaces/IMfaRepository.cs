using Domain.Aggregates;
using Domain.Entities;
using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces;

public interface IMfaRepository
{
    Task<MfaViewModel> MfaSetup(UserAccountAggregate user);
    Task<Result> ValidateMfa(string email, string token);
}