using Application.Commands.Auth.Register;
using AutoMapper;
using Domain.Aggregates;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.UserAccount;
using Shared.Dtos.UserProfile;
using Shared.Model;

namespace Application.Mapper;

/// <summary>
/// Provides mapping configurations for authentication-related models and DTOs.
/// This class contains the mappings between various data transfer objects (DTOs) and domain models
/// related to user profiles, user accounts, authentication credentials, and login responses.
/// </summary>
public static class AuthMapper
{
    /// <summary>
    /// Configures the AutoMapper mappings for authentication-related models and DTOs.
    /// This method sets up mappings between DTOs such as <see cref="CreateUserProfileDto"/> and 
    /// <see cref="UserProfileModel"/>, <see cref="CreateUserAccountDto"/> and <see cref="UserAccountModel"/>, 
    /// as well as mappings for authentication responses like login tokens and user profile data.
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to define the mappings.</param>
    /// <remarks>
    /// The method defines mappings that are used throughout the application for user registration, login, 
    /// and response generation. These mappings help transform data between the DTOs and domain models, ensuring
    /// consistency and ease of data manipulation.
    /// </remarks>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        // Map CreateUserProfileDto to UserProfileModel
        config.CreateMap<CreateUserProfileDto, UserProfileModel>();
        // Map CreateUserAccountDto to UserAccountModel
        config.CreateMap<CreateUserAccountDto, UserAccountModel>();
        
        // Map UserProfileModel to RegisterUserResponseDto
        config.CreateMap<UserProfileModel, RegisterUserResponseDto>();
        // Map UserAccountModel to UserAccountResponseDto
        config.CreateMap<UserAccountModel, UserAccountResponseDto>();
        
        // Map LoginUserRequestDto to UserAccountModel for login request
        config.CreateMap<LoginUserRequestDto, UserAccountModel>();
        // Map TokenModel to LoginResponseDto for returning a token-based response
        config.CreateMap<TokenModel, LoginResponseDto>();
        // Map TokenModel to UserProfileResponseDto
        config.CreateMap<TokenModel, UserProfileResponseDto>();
        // Map TokenModel to VerifyUserEmailRequestDto for verifying user email
        config.CreateMap<TokenModel, VerifyUserResponseDto>();
        
        config.CreateMap<RegisterCommand, UserProfileAggregate>();
        config.CreateMap<UserProfileAggregate, RegisterUserResponseDto>();
        config.CreateMap<RegisterCommand, UserAccountAggregate>();
        config.CreateMap<UserAccountAggregate, UserAccountResponseDto>();
    }
}