using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Entities;

namespace taf_server.Domain.Interfaces;

public interface IUserLoginDataCommandRepository
{
    Task<bool> IsUserLoginDataExisted(string loginCredential);
}