﻿using Application.Commands.Auth.Register;
using AutoMapper;
using Domain.Usecase;
using MediatR;
using Shared.Dtos.Authentication.Register;
using Shared.Model;

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
        var userProfileModel = _mapper.Map<UserProfileModel>(request.UserProfile);
        var userAccountModel = _mapper.Map<UserAccountModel>(request.UserAccount);
        
        var registerResponse = await _mediator.Send(new RegisterCommand(userProfileModel, userAccountModel));
        
        return _mapper.Map<RegisterUserResponseDto>(registerResponse);
    }
}
