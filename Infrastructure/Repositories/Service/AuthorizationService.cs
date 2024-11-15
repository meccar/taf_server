using Domain.Interfaces;
using Infrastructure.SeedWork.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Repositories.Service;

public class AuthorizationService : AuthorizationHandler<AuthorizationRequirement>
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthorizationService(
        IUnitOfWork unitOfWork
        )
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AuthorizationRequirement requirement)
    {
        
    }
}