using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Shared.Model;

namespace Application.Mapper;

/// <summary>
/// Provides mapping configurations for user login data-related entities and data transfer objects (DTOs).
/// </summary>
/// <remarks>
/// This class defines the mappings between <see cref="UserLoginDataModel"/> and <see cref="UserAccountAggregate"/> 
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
        config.CreateMap<UserAccountModel, UserAccountAggregate>()
            .ForMember(dest => dest.EId, opt => opt.Ignore());
        config.CreateMap<UserAccountAggregate, UserAccountModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}