using Application.Dtos.Authentication.Register;
using Application.Dtos.UserAccount;
using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Model;

namespace Application.Mapper;

public static class UserTokenMapper
{
    /// <summary>
    /// Configures the mappings for user account models and entities.
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserTokenModel, UserTokenEntity>();
        config.CreateMap<UserTokenEntity, UserTokenModel>();
    }
}