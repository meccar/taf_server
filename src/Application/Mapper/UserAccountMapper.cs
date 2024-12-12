using AutoMapper;
using Domain.Aggregates;
using Shared.Model;

namespace Application.Mapper;

/// <summary>
/// Provides the mapping configurations for user account models and aggregates.
/// This class configures the mapping between the <see cref="UserProfileAggregate"/> and the <see cref="UserProfileModel"/>.
/// It ensures that data can be transformed between these domain models and their corresponding view models.
/// </summary>
public static class UserAccountMapper
{
    /// <summary>
    /// Configures the AutoMapper mappings for user account models and aggregates.
    /// This method sets up the mappings between the <see cref="UserProfileAggregate"/> and the <see cref="UserProfileModel"/>,
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    /// <remarks>
    /// This method defines mappings that are used when transforming data between the aggregate models and the DTOs 
    /// used in the application's business logic and API layer. It also handles any specific transformations like ignoring 
    /// </remarks>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        // Map UserProfileAggregate to UserProfileModel, and map the UserAccount property
        config.CreateMap<UserProfileAggregate, UserProfileModel>()
            .ForMember(dest => dest.UserAccount, opt => opt.MapFrom(src => src.UserAccount));

        // Map UserProfileModel to UserProfileAggregate, ignoring the EId property
        config.CreateMap<UserProfileModel, UserProfileAggregate>()
            .ForMember(dest => dest.EId, opt => opt.Ignore());
    }
}