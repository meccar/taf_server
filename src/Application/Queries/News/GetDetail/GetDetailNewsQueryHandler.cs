using AutoMapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Exceptions;
using Shared.Dtos.News;

namespace Application.Queries.News.GetDetail;

public class GetDetailNewsQueryHandler : TransactionalQueryHandler<GetDetailNewsQuery, GetDetailNewsResponseDto>
{
    private readonly IMapper _mapper;
    
    public GetDetailNewsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<GetDetailNewsResponseDto> ExecuteCoreAsync(GetDetailNewsQuery request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork
            .NewsRepository
            .FindByCondition(x => x.Uuid == request.Eid, true)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        return result is not null
            ? _mapper.Map<GetDetailNewsResponseDto>(result) 
            : throw new BadRequestException("There was an error getting the news");
    }
}