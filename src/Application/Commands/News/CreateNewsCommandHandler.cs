using AutoMapper;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.News;
using Shared.Model;

namespace Application.Commands.News;

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

        var newsModel = _mapper.Map<NewsModel>(request.NewsRequestModel);
        
        newsModel.CreatedByUserAccountId = user!.Id;
        newsModel.UpdatedByUserAccountId = user.Id;
        newsModel.CreatedAt = DateTime.Now;
        newsModel.DeletedAt = null;
        
        var result = await UnitOfWork.NewsRepository.CreateNewsAsync(newsModel);

        if (result.Succeeded)
            return _mapper.Map<CreateNewsResponseDto>(result.Value); 
        
        throw new BadRequestException(
            result.Errors.FirstOrDefault() 
            ?? "There was an error creating the news"
        );
    }
}