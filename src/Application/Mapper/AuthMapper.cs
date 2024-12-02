using AutoMapper;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.UserAccount;
using Shared.Dtos.UserLoginData;
using Shared.Model;

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