using AutoMapper;
using Domain.Interfaces;
using Shared.Dtos;
using Shared.Dtos.Exceptions;

namespace Application.Commands.Auth.Delete;

public class DeleteUserCommandHandler 
    : TransactionalCommandHandler<DeleteUserCommand, SuccessResponseDto>
{
    private readonly IMapper _mapper;
    public DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<SuccessResponseDto> ExecuteCoreAsync(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var getCurrentUserResult = await UnitOfWork
            .UserAccountRepository
            .GetCurrentUser(request.UserAccountEid);
        
        if (!getCurrentUserResult.Succeeded)
            throw new UnauthorizedException(getCurrentUserResult.Errors.FirstOrDefault()!);

        var getUserProfileResult = await UnitOfWork
            .UserProfileRepository
            .GetUserProfileAsync(getCurrentUserResult.Value!.UserProfileId);
        
        if (!getUserProfileResult.Succeeded)
            throw new UnauthorizedException(getUserProfileResult.Errors.FirstOrDefault()!);
        
        getUserProfileResult.Value!.DeletedAt = DateTime.Now;
        getUserProfileResult.Value!.IsDeleted = true;
        
        var softDeleteUserAccountResult = await UnitOfWork
            .UserProfileRepository
            .SoftDeleteUserAccount(getUserProfileResult.Value!);

        return softDeleteUserAccountResult.Succeeded
            ? new SuccessResponseDto(true)
            : throw new BadRequestException(softDeleteUserAccountResult.Errors.FirstOrDefault()!);
    }
}