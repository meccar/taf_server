using taf_server.Domain.Model;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Entities;
using taf_server.Presentations.Dtos.UserLoginData;

namespace taf_server.Domain.Interfaces;

public interface IUserLoginDataCommandRepository
{
    Task<bool> IsUserLoginDataExisted(string loginCredential);
    Task<UserLoginDataModel> CreateUserLoginData(CreateUserLoginDataDto userLoginDataDto);
}