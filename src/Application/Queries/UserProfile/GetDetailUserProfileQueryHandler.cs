using AutoMapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Exceptions;
using Shared.Dtos.UserProfile;

namespace Application.Queries.UserProfile;

public class GetDetailUserProfileQueryHandler : TransactionalQueryHandler<GetDetailUserProfileQuery, GetDetailUserProfileResponseDto>
{
    private readonly IMapper _mapper;
    
    public GetDetailUserProfileQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<GetDetailUserProfileResponseDto> ExecuteCoreAsync(GetDetailUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork
            .UserProfileRepository
            .FindByCondition(x => x.EId == request.Eid, true)
            .FirstOrDefaultAsync(cancellationToken);
        
        return result is not null 
            ? _mapper.Map<GetDetailUserProfileResponseDto>(result) 
            : throw new BadRequestException("There was an error processing your request.");
    }
}