using AutoMapper;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.News;

namespace Application.Queries.News;

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
        var result = await UnitOfWork.NewsRepository.GetDetailNewsAsync(request.Eid);
        
        return result.Succeeded
            ? _mapper.Map<GetDetailNewsResponseDto>(result.Value) 
            : throw new BadRequestException(
                result.Errors.FirstOrDefault() 
                ?? "There was an error getting the news");
    }
}