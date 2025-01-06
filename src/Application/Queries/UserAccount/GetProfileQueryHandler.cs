using AutoMapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Exceptions;
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
        var userAccount = await UnitOfWork.UserAccountRepository.GetCurrentUser();
        
        if (userAccount is null)
            throw new UnauthorizedException("You do not have permission to access");

        var userProfile = await UnitOfWork
            .UserProfileRepository
            .FindByCondition(x => x.Id == userAccount.UserProfileId, true)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (userProfile is null)
            throw new UnauthorizedException("You do not have permission to access");
            
        userProfile.UserAccount = userAccount;
        var response = _mapper.Map<GetProfileResponseDto>(userProfile);

        return response;
    }
}