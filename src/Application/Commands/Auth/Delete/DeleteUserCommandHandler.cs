using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Shared.Dtos.Authentication;
using Shared.Dtos.Exceptions;

namespace Application.Commands.Auth.Delete;

public class DeleteUserCommandHandler : TransactionalCommandHandler<DeleteUserCommand, DeleteUserResponseDto>
{
    private readonly IMapper _mapper;
    public DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<DeleteUserResponseDto> ExecuteCoreAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.UserAccountRepository.GetCurrentUser();
        
        if (user == null)
            throw new UnauthorizedException("You do not have permission to delete this user");

        
        if (!(user.EId != request.Eid))
            throw new UnauthorizedException("You do not have permission to delete this user");

        // user.
        var userProfileAggregate = _mapper.Map<UserProfileAggregate>(request);
        
        //TODO: fix this
        var userAccountAggregate = UnitOfWork.UserProfileRepository.SoftDeleteUserAccount(userProfileAggregate);

        throw new NotImplementedException();
    }
}