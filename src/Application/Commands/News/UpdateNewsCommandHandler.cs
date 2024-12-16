using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.News;

namespace Application.Commands.News;

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

        if (user == null)
            throw new BadRequestException("User not found");
        
        var GetNewsResult = await UnitOfWork.NewsRepository.GetDetailNewsAsync(request.Eid);
        
        if (!GetNewsResult.Succeeded)
            throw new BadRequestException(
                GetNewsResult.Errors.FirstOrDefault() 
                ?? "There was an error getting the news");
        
        if (!(user.Id == GetNewsResult.Value!.CreatedByUserAccountId))
            throw new UnauthorizedException("You do not have permission to update the news");

        var newsAggregate = _mapper.Map<NewsAggregate>(request);
        
        var result = await UnitOfWork.NewsRepository.UpdateAsync(newsAggregate);
        
        return result.Succeeded 
            ?_mapper.Map<UpdateNewsResponseDto>(result.Value)
            : throw new BadRequestException(
                result.Errors.FirstOrDefault()
                ?? "There was an error updating the news");
    }
}