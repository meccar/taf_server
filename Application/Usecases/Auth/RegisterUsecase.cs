using Application.Commands.Auth.Register;
using Application.Dtos.Authentication.Register;
using AutoMapper;
using Domain.Usecase;
using MediatR;

namespace Application.Usecases.Auth;
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
