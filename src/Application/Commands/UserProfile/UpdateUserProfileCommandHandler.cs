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

        if (!user.Succeeded)
            throw new UnauthorizedException("You do not have permission to update this user");

        if (!(user.Value!.UserProfile.EId == request.Eid))
            throw new UnauthorizedException("You do not have permission to update this user");

        var userProfileAggregate = _mapper.Map<UserProfileAggregate>(request.UpdateUserProfileRequestDto);

        userProfileAggregate.UpdatedAt = DateTime.Now;
        
        var result = await UnitOfWork.UserProfileRepository.UpdateUserProfileAsync(userProfileAggregate);

        return result.Succeeded
            ? _mapper.Map<UpdateUserProfileResponseDto>(userProfileAggregate)
            : throw new BadRequestException(
                result.Errors.FirstOrDefault()
                ?? "Failed to update the user's profile");
    }
}