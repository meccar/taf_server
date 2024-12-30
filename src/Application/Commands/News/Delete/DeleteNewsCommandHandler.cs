using AutoMapper;
using Domain.Interfaces;
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

        if (getCurrentUserResult == null)
            throw new UnauthorizedException("You are not logged in");

        var newsAggregate = await UnitOfWork
            .NewsRepository
            .GetDetailNewsAsync(request.NewsEid);
        
        if(!newsAggregate.Succeeded)
            throw new NotFoundException(newsAggregate.Errors.FirstOrDefault()!);

        if(newsAggregate.Value!.CreatedByUserAccountId != getCurrentUserResult.Id)
            throw new UnauthorizedException("You do not have permission to delete this news!");

        var softDeleteNewsResult = await UnitOfWork
            .NewsRepository
            .SoftDeleteAsync(newsAggregate.Value);
        
       return softDeleteNewsResult.Succeeded
           ? new SuccessResponseDto(true)
           : throw new BadRequestException(softDeleteNewsResult.Errors.FirstOrDefault()!);
    }
}