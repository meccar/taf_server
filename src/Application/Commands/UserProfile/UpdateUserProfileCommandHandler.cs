using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.UserProfile;

namespace Application.Commands.UserProfile;

public class
    UpdateUserProfileCommandHandler : TransactionalCommandHandler<UpdateUserProfileCommand,
    UpdateUserProfileResponseDto>
{
    private readonly IMapper _mapper;

    public UpdateUserProfileCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<UpdateUserProfileResponseDto> ExecuteCoreAsync(UpdateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.UserAccountRepository.GetCurrentUser();

        if (user is null)
            throw new UnauthorizedException("You do not have permission to update this user");

        if (user.UserProfile.EId != request.Eid)
            throw new UnauthorizedException("You do not have permission to update this user");

        var userProfileAggregate = _mapper.Map<UserProfileAggregate>(request.UpdateUserProfileRequestDto);

        userProfileAggregate.UpdatedAt = DateTime.Now;
        
        var result = await UnitOfWork.UserProfileRepository.UpdateAsync(userProfileAggregate);

        return result is not null
            ? _mapper.Map<UpdateUserProfileResponseDto>(userProfileAggregate)
            : throw new BadRequestException("Failed to update the user's profile");
    }
}