using AutoMapper;
using MediatR;
using taf_server.Application.Commands.Auth.Register;
using taf_server.Domain.Usecase;
using taf_server.Presentations.Dtos.Authentication.Register;

namespace taf_server.Application.Usecases.Auth;
public class RegisterUsecase : IUseCase<RegisterUserRequestDto, RegisterUserResponseDto>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RegisterUsecase(IMediator mediator, IMapper mapper)
    {
        _mediator=mediator;
        _mapper=mapper;
    }

    public async Task<RegisterUserResponseDto> Execute(RegisterUserRequestDto request)
    {
        var registerResponse = await _mediator.Send(new RegisterCommand(request));
        return _mapper.Map<RegisterUserResponseDto>(registerResponse);
    }
}
