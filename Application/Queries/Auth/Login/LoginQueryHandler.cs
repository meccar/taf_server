using System.Security.Authentication;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Query;

namespace Application.Queries.Auth.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, UserAccountModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserAccountModel> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var userAccountModel = _unitOfWork.UserLoginDataQueryRepository.FindOneByEmail(request.Email);
        if (userAccountModel == null)
        {
            throw new InvalidCredentialException("Invalid credentials");
        }

        return null;
    }
}