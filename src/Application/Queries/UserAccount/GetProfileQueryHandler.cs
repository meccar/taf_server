using AutoMapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.UserAccount;

namespace Application.Queries.UserAccount;

public class GetProfileQueryHandler : TransactionalQueryHandler<GetProfileQuery, GetProfileResponseDto>
{
    private readonly IMapper _mapper;

    public GetProfileQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<GetProfileResponseDto> ExecuteCoreAsync(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.UserAccountRepository.GetCurrentUser();
        
        if (user is null)
            throw new UnauthorizedAccessException("User not found");

        user.UserProfile = await UnitOfWork
            .UserProfileRepository
            .FindByCondition(x => x.Id == user.UserProfileId, true)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return _mapper.Map<GetProfileResponseDto>(user);
    }
}