using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.News;

namespace Application.Commands.News.Create;

public class CreateNewsCommandHandler : TransactionalCommandHandler<CreateNewsCommand, CreateNewsResponseDto>
{
    private readonly IMapper _mapper;

    public CreateNewsCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<CreateNewsResponseDto> ExecuteCoreAsync(CreateNewsCommand request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.UserAccountRepository.GetCurrentUser();

        var newsAggregate = _mapper.Map<NewsAggregate>(request);
        
        newsAggregate.CreatedByUserAccountId = user.Value!.Id;
        newsAggregate.UpdatedByUserAccountId = user.Value!.Id;
        newsAggregate.CreatedAt = DateTime.Now;
        newsAggregate.DeletedAt = null;
        
        var result = await UnitOfWork.NewsRepository.CreateNewsAsync(newsAggregate);

        if (result.Succeeded)
            return _mapper.Map<CreateNewsResponseDto>(result.Value); 
        
        throw new BadRequestException(
            result.Errors.FirstOrDefault() 
            ?? "There was an error creating the news");
    }
}