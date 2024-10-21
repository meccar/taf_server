using Microsoft.AspNetCore.Identity;
using taf_server.Domain.Model;
using taf_server.Presentations.Dtos.UserAccount;

namespace taf_server.Domain.Interfaces;
public interface IUserAccountCommandRepository
{
    Task<taf_server.Domain.Aggregates.UserAccountAggregate?> FindByEmailAsync(string email);
    Task<taf_server.Domain.Aggregates.UserAccountAggregate?> FindByPhoneNumberAsync(string phoneNumber);
    Task<UserAccountModel> CreateUserAsync(CreateUserAccountDto createUserAccountDto);
    Task AddUserToRolesAsync(Domain.Aggregates.UserAccountAggregate user, IEnumerable<string> roles);
}
