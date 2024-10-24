using AutoMapper;
using taf_server.Domain.Model;
using taf_server.Infrastructure.Entities;
using taf_server.Presentations.Dtos.UserAccount;
using taf_server.Presentations.Dtos.UserLoginData;

namespace taf_server.Infrastructure.Mapper;

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
        config.CreateMap<UserLoginDataModel, UserLoginDataEntity>();
        config.CreateMap<UserLoginDataEntity, UserLoginDataModel>();
        config.CreateMap<CreateUserLoginDataDto, UserLoginDataModel>();
        config.CreateMap<CreateUserLoginDataDto, UserLoginDataEntity>();
        config.CreateMap<UserLoginDataEntity, UserLoginDataModel>();
    }
}