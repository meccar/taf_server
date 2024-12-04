using Application.Queries.Auth.VerifyUser;
using AutoMapper;
using Domain.Usecase;
using MediatR;
using Shared.Dtos.Authentication.Credentials;

namespace Application.Usecases.Auth;

public class VerifyUserUsecase : IUseCase<VerifyUserRequestDto, VerifyUserRequestDto>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public VerifyUserUsecase(
        IMediator mediator,
        IMapper mapper
    )
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    public async Task<VerifyUserRequestDto> Execute(VerifyUserRequestDto request)
    {
        var loginResponse = await _mediator.Send(new VerifyUserQuery(request.Token));
        
        return _mapper.Map<VerifyUserRequestDto>(loginResponse);
    }
}