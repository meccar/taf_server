using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Validator;

public class CustomUserValidator : IUserValidator<UserAccountAggregate>
{
    public Task<IdentityResult> ValidateAsync(UserManager<UserAccountAggregate> manager, UserAccountAggregate user)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}