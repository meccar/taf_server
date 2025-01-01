using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.News;

namespace Application.Commands.News.Update;

public class UpdateNewsCommandHandler : TransactionalCommandHandler<UpdateNewsCommand, UpdateNewsResponseDto>
{
    private readonly IMapper _mapper;
    
    public UpdateNewsCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<UpdateNewsResponseDto> ExecuteCoreAsync(UpdateNewsCommand request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.UserAccountRepository.GetCurrentUser();

        if (user is null)
            throw new BadRequestException("User not found");
        
        var getNewsResult = await UnitOfWork.NewsRepository.GetDetailNewsAsync(request.Eid);
        
        if (getNewsResult is null)
            throw new BadRequestException("There was an error getting the news");
        
        if (user.Id != getNewsResult.CreatedByUserAccountId)
            throw new UnauthorizedException("You do not have permission to update the news");

        var newsAggregate = _mapper.Map<NewsAggregate>(request);
        
        var result = await UnitOfWork.NewsRepository.UpdateNewsAsync(newsAggregate);
        
        return result is not null 
            ?_mapper.Map<UpdateNewsResponseDto>(result)
            : throw new BadRequestException("There was an error updating the news");
    }
}