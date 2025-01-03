using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.UserAccount;

namespace Application.Commands.UserAccount;

public class UpdateUserAccountCommandHandler : TransactionalCommandHandler<UpdateUserAccountCommand, UpdateUserAccountResponseDto>
{
    private readonly IMapper _mapper;
    
    public UpdateUserAccountCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<UpdateUserAccountResponseDto> ExecuteCoreAsync(UpdateUserAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.UserAccountRepository.GetCurrentUser();

        if (user is null)
            throw new UnauthorizedException("You do not have permission to update this user");
        
        if (user.EId != request.Eid)
            throw new UnauthorizedException("You do not have permission to update this user");
        
        var userAccountAggregate = _mapper.Map<UserAccountAggregate>(request);
        
        // var result = await UnitOfWork.UserAccountRepository.UpdateUserAccountAsync(userAccountAggregate);
        var result = await UnitOfWork.UserAccountRepository.UpdateAsync(userAccountAggregate);

        return result.Succeeded
            ? _mapper.Map<UpdateUserAccountResponseDto>(userAccountAggregate)
            : throw new BadRequestException(
                result.Errors.FirstOrDefault()!);
    }
}