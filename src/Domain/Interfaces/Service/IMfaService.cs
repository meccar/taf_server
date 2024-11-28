using Domain.Entities;
using Domain.Model;

namespace Domain.Interfaces.Service;

public interface IMfaService
{
    Task<bool> MfaSetup(UserLoginDataEntity user);
    Task<bool> MfaSetup(MfaViewModel model, UserLoginDataEntity user);
}