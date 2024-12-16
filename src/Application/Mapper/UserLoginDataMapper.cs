using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Shared.Model;

namespace Application.Mapper;

/// <summary>
/// Provides the mapping configurations for user login data models and aggregates.
/// This class configures the mapping between the <see cref="UserAccountModel"/> and the <see cref="UserAccountAggregate"/>,
/// ensuring that the data related to user login is mapped appropriately between domain models and view models.
/// </summary>
public static class UserLoginDataMapper
{
    /// <summary>
    /// Configures the AutoMapper mappings for user login data models and aggregates.
    /// This method sets up the mappings between the <see cref="UserAccountModel"/> and the <see cref="UserAccountAggregate"/>,
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    /// <remarks>
    /// The method defines mappings that facilitate the conversion of data between the <see cref="UserAccountModel"/> and
    /// <see cref="UserAccountAggregate"/>. These mappings are important for ensuring that sensitive data, like passwords,
    /// are properly excluded from certain models. This also enables easier manipulation of the data when performing login-related operations.
    /// </remarks>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
    }
}