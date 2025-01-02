using AutoMapper;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Dtos.Exceptions;

namespace Application.Commands.News.Delete;

public class DeleteNewsCommandHandler 
    : TransactionalCommandHandler<DeleteNewsCommand, SuccessResponseDto>
{
    private readonly IMapper _mapper;
    
    public DeleteNewsCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<SuccessResponseDto> ExecuteCoreAsync(
        DeleteNewsCommand request,
        CancellationToken cancellationToken)
    {
        var getCurrentUserResult = await UnitOfWork
            .UserAccountRepository
            .GetCurrentUser();

        if (getCurrentUserResult is null)
            throw new UnauthorizedException("You are not logged in");

        var newsAggregate = await UnitOfWork
            .NewsRepository
            .FindByCondition(x => x.Uuid == request.NewsEid, true)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if(newsAggregate is null)
            throw new NotFoundException("News not found");

        if(newsAggregate.CreatedByUserAccountId != getCurrentUserResult.Id)
            throw new UnauthorizedException("You do not have permission to delete this news!");

        newsAggregate.DeletedAt = DateTime.Now;
        newsAggregate.IsDeleted = true;
        
        var softDeleteNewsResult = await UnitOfWork
            .NewsRepository
            .UpdateAsync(newsAggregate);
        
       return softDeleteNewsResult is not null
           ? new SuccessResponseDto(true)
           : throw new BadRequestException("There was an error processing your request");
    }
}