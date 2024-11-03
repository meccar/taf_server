using Application.Dtos.Authentication.Login;
using Application.Queries.Auth.Login;
using AutoMapper;
using Domain.Usecase;
using MediatR;

namespace Application.Usecases.Auth;
public class LoginUsecase : IUseCase<LoginUserRequestDto, LoginResponseDto>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LoginUsecase(IMediator mediator, IMapper mapper)
    {
        _mediator=mediator;
        _mapper=mapper;
    }

    public async Task<LoginResponseDto> Execute(LoginUserRequestDto request)
    {
        var loginResponse = await _mediator.Send(new LoginQuery(request));
        return _mapper.Map<LoginResponseDto>(loginResponse);
    }
}
