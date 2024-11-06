using Application.Dtos.UserLoginData;
using AutoMapper;
using Domain.Entities;
using Domain.Model;

namespace Application.Mapper;

/// <summary>
/// Provides mapping configurations for user login data-related entities and data transfer objects (DTOs).
/// </summary>
/// <remarks>
/// This class defines the mappings between <see cref="UserLoginDataModel"/> and <see cref="UserLoginDataEntity"/> 
/// as well as between <see cref="CreateUserLoginDataDto"/> and the corresponding models. 
/// It utilizes AutoMapper to facilitate seamless object-to-object mapping, enabling efficient transformation 
/// between different representations of user login data.
/// </remarks>
public static class UserLoginDataMapper
{
    /// <summary>
    /// Configures the mappings for user login data models and entities.
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserLoginDataModel, UserLoginDataEntity>()
            .ForMember(dest => dest.Uuid, opt => opt.Ignore());
        config.CreateMap<UserLoginDataEntity, UserLoginDataModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
        config.CreateMap<CreateUserLoginDataDto, UserLoginDataModel>();
        config.CreateMap<CreateUserLoginDataDto, UserLoginDataEntity>();
        config.CreateMap<UserLoginDataModel, UserLoginDataResponseDto>();
    }
}