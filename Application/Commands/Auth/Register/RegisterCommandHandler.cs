using taf_server.Domain.SeedWork.Command;
using taf_server.Application.Exceptions;
using taf_server.Infrastructure.Repositories;
using taf_server.Presentations.Dtos.Authentication;

namespace taf_server.Application.Commands.Auth.Register;

public class RegisterCommandHandler(UnitOfWork unitOfWork) 
        : ICommandHandler<RegisterCommand, RegisterUserResponseDto>
{
    public async Task<RegisterUserResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await unitOfWork.UserAccountCommandRepository.FindByEmailAsync(request.UserLogin.Email) != null)
            throw new BadRequestException("Email already exists");

        if (await unitOfWork.UserAccountCommandRepository.FindByPhoneNumberAsync(request.UserAccount.PhoneNumber) != null)
            throw new BadRequestException("Phone number already exists");

        var userAccount = await unitOfWork.UserAccountCommandRepository.CreateUserAsync(request.UserAccount);
        var userLoginData = await unitOfWork.UserLoginDataCommandRepository.IsUserLoginDataExisted(request.UserLogin.Email)

        return null;
    }
}
