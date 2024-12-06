using Shared.FileObjects;

namespace Shared.Enums;

/// <summary>
/// Represents roles and their associated claims in the system.
/// </summary>
public class ERoleWithClaims
{
    /// <summary>
    /// A dictionary that maps roles to their corresponding claims.
    /// Each role is associated with a set of actions that can be performed.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> RoleClaims = 
        new Dictionary<string, IReadOnlyCollection<string>>
        {
            { 
                FoRole.Admin, 
                new[] 
                {
                    FoClaimeActionsValue.View,
                    FoClaimeActionsValue.Read,
                    FoClaimeActionsValue.Update,
                    FoClaimeActionsValue.Delete
                }
            },
            { 
                FoRole.CompanyManager, 
                new[] 
                {
                    FoClaimeActionsValue.View,
                    FoClaimeActionsValue.Read,
                    FoClaimeActionsValue.Update
                }
            },
            { 
                FoRole.CompanyUser, 
                new[] 
                {
                    FoClaimeActionsValue.View,
                    FoClaimeActionsValue.Read,
                    FoClaimeActionsValue.Update
                }
            },
            { 
                FoRole.User, 
                new[] 
                {
                    FoClaimeActionsValue.View,
                    FoClaimeActionsValue.Read,
                    FoClaimeActionsValue.Update
                }
            },
        };
}