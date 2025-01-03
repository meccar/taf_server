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
        
        if (user is null)
            throw new UnauthorizedException("You are not logged in");
        
        var newsAggregate = _mapper.Map<NewsAggregate>(request);
        
        newsAggregate.CreatedByUserAccountId = user.Id;
        newsAggregate.UpdatedByUserAccountId = user.Id;
        newsAggregate.CreatedAt = DateTime.Now;
        newsAggregate.DeletedAt = null;
        
        var result = await UnitOfWork.NewsRepository.CreateAsync(newsAggregate);

        return result is not null
            ? _mapper.Map<CreateNewsResponseDto>(result.Entity) 
            : throw new BadRequestException("There was an error creating the news");
    }
}