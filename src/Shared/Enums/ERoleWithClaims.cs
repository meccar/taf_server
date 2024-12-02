using Shared.FileObjects;

namespace Shared.Enums;

public class ERoleWithClaims
{
    public static readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> RoleClaims = 
        new Dictionary<string, IReadOnlyCollection<string>>
        {
            { 
                FORole.Admin, 
                new[] 
                {
                    FOClaimeActionsValue.View,
                    FOClaimeActionsValue.Read,
                    FOClaimeActionsValue.Update,
                    FOClaimeActionsValue.Delete
                }
            },
            { 
                FORole.CompanyManager, 
                new[] 
                {
                    FOClaimeActionsValue.View,
                    FOClaimeActionsValue.Read,
                    FOClaimeActionsValue.Update
                }
            },
            { 
                FORole.CompanyUser, 
                new[] 
                {
                    FOClaimeActionsValue.View,
                    FOClaimeActionsValue.Read,
                    FOClaimeActionsValue.Update
                }
            },
            { 
                FORole.User, 
                new[] 
                {
                    FOClaimeActionsValue.View,
                    FOClaimeActionsValue.Read,
                    FOClaimeActionsValue.Update
                }
            },
        };
}