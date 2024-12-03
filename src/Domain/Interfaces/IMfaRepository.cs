using Domain.Entities;
using Shared.Model;

namespace Domain.Interfaces;

public interface IMfaRepository
{
    Task<bool> MfaSetup(UserAccountAggregate user);
    Task<bool> MfaSetup(MfaViewModel model, UserAccountAggregate user);
}