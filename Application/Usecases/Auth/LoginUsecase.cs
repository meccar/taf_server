using AutoMapper;
using MediatR;
using taf_server.Application.Queries.Auth.Login;
using taf_server.Domain.Usecase;
using taf_server.Presentations.Dtos.Authentication.Login;

namespace taf_server.Application.Usecases.Auth;
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
