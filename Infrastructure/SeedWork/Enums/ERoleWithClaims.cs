namespace Infrastructure.SeedWork.Enums;

public class ERoleWithClaims
{
    public static readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> RoleClaims = 
        new Dictionary<string, IReadOnlyCollection<string>>
        {
            { 
                ERole.Admin, 
                new[] 
                {
                    EClaimValue.View,
                    EClaimValue.Read,
                    EClaimValue.Update,
                    EClaimValue.Delete
                }
            },
            { 
                ERole.CompanyManager, 
                new[] 
                {
                    EClaimValue.View,
                    EClaimValue.Read,
                    EClaimValue.Update
                }
            },
            { 
                ERole.CompanyUser, 
                new[] 
                {
                    EClaimValue.View,
                    EClaimValue.Read,
                    EClaimValue.Update
                }
            },
            { 
                ERole.User, 
                new[] 
                {
                    EClaimValue.View,
                    EClaimValue.Read,
                    EClaimValue.Update
                }
            },
        };
}