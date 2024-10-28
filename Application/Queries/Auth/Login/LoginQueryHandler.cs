using System.Security.Authentication;
using taf_server.Domain.Interfaces;
using taf_server.Domain.Model;
using taf_server.Domain.SeedWork.Query;

namespace taf_server.Application.Queries.Auth.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, UserAccountModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserAccountModel> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var userAccountEntity = _unitOfWork.UserAccountQueryRepository.FindOneByEmail(request.Email);
        if (userAccountEntity == null)
        {
            throw new InvalidCredentialException("Invalid credentials");
        }

        return null;
    }
}