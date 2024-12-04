using AutoMapper;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.UserAccount;
using Shared.Dtos.UserProfile;
using Shared.Model;

namespace Application.Mapper;

public static class AuthMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<CreateUserProfileDto, UserProfileModel>();
        config.CreateMap<CreateUserAccountDto, UserAccountModel>();
        
        config.CreateMap<UserProfileModel, RegisterUserResponseDto>();
        config.CreateMap<UserAccountModel, UserAccountResponseDto>();
        
        config.CreateMap<LoginUserRequestDto, UserAccountModel>();
        config.CreateMap<TokenModel, LoginResponseDto>();
        config.CreateMap<TokenModel, UserProfileResponseDto>();
        config.CreateMap<TokenModel, VerifyUserRequestDto>();
    }
}