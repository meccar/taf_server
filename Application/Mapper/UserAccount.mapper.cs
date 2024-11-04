using Application.Dtos.Authentication.Register;
using Application.Dtos.UserAccount;
using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Model;

namespace Application.Mapper;

/// <summary>
/// Provides mapping configurations for user account-related entities and data transfer objects (DTOs).
/// </summary>
/// <remarks>
/// This class is responsible for defining the mappings between <see cref="UserAccountModel"/> and <see cref="UserAccountEntity"/> 
/// as well as between <see cref="CreateUserAccountDto"/> and other models. It utilizes AutoMapper to facilitate object-to-object 
/// mapping, enabling seamless transformation between different representations of user account data.
/// </remarks>
public static class UserAccountMapper
{
    /// <summary>
    /// Configures the mappings for user account models and entities.
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserAccountAggregate, UserAccountModel>();
        config.CreateMap<CreateUserAccountDto, UserAccountModel>();
        config.CreateMap<CreateUserAccountDto, UserAccountAggregate>();
        config.CreateMap<UserAccountModel, UserAccountAggregate>()
            .ForMember(dest => dest.Uuid, opt => opt.Ignore());
            // .ForMember(dest => dest.UserLoginData, opt => opt.Ignore());
        config.CreateMap<UserAccountModel, RegisterUserResponseDto>();
        // config.CreateMap<UserAccountModel, UserAccountResponseDto>();
    }
}