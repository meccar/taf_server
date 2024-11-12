using Application.Dtos.Authentication.Credentials;
using Application.Dtos.Authentication.Login;
using Application.Dtos.Authentication.Register;
using Application.Dtos.UserAccount;
using Application.Dtos.UserLoginData;
using AutoMapper;
using Domain.Aggregates;
using Domain.Model;

namespace Application.Mapper;

public static class AuthMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<CreateUserAccountDto, UserAccountModel>();
        config.CreateMap<CreateUserLoginDataDto, UserLoginDataModel>();
        config.CreateMap<LoginUserRequestDto, UserLoginDataModel>();
        config.CreateMap<UserAccountModel, RegisterUserResponseDto>();
        config.CreateMap<UserLoginDataModel, UserLoginDataResponseDto>();
        config.CreateMap<TokenModel, LoginResponseDto>();
        config.CreateMap<TokenModel, UserAccountResponseDto>();

    }
}