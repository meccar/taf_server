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
        
        if (getCurrentUserResult == null)
            throw new UnauthorizedException("You are not logged in.");

        var getUserProfileResult = await UnitOfWork
            .UserProfileRepository
            .GetUserProfileAsync(getCurrentUserResult.UserProfileId);
        
        if (getUserProfileResult == null)
            throw new UnauthorizedException("You are not logged in.");
        
        getUserProfileResult.DeletedAt = DateTime.Now;
        getUserProfileResult.IsDeleted = true;
        
        var softDeleteUserAccountResult = await UnitOfWork
            .UserProfileRepository
            .SoftDeleteUserAccount(getUserProfileResult);

        return softDeleteUserAccountResult != null
            ? new SuccessResponseDto(true)
            : throw new BadRequestException("There was an error. Please try again.");
    }
}