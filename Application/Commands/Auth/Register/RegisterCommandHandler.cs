using taf_server.Domain.SeedWork.Command;
using taf_server.Application.Exceptions;
using taf_server.Domain.Model;
using taf_server.Infrastructure.Repositories;
using taf_server.Presentations.Dtos.Authentication;

namespace taf_server.Application.Commands.Auth.Register;

public class RegisterCommandHandler(UnitOfWork unitOfWork) 
        : ICommandHandler<RegisterCommand, UserAccountModel>
{
    public async Task<UserAccountModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.UserLoginDataCommandRepository.IsUserLoginDataExisted(request.UserLogin.Email))
            throw new BadRequestException("Email already exists");

        if (await unitOfWork.UserAccountCommandRepository.IsUserAccountDataExisted(request.UserAccount.PhoneNumber))
            throw new BadRequestException("Phone number already exists");

        var userAccount = await unitOfWork.UserAccountCommandRepository.CreateUserAsync(request.UserAccount);
        userAccount.UserLoginData = await unitOfWork.UserLoginDataCommandRepository.CreateUserLoginData(request.UserLogin);

        return userAccount;
    }
}
